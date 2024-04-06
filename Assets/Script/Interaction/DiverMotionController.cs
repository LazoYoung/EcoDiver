using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Interaction
{
    public class DiverMotionController : MonoBehaviour
    {
        public InputController inputController;
        
        [Tooltip("Button used to trigger swimming motion")]
        public InputActionReference triggerButton;
        
        [Tooltip("Value of the trigger button")]
        public InputActionReference triggerButtonValue;

        private bool _trigger;
        
        private void Awake()
        {
            triggerButton.action.performed += OnTrigger;
            
            if (triggerButton == null || !triggerButton.action.enabled)
            {
                Debug.LogError("Trigger button is missing or inactive!");
                enabled = false;
            }
            else if (triggerButtonValue == null || triggerButtonValue.action.enabled)
            {
                Debug.LogError("Trigger button value is missing or inactive!");
                enabled = false;
            }
        }

        private void OnDestroy()
        {
            triggerButton.action.performed -= OnTrigger;
        }

        private void FixedUpdate()
        {
            if (_trigger)
            {
                ApplyMotion();
                
                if (triggerButtonValue.action.ReadValue<float>() < 0.1f)
                {
                    _trigger = false;
                    Debug.Log("Motion detection end.");
                }
            }
        }

        private void ApplyMotion()
        {
            var leftPoint = inputController.leftHandPosition;
            var rightPoint = inputController.rightHandPosition;
            var leftVelocity = inputController.leftHandVelocity;
            var rightVelocity = inputController.rightHandVelocity;
            
            // todo method stub
            
        }

        private void OnTrigger(InputAction.CallbackContext context)
        {
            _trigger = true;
            Debug.Log("Motion detection start.");
        }
    }
}
