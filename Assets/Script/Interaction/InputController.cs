using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

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
        
        [Tooltip("Text used to show head position")]
        public TMP_Text headPositionText;
        
        [Tooltip("Text used to show head rotation")]
        public TMP_Text headRotationText;

        public Vector3 headPosition { get; private set; } = new(0, 0, 0);
        public Quaternion headRotation { get; private set; } = Quaternion.Euler(0, 0, 0);
        public Vector3 leftHandPosition { get; private set; } = new(0, 0, 0);
        public Vector3 rightHandPosition { get; private set; } = new(0, 0, 0);
        public Vector3 leftHandVelocity { get; private set; } = new(0, 0, 0);
        public Vector3 rightHandVelocity { get; private set; } = new(0, 0, 0);
        
        private InputDevice _headDevice;
        private InputDevice _leftHandDevice;
        private InputDevice _rightHandDevice;
        private InputAction _headPositionAction;
        private InputAction _headRotationAction;
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
        
        private Vector3 GetLocalPosition(Vector3 srcPos, Vector3 headPos, Quaternion headRot)
        {
            var angle = headRot.eulerAngles.y;
            var position = srcPos - headPos;
            var rotation = Quaternion.AngleAxis(-angle, Vector3.up);

            return rotation * position;
        }
        
        private void Update()
        {
            if (useReference)
            {
                var leftHandPositionSrc = _leftHandPositionAction.ReadValue<Vector3>();
                var rightHandPositionSrc = _rightHandPositionAction.ReadValue<Vector3>();
                headPosition = _headPositionAction.ReadValue<Vector3>();
                headRotation = _headRotationAction.ReadValue<Quaternion>();
                leftHandPosition = GetLocalPosition(leftHandPositionSrc, headPosition, headRotation);
                rightHandPosition = GetLocalPosition(rightHandPositionSrc, headPosition, headRotation);
                leftHandVelocity = _leftHandVelocityAction.ReadValue<Vector3>();
                rightHandVelocity = _rightHandVelocityAction.ReadValue<Vector3>();
            }
            else
            {
                if (_headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var headPos))
                    headPosition = headPos;

                if (_headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out var headRot))
                    headRotation = headRot;
                
                if (_leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var leftPos))
                    leftHandPosition = GetLocalPosition(leftPos, headPos, headRot);

                if (_rightHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out var rightPos))
                    rightHandPosition = GetLocalPosition(rightPos, headPos, headRot);
                
                if (_leftHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out var leftVel))
                    leftHandVelocity = leftVel;

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
                var head = inputActions.FindActionMap("XRI Head", true);
                _leftHandPositionAction = leftHand.FindAction("Position", true);
                _rightHandPositionAction = rightHand.FindAction("Position", true);
                _leftHandVelocityAction = leftHand.FindAction("Velocity", true);
                _rightHandVelocityAction = rightHand.FindAction("Velocity", true);
                _headPositionAction = head.FindAction("Position", true);
                _headRotationAction = head.FindAction("Rotation", true);
                
                if (!_rightHandVelocityAction.enabled)
                {
                    Debug.LogWarning("Failed to obtain device velocity! Confirm XR Device Simulator is OFF?");
                }
            }
            else
            {
                _leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
                _rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
                _headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
                
                if (!_leftHandDevice.isValid || !_rightHandDevice.isValid || !_headDevice.isValid)
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
                leftHandVelocityText == null || rightHandVelocityText == null ||
                headPositionText == null || headRotationText == null)
            {
                debugMode = false;
            }
            else
            {
                leftHandPositionText.text = "N/A";
                rightHandPositionText.text = "N/A";
                leftHandVelocityText.text = "N/A";
                rightHandVelocityText.text = "N/A";
                headPositionText.text = "N/A";
                headRotationText.text = "N/A";
            }
        }
        
        private void RelayControllerInfo()
        {
            leftHandPositionText.text = leftHandPosition.ToString();
            rightHandPositionText.text = rightHandPosition.ToString();
            leftHandVelocityText.text = leftHandVelocity.ToString();
            rightHandVelocityText.text = rightHandVelocity.ToString();
            headPositionText.text = headPosition.ToString();
            headRotationText.text = headRotation.eulerAngles.ToString();
        }
    }
}
