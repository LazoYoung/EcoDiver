using System.Collections.Generic;
using Crest;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RigidbodyConstraints;
using Vector3 = UnityEngine.Vector3;

namespace Script.Interaction
{
    internal struct RigidbodyState
    {
        private RigidbodyConstraints _constraints;
        private bool _useGravity;
        private float _drag;
        private float _angularDrag;

        internal void Save(Rigidbody body)
        {
            _constraints = body.constraints;
            _drag = body.drag;
            _useGravity = body.useGravity;
            _angularDrag = body.angularDrag;
        }
        
        internal void Restore(Rigidbody body)
        {
            body.constraints = _constraints;
            body.drag = _drag;
            body.useGravity = _useGravity;
            body.angularDrag = _angularDrag;
        }
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public class DiverController : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Access to controller input manager.")]
        [SerializeField] private InputController input;
        [Tooltip("Defines the forward direction to propel. Recommended: Main Camera")]
        [SerializeField] private Transform forwardReference;
        
        [Header("Controls")]
        [Tooltip("Player should press and hold the buttons to swim.")]
        [SerializeField] private bool triggerMode;
        [Tooltip("This button maps to the left-hand motion.")]
        [SerializeField] private InputActionReference leftButton;
        [Tooltip("This button maps to the right-hand motion.")]
        [SerializeField] private InputActionReference rightButton;

        [Header("Physics")]
        [Tooltip("Amount of force applied to propel underwater.")]
        [SerializeField] private float swimForce = 3f;
        [Tooltip("Amount of torque applied to spin around underwater.")]
        [SerializeField] private float spinTorque = 0.01f;
        [Tooltip("Amount of force to simulate fluid resistance.")]
        [SerializeField] private float dragForce = 0.3f;
        [Tooltip("Vertical speed sinking underwater.")]
        [SerializeField] private float sinkSpeed = 1f;
        [Tooltip("Minimum amount of input for controller deadzone.")]
        [SerializeField] private float minInput = 0.5f;
        [Tooltip("Cooldown in seconds before the next input is allowed.")]
        [SerializeField] private float cooldown = 0.3f;

        [Header("Experiment")]
        [Tooltip("Print verbose output to Console.")]
        [SerializeField] private bool verbose;
        [Tooltip("Force underwater physics to persist above sea level. Testing purpose only!")]
        [SerializeField] private bool forceUnderwater;

        private const float SinkForce = 0.5f;
        private const float SpinDrag = 1.0f;
        private Rigidbody _rigidbody;
        private RigidbodyState _rstate;
        private float _timer;
        private bool _inPropel;
        private bool _inSpin;
        private bool _underwater;

        private void Awake()
        {
            if (!ValidateComponents())
            {
                enabled = false;
                return;
            }

            _underwater = forceUnderwater;
            _rigidbody = GetComponent<Rigidbody>();
            _rstate.Save(_rigidbody);
            UpdateRigidbody();
        }

        private void OnDestroy()
        {
            _underwater = false;
            _rstate.Restore(_rigidbody);
        }

        private void OnCollisionEnter()
        {
            // stop player from spinning after ground collision
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (!forceUnderwater)
            {
                _underwater = OceanRenderer.Instance && OceanRenderer.Instance.ViewerHeightAboveWater < 1f;
            }
            
            UpdateRigidbody();
            
            if (_timer < cooldown)
            {
                _timer += Time.fixedDeltaTime;
            }
            else if (_underwater)
            {
                UpdateMotion();
            }
        }

        private void UpdateMotion()
        {
            var leftForce = input.TranslateVelocity(input.leftHandVelocity);
            var rightForce = input.TranslateVelocity(input.rightHandVelocity);
            var leftPower = IsForceSufficient(leftForce);
            var rightPower = IsForceSufficient(rightForce);
            var leftTrigger = !triggerMode || leftButton.action.IsPressed();
            var rightTrigger = !triggerMode || rightButton.action.IsPressed();
            var left = leftPower && leftTrigger;
            var right = rightPower && rightTrigger;
            var noise = (leftPower ^ leftTrigger) | (rightPower ^ rightTrigger);
            
            if (!noise && left && right)
            {
                PropelForward(leftForce, rightForce);
            }
            else if (!noise && left ^ right)
            {
                Spin(left ? leftForce : rightForce);
            }
            else if (_inPropel || _inSpin)
            {
                _timer = 0;
                _inPropel = false;
                _inSpin = false;
            }
            else if (_rigidbody.velocity.y < sinkSpeed)
            {
                var sinkForce = dragForce + SinkForce;
                _rigidbody.AddForce(sinkForce * Vector3.down);
            }
        }

        private void Spin(Vector3 force)
        {
            var localInputForce = force;
            var unitXZ = Vector3.ProjectOnPlane(localInputForce, Vector3.up).normalized;
            var factor = Vector3.Dot(unitXZ, Vector3.left);

            if (_inPropel || Mathf.Abs(factor) < 0.2f)
                return;

            var torque = factor * spinTorque * forwardReference.up;
                
            _rigidbody.AddTorque(torque, ForceMode.Force);
            _inSpin = true;

            if (verbose)
            {
                var mode = factor > 0 ? 'R' : 'L';
                Debug.Log($"Swim action: rotate {mode} with factor {factor}");
            }
        }

        private void PropelForward(Vector3 leftForce, Vector3 rightForce)
        {
            var localInputForce = leftForce + rightForce;
            var unitXZ = Vector3.ProjectOnPlane(localInputForce, Vector3.up).normalized;
            var factor = Mathf.Max(0, Vector3.Dot(unitXZ, Vector3.back));

            if (_inSpin || factor < 0.2f)
                return;
            
            var inputForce = forwardReference.TransformVector(localInputForce);
            var stroke = -1 * inputForce.normalized;
            var facing = forwardReference.forward;
            var force = factor * swimForce * Vector3.Slerp(stroke, facing, 0.5f);
                    
            _rigidbody.AddForce(force, ForceMode.Force);
            _inPropel = true;
                    
            if (verbose)
                Debug.Log($"Swim action: propel with factor {factor}");
        }

        private bool IsForceSufficient(Vector3 force)
        {
            return force.sqrMagnitude > minInput * minInput;
        }
        
        private void UpdateRigidbody()
        {
            if (_underwater)
            {
                // ReSharper disable once BitwiseOperatorOnEnumWithoutFlags
                _rigidbody.constraints = FreezeRotationX | FreezeRotationZ;
                _rigidbody.useGravity = false;
                _rigidbody.drag = dragForce;
                _rigidbody.angularDrag = SpinDrag;
            }
            else
            {
                _rstate.Restore(_rigidbody);
            }
        }
        
        private bool ValidateComponents()
        {
            var blame = new List<string>();

            if (!input || !input.enabled)
                blame.Add("InputController");

            if (!forwardReference)
                blame.Add("ForwardReference");
            
            if (triggerMode && !leftButton)
                blame.Add("LeftButton");
            
            if (triggerMode && !rightButton)
                blame.Add("RightButton");
            
            foreach (var item in blame)
                Debug.LogError("DiverController found a component missing: " + item);

            return blame.Count == 0;
        }
    }
}
