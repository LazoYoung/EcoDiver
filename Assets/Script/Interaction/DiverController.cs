using System.Collections.Generic;
using Crest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    
    public class DiverController : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Access to controller input manager.")]
        [SerializeField] private InputManager input;
        [Tooltip("Defines the forward direction to propel. Recommended: Main Camera")]
        [SerializeField] private Transform forwardReference;
        [Tooltip("Rigidbody of this player.")]
        [SerializeField] private Rigidbody rigidBody;
        
        [Header("Controls")]
        [Tooltip("Player should press and hold the buttons to swim.")]
        [SerializeField] private bool triggerMode;
        [Tooltip("This button maps to the left-hand motion.")]
        [SerializeField] private InputActionReference leftButton;
        [Tooltip("This button maps to the right-hand motion.")]
        [SerializeField] private InputActionReference rightButton;

        [Header("Physics")]
        [Tooltip("Amount of force applied to propel underwater.")]
        [SerializeField] private float swimForce = 4f;
        [FormerlySerializedAs("spinTorque")]
        [Tooltip("Angular speed applied to spin underwater.")]
        [SerializeField] private int spinSpeed = 30;
        [Tooltip("Amount of force to simulate fluid resistance.")]
        [SerializeField] private float dragForce = 0.2f;
        [Tooltip("Vertical speed sinking underwater.")]
        [SerializeField] private float sinkSpeed = 0.1f;
        [Tooltip("Minimum amount of input for controller deadzone.")]
        [SerializeField] private float minInput = 0.5f;
        [Tooltip("Cooldown in seconds before the next input is allowed.")]
        [SerializeField] private float cooldown = 0.3f;

        [Header("Experiment")]
        [Tooltip("Print verbose output to Console.")]
        [SerializeField] private bool verbose;
        [Tooltip("Force underwater physics to persist above sea level. Testing purpose only!")]
        [SerializeField] private bool forceUnderwater;

        private const float SinkForce = 0.3f;
        private RigidbodyState _rstate;
        private Vector3 _spinVelocity;
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
            _spinVelocity = new Vector3(0, spinSpeed, 0);
            rigidBody.automaticCenterOfMass = false;
            _rstate.Save(rigidBody);
            UpdateRigidbody();
        }

        private void OnDestroy()
        {
            _underwater = false;
            _rstate.Restore(rigidBody);
        }

        private void OnCollisionEnter()
        {
            // stop player from spinning after ground collision
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
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
            bool leftPower = IsForceSufficient(leftForce);
            bool rightPower = IsForceSufficient(rightForce);
            bool leftTrigger = !triggerMode || leftButton.action.IsPressed();
            bool rightTrigger = !triggerMode || rightButton.action.IsPressed();
            bool left = leftPower && leftTrigger;
            bool right = rightPower && rightTrigger;
            bool noise = (leftPower ^ leftTrigger) | (rightPower ^ rightTrigger);
            
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
            else if (rigidBody.velocity.y < sinkSpeed)
            {
                float sinkForce = dragForce + SinkForce;
                rigidBody.AddForce(sinkForce * Vector3.down);
            }
        }

        private void Spin(Vector3 force)
        {
            var localInputForce = force;
            var unitXZ = Vector3.ProjectOnPlane(localInputForce, Vector3.up).normalized;
            float factor = Vector3.Dot(unitXZ, Vector3.left);

            if (_inPropel || Mathf.Abs(factor) < 0.2f)
                return;

            var delta = Quaternion.Euler(_spinVelocity * (factor * Time.fixedDeltaTime));
            rigidBody.MoveRotation(rigidBody.rotation * delta);
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
            float factor = Mathf.Max(0, Vector3.Dot(unitXZ, Vector3.back));

            if (_inSpin || factor < 0.2f)
                return;
            
            var inputForce = forwardReference.TransformVector(localInputForce);
            var stroke = -1 * inputForce.normalized;
            var facing = forwardReference.forward;
            var force = factor * swimForce * Vector3.Slerp(stroke, facing, 0.5f);
                    
            rigidBody.AddForce(force, ForceMode.Force);
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
                rigidBody.constraints = FreezeRotation;
                rigidBody.useGravity = false;
                rigidBody.drag = dragForce;
            }
            else
            {
                _rstate.Restore(rigidBody);
            }
        }
        
        private bool ValidateComponents()
        {
            var blame = new List<string>();

            if (rigidBody == null)
                blame.Add("RigidBody");
            
            if (input == null || !input.enabled)
                blame.Add("InputController");

            if (forwardReference == null)
                blame.Add("ForwardReference");
            
            if (triggerMode && leftButton == null)
                blame.Add("LeftButton");
            
            if (triggerMode && rightButton == null)
                blame.Add("RightButton");
            
            foreach (string item in blame)
                Debug.LogError("DiverController found a component missing: " + item);

            return blame.Count == 0;
        }
    }
}
