using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

namespace Script.Interaction
{
    public class InputController : MonoBehaviour
    {
        [Header("Tracking source")]
        [Tooltip("Use .inputactions to obtain XR tracking data.")]
        public bool useReference;
        
        public InputActionAsset inputActions;

        [Header("Debug")]
        public bool debugMode;
        
        [Tooltip("Text used to show controller position")]
        public TMP_Text leftHandPositionText;
        
        [Tooltip("Text used to show controller position")]
        public TMP_Text rightHandPositionText;

        [Tooltip("Text used to show controller velocity")]
        public TMP_Text leftHandVelocityText;

        [Tooltip("Text used to show controller velocity")]
        public TMP_Text rightHandVelocityText;

        public Vector3 leftHandPosition { get; private set; } = new(0, 0, 0);
        public Vector3 rightHandPosition { get; private set; } = new(0, 0, 0);
        public Vector3 leftHandVelocity { get; private set; } = new(0, 0, 0);
        public Vector3 rightHandVelocity { get; private set; } = new(0, 0, 0);
        
        private InputDevice _leftHandDevice;
        private InputDevice _rightHandDevice;
        private InputAction _leftHandPositionAction;
        private InputAction _rightHandPositionAction;
        private InputAction _leftHandVelocityAction;
        private InputAction _rightHandVelocityAction;
        
        private void Awake()
        {
            InitDebugStates();

            try
            {
                LoadInputDevices();
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to access XR device: " + e.Message);
                enabled = false;
            }
        }

        private void OnDestroy()
        {
            InitDebugStates();
        }
        
        private void Update()
        {
            if (useReference)
            {
                leftHandPosition = _leftHandPositionAction.ReadValue<Vector3>();
                rightHandPosition = _rightHandPositionAction.ReadValue<Vector3>();
                leftHandVelocity = _leftHandVelocityAction.ReadValue<Vector3>();
                rightHandVelocity = _rightHandVelocityAction.ReadValue<Vector3>();
            }
            else
            {
                if (_leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var leftPos))
                    leftHandPosition = leftPos;

                if (_leftHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out var leftVel))
                    leftHandVelocity = leftVel;

                if (_rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var rightPos))
                    rightHandPosition = rightPos;

                if (_rightHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out var rightVel))
                    rightHandVelocity = rightVel;
            }

            if (debugMode)
            {
                RelayControllerInfo();
            }
        }
        
        private void LoadInputDevices()
        {
            if (useReference)
            {
                if (inputActions == null)
                {
                    Debug.LogWarning("Required component is missing! Reverting to normal mode...");
                    useReference = false;
                }
                
                var leftHand = inputActions.FindActionMap("XRI LeftHand", true);
                var rightHand = inputActions.FindActionMap("XRI RightHand", true);
                _leftHandPositionAction = leftHand.FindAction("Position", true);
                _rightHandPositionAction = rightHand.FindAction("Position", true);
                _leftHandVelocityAction = leftHand.FindAction("Velocity", true);
                _rightHandVelocityAction = rightHand.FindAction("Velocity", true);
                
                if (!_rightHandVelocityAction.enabled)
                {
                    Debug.LogWarning("Failed to obtain device velocity! Confirm XR Device Simulator is OFF?");
                }
            }
            else
            {
                _leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                _rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                
                if (!_leftHandDevice.isValid || !_rightHandDevice.isValid)
                {
                    throw new UnityException("XR device not found!");
                }
            }
        }
        
        private void InitDebugStates()
        {
            if (!debugMode)
                return;
                
            if (leftHandPositionText == null || rightHandPositionText == null ||
                leftHandVelocityText == null || rightHandVelocityText == null)
            {
                debugMode = false;
            }
            else
            {
                leftHandPositionText.text = "N/A";
                rightHandPositionText.text = "N/A";
                leftHandVelocityText.text = "N/A";
                rightHandVelocityText.text = "N/A";
            }
        }
        
        private void RelayControllerInfo()
        {
            leftHandPositionText.text = leftHandPosition.ToString();
            rightHandPositionText.text = rightHandPosition.ToString();
            leftHandVelocityText.text = leftHandVelocity.ToString();
            rightHandVelocityText.text = rightHandVelocity.ToString();
        }
    }
}
