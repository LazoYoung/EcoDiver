using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace Script.Interaction
{
    public class RayToggleController : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor rayInteractor;

        void Awake()
        {
            if (rayInteractor == null)
            {
                Debug.LogError("RayToggleOnHover: No XRBaseInteractor found on the GameObject.");
                return;
            }
            if (rayInteractor is XRRayInteractor ray)
            {
                ray.enabled = false;
            }

            // Subscribe to hover events
            rayInteractor.hoverEntered.AddListener(OnHoverEntered);
            rayInteractor.hoverExited.AddListener(OnHoverExited);
        }

        private void OnDestroy()
        {
            // Unsubscribe to avoid memory leaks
            if (rayInteractor != null)
            {
                rayInteractor.hoverEntered.RemoveListener(OnHoverEntered);
                rayInteractor.hoverExited.RemoveListener(OnHoverExited);
            }
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            // Enable the ray when hovering over an interactable
            if (rayInteractor is XRRayInteractor ray)
            {
                ray.enabled = true;
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            // Disable the ray when not hovering over any interactable
            if (rayInteractor is XRRayInteractor ray)
            {
                ray.enabled = false;
            }
        }
    }
}
