using System;
using System.Collections.Generic;
using FlexXR.Runtime.FlexXRPanel;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlexXR.Runtime.Demos
{
    public static class FlexXRDemo0UIDocumentBindings
    {
        public static void Bind(FlexXRPanelManager[] flexXRPanelManagers, FlexXRAdvancedDemo0Settings demoSettings, bool showMixedRealityMode)
        {
            var panel0Content = flexXRPanelManagers[0].flexXRPanelElements.FlexXRContentElement;
            
            {// Demo mode dropdown
                var demoModeDropdownField    = panel0Content.Q<DropdownField>("DemoModeDropdownField");
                
                demoModeDropdownField.choices = new List<string> {"Screen", "World"};
                if (showMixedRealityMode) demoModeDropdownField.choices.Add("MixedReality");
                
                var screenDemoModeInfo       = panel0Content.Q<VisualElement>("ScreenDemoModeInfo");
                var worldDemoModeInfo        = panel0Content.Q<VisualElement>("WorldDemoModeInfo");
                var mixedRealityDemoModeInfo = panel0Content.Q<VisualElement>("MixedRealityDemoModeInfo");
                demoModeDropdownField.RegisterValueChangedCallback(evt =>
                {
                    var newMode = (InteractionMode)Enum.Parse(typeof(InteractionMode), evt.newValue);
                    demoSettings.demoMode = newMode;
                    screenDemoModeInfo.style.display       = newMode == InteractionMode.Screen       ? DisplayStyle.Flex : DisplayStyle.None;
                    worldDemoModeInfo.style.display        = newMode == InteractionMode.World        ? DisplayStyle.Flex : DisplayStyle.None;
                    mixedRealityDemoModeInfo.style.display = newMode == InteractionMode.MixedReality ? DisplayStyle.Flex : DisplayStyle.None;
                    foreach (var panelManager in flexXRPanelManagers)
                    {
                        switch (newMode)
                        {
                            case InteractionMode.None:
                                break;
                            case InteractionMode.Screen:
                            case InteractionMode.World:
                                if (panelManager.runtimeSettings.interactionMode == InteractionMode.MixedReality) panelManager.transform.position -= 0.2f*Vector3.forward;
                                panelManager.runtimeSettings.interactionMode = newMode;
                                panelManager.runtimeSettings.worldMesh.shape = MeshShape.Flat;
                                break;
                            case InteractionMode.MixedReality:
#if XRM_EXISTS && XRI_EXISTS
                                if (panelManager.runtimeSettings.interactionMode != InteractionMode.MixedReality) panelManager.transform.position += 0.2f*Vector3.forward;
                                panelManager.runtimeSettings.interactionMode = newMode;
                                panelManager.runtimeSettings.worldMesh.shape = MeshShape.Cylindrical;
#else
                                Debug.LogError("Switching to this FlexXR demo's mixed reality mode requires installing Unity's XR Plugin Management and XR Interaction Toolkit packages. Setting World mode instead.");
                                demoModeDropdownField.value = "World";
#endif
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                });
            }
            
            {// Panel scale slider
                var panelScaleSlider = panel0Content.Q<Slider>("PanelScaleSlider");
                flexXRPanelManagers[0].document.rootVisualElement.RegisterCallback<PointerCaptureOutEvent>(_ =>
                {
                    foreach (var panelManager in flexXRPanelManagers)
                    {
                        panelManager.runtimeSettings.panelScale = panelScaleSlider.value;
                    }
                });
                panelScaleSlider.schedule.Execute(_ =>
                {
                    if (flexXRPanelManagers[0].document.rootVisualElement.focusController.focusedElement == panelScaleSlider) return;
                    foreach (var panelManager in flexXRPanelManagers)
                    {
                        panelScaleSlider.value = panelManager.runtimeSettings.panelScale;
                    }
                }).Every(100);
            }
        }
    }
}
