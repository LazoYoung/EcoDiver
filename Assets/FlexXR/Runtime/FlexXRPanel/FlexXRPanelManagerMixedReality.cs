#if XRI_EXISTS
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlexXR.Runtime.FlexXRPanel
{
    public partial class FlexXRPanelManager
    {
        private int     XRRayCount       => _xrRayInteractors.Count;
        private Vector3 XRRayOrigin      => _xrRayInteractors[0].rayOriginTransform.position;
        private Vector3 XRRayDirection   => _xrRayInteractors[0].rayOriginTransform.forward;
        private float   XRRayMaxDistance => _xrRayInteractors[0].maxRaycastDistance;

        private void AwakeMixedReality()
        {
            var interactable = info.worldMesh.gameObject.AddComponent<XRSimpleInteractable>();
            
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            interactable.activated.AddListener(OnActivated);
            interactable.deactivated.AddListener(OnDeactivated);
        }

        private void UpdateMixedReality()
        {
            if (ActiveInteractionMode == InteractionMode.MixedReality)
            {
                _cursor.style.display = runtimeSettings.mixedReality.cursor.show ? DisplayStyle.Flex : DisplayStyle.None;
                var newCursorSize = runtimeSettings.mixedReality.cursor.inactiveSize - (runtimeSettings.mixedReality.cursor.inactiveSize - runtimeSettings.mixedReality.cursor.activeSize)*ActiveInteractor.xrController.currentControllerState.activateInteractionState.value;
                _cursor.style.width  = newCursorSize;
                _cursor.style.height = newCursorSize;
            }
        }   
        
        #region XR Interaction
        private class XRRayInteractorStateOnHoverExit
        {
            public bool hoverToSelect;
        }
        private readonly Dictionary<XRRayInteractor,XRRayInteractorStateOnHoverExit> _xrRayInteractorStatesOnHoverExit = new();
        private readonly List<XRRayInteractor>                                 _xrRayInteractors         = new();
        
        private XRRayInteractor ActiveInteractor => _xrRayInteractors.Count > 0 ? _xrRayInteractors[0] : null;

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            switch (args.interactorObject)
            {
                case XRRayInteractor xrRayInteractor:
                    xrRayInteractor.xrController.SendHapticImpulse(runtimeSettings.mixedReality.controller.hapticAmplitude, runtimeSettings.mixedReality.controller.hapticDuration);
                    _xrRayInteractors.Add(xrRayInteractor);
                    // ! For activation to work without grabbing, exactly one `XRRayInteractor` must have `hoverToSelect` enabled.
                    var hoverToSelect = xrRayInteractor == ActiveInteractor;
                    if (xrRayInteractor.hoverToSelect != hoverToSelect)
                    {
                        _xrRayInteractorStatesOnHoverExit.Add(xrRayInteractor, new XRRayInteractorStateOnHoverExit{hoverToSelect = xrRayInteractor.hoverToSelect});
                        xrRayInteractor.hoverToSelect = hoverToSelect;
                    }
                    break;
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            switch (args.interactorObject)
            {
                case XRRayInteractor xrRayInteractor:
                    
                    xrRayInteractor.xrController.SendHapticImpulse(runtimeSettings.mixedReality.controller.hapticAmplitude, runtimeSettings.mixedReality.controller.hapticDuration);
                    
                    _xrRayInteractors.Remove(xrRayInteractor);
                    
                    if (_xrRayInteractorStatesOnHoverExit.ContainsKey(xrRayInteractor))
                    {
                        xrRayInteractor.hoverToSelect = _xrRayInteractorStatesOnHoverExit[xrRayInteractor].hoverToSelect;
                        _xrRayInteractorStatesOnHoverExit.Remove(xrRayInteractor);
                    }

                    if (ActiveInteractor is {  })
                    {
                        ActiveInteractor.xrController.SendHapticImpulse(1.5f*runtimeSettings.mixedReality.controller.hapticAmplitude, 1.5f*runtimeSettings.mixedReality.controller.hapticDuration);  // Stronger and longer here so that it's more clear which controller is still active on the panel.
                        ActiveInteractor.hoverToSelect = true;
                    }
                    break;
            }
        }

        private void OnActivated(ActivateEventArgs args)
        {
            switch (args.interactorObject)
            {
                case XRRayInteractor xrRayInteractor:
                    if (_xrRayInteractors.Count == 0) return;
                    if (xrRayInteractor != _xrRayInteractors[0]) return;
                    SendPointerDownEvent();
                    break;
            }
        }

        private void OnDeactivated(DeactivateEventArgs args)
        {
            switch (args.interactorObject)
            {
                case XRRayInteractor xrRayInteractor:
                    if (_xrRayInteractors.Count == 0) return;
                    if (xrRayInteractor != _xrRayInteractors[0]) return;
                    SendPointerUpEvent();
                    break;
            }
        }
        #endregion


    }    
}
#endif