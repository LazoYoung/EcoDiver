using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

namespace Script.Interaction
{
    public class DiverMotionController : MonoBehaviour
    {
        [Tooltip("Button used to trigger swimming motion")]
        public InputActionReference triggerButton;
        
        [Tooltip("(Optional) XR Origin")]
        public GameObject origin;

        [Tooltip("(Optional) GameObject which simulates the left-hand controller")]
        public GameObject leftController;
        
        [Tooltip("(Optional) GameObject which simulates the right-hand controller")]
        public GameObject rightController;
        
        [Tooltip("(Optional) Position of XR left-hand controller")]
        public InputActionReference leftHandPosition;
        
        [Tooltip("(Optional) Position of XR right-hand controller")]
        public InputActionReference rightHandPosition;

        public bool experiment;
        
        private InputDevice _leftHand;
        
        private InputDevice _rightHand;
        
        private bool _backup;

        private void Awake()
        {
            InitInputDevices();
            triggerButton.action.performed += OnTrigger;
        }

        private void OnDestroy()
        {
            triggerButton.action.performed -= OnTrigger;
        }

        private void FixedUpdate()
        {
            Vector3 leftPoint, rightPoint;
        
            if (experiment)
            {
                leftPoint = leftHandPosition.action.ReadValue<Vector3>();
                rightPoint = rightHandPosition.action.ReadValue<Vector3>();
            }
            else if (_backup)
            {
                var leftGlobalPos = leftController.transform.position;
                var rightGlobalPos = rightController.transform.position;
                var parent = origin.transform;
                leftPoint = parent.InverseTransformPoint(leftGlobalPos);
                rightPoint = parent.InverseTransformPoint(rightGlobalPos);
            }
            else
            {
                bool success = _leftHand.TryGetFeatureValue(CommonUsages.devicePosition, out leftPoint);
                success &= _rightHand.TryGetFeatureValue(CommonUsages.devicePosition, out rightPoint);

                if (!success)
                {
                    Debug.LogWarning("Failed to retrieve feature value of an InputDevice.");
                    return;
                }
            }

            ApplyMotion(leftPoint, rightPoint);
        }

        private void ApplyMotion(Vector3 leftPoint, Vector3 rightPoint)
        {
            // todo method stub
            Debug.Log(leftPoint);
        }
    
        private void InitInputDevices()
        {
            if (experiment)
            {
                if (leftHandPosition != null && rightHandPosition != null)
                    return;
                
                Debug.LogWarning("Required components are missing! Reverting to normal mode...");
                experiment = false;
            }
            
            _leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            _rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        
            if (!_leftHand.isValid || !_rightHand.isValid)
            {
                Debug.LogWarning("XR hand device not found! Reverting to backup mode...");
                _backup = true;
                
                if (origin == null || leftController == null || rightController == null)
                {
                    Debug.LogError("Required components are missing!");
                    enabled = false;
                }
            }
        }

        private void OnTrigger(InputAction.CallbackContext context)
        {
            Debug.Log("Motion button triggered.");
        }
    }
}
