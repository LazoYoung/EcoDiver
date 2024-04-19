using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Interaction
{
    public class ToolSelector : MonoBehaviour
    {
        public InputActionReference toggleButton;
        private bool _active;

        private void Awake()
        {
            toggleButton.action.performed += OnToggle;
        }

        private void OnDestroy()
        {
            toggleButton.action.performed -= OnToggle;
        }

        private void OnToggle(InputAction.CallbackContext context)
        {
            Toggle();
            var state = _active ? "ON" : "OFF";
            Debug.Log($"Tool menu toggled {state}");
        }

        private void Toggle()
        {
            _active = !_active;
        }
    }
}
