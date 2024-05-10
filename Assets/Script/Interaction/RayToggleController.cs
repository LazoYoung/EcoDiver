using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace Script.Interaction
{
    public class RayToggleController : MonoBehaviour
    {
        [SerializeField]
        private List<XRRayInteractor> rayInteractables;

        [SerializeField] private InputActionReference toggleActionReference; // Input Action for toggling

        private void OnEnable()
        {
            toggleActionReference.action.Enable();
            toggleActionReference.action.performed += HandleToggle;
        }

        private void OnDisable()
        {
            toggleActionReference.action.Disable();
            toggleActionReference.action.performed -= HandleToggle;
        }

        private void HandleToggle(InputAction.CallbackContext context)
        {
            foreach (var interactable in rayInteractables)
            {
                if (interactable != null)
                {
                    interactable.enabled = !interactable.enabled;  // Toggle the state
                    Debug.Log($"{interactable.gameObject.name} is now {(interactable.enabled ? "enabled" : "disabled")}");
                }
            }
        }
    }
}
