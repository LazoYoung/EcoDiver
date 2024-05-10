using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using FlexXR.Runtime.FlexXRPanel;
using UnityEngine.InputSystem.XR;

#if XRI_EXISTS
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
#endif

#if XRM_EXISTS
using UnityEngine.XR.Management;
#endif

namespace FlexXR.Runtime.Demos
{
    public class FlexXRAdvancedDemo0Settings
    {
        public InteractionMode demoMode = InteractionMode.Screen;
    }
    
    [DisallowMultipleComponent]
    public class FlexXRAdvancedDemo0Main : MonoBehaviour
    {
        [Tooltip("An extra camera offset is required for some XR corner cases, e.g. when using Quest Link.")]
        public Vector3 extraXRCameraOffset = new Vector3(0, -1.1176f, 0);
        
        [Tooltip("For the particular case of `UIElements.DropdownField`, the style has to be modified in a special way here.")]
        public StyleSheet[] rootStyleSheets;

        private readonly FlexXRAdvancedDemo0Settings _settings = new ();

        private TrackedPoseDriver _trackedPoseDriver;

        private Transform             _xrRigTransform;
        private Transform             _cameraOffsetTransform;

        private FlexXRPanelManager[] _flexXRPanelManagers;
        
#if XRM_EXISTS
        private static XRManagerSettings XRManager => XRGeneralSettings.Instance is null ? null : XRGeneralSettings.Instance.Manager;
#endif
        
#if XRI_EXISTS
        private ActionBasedController _leftXRController;
        private ActionBasedController _rightXRController;
        private XRDeviceSimulator     _xrDeviceSimulator;
        
        private bool _simulatingXRDevice;
        private bool _xrInteractionEnabled;
        private bool XRInteractionEnabled
        {
            set
            {
                _leftXRController.gameObject.SetActive(_xrInteractionEnabled);
                _rightXRController.gameObject.SetActive(_xrInteractionEnabled);
                
                if (value == _xrInteractionEnabled) return;
                _xrInteractionEnabled = value;

                if (_xrInteractionEnabled)
                {
#if XRM_EXISTS
                    if (XRManager is { activeLoader: null })
                    {
                        _xrDeviceSimulator.gameObject.SetActive(false);
                        XRManager.InitializeLoaderSync();
                        if (XRManager.activeLoader is { })
                        {
                            XRManager.StartSubsystems();
                        } // ! Otherwise assuming there is no XR loader and will use simulation instead.
                    }
                    
                    // ReSharper disable once MergeSequentialChecks  // Don't combine because it can conflict with Unity's object lifecycle management.
                    if (XRManager is null || XRManager.activeLoader is null)
                    {   // ! Assuming there was no loader so we simulate instead.
                        _xrDeviceSimulator.gameObject.SetActive(true);
                        _simulatingXRDevice = true;
                    }
#endif
                    
                    if (!_simulatingXRDevice) _cameraOffsetTransform.localPosition += extraXRCameraOffset;
                }
                else
                {
                    if (_simulatingXRDevice)
                    {
                        _xrDeviceSimulator.gameObject.SetActive(false);
                    }
                    else
                    {
                        _cameraOffsetTransform.localPosition -= extraXRCameraOffset;
                    }

#if XRM_EXISTS
                    if (XRManager is { activeLoader: { } })
                    {
                        XRManager.StopSubsystems();
                        XRManager.DeinitializeLoader();
                    }
#endif
                }
            }
        }
#endif
        
        
        private void FindGameObjectsAndComponents()
        {
            _flexXRPanelManagers = transform.GetComponentsInChildren<FlexXRPanelManager>();
            if (_flexXRPanelManagers.Length == 0)
            {
                throw new Exception("Demo setup failed. Could not find any FlexXR Panel Manager.");
            }
            
            var allGameObjectsInLoadedResources = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var go in allGameObjectsInLoadedResources)
            {
                if (!go.name.Equals("XR Rig")) continue;
                if (go.scene.name is null) continue;
                _xrRigTransform = go.transform;
                _xrRigTransform.gameObject.SetActive(true);
                break;
            }
            
#if XRI_EXISTS
            if (_xrRigTransform is { })
            {
                _cameraOffsetTransform = _xrRigTransform.Find("Camera Offset");
                
                _leftXRController  = _cameraOffsetTransform.Find("LeftHand Controller")?.GetComponent<ActionBasedController>();
                
                _rightXRController = _cameraOffsetTransform.Find("RightHand Controller")?.GetComponent<ActionBasedController>();
            }
            
            _xrDeviceSimulator = Resources.FindObjectsOfTypeAll<XRDeviceSimulator>().FirstOrDefault();
#else
            if (_xrRigTransform is { })
            {
                Debug.LogWarning("XR Interaction Toolkit is not installed, so the XR Rig from the demo scene will be destroyed. To avoid this warning and other warnings about related missing components, either install the XR Interaction Toolkit package or use the other demo scene without XRI game objects.");
                var mainCameraTransform = _xrRigTransform.GetComponentInChildren<Camera>().transform;
                mainCameraTransform.SetParent(null, worldPositionStays: true);
                Destroy(_xrRigTransform.gameObject);
                _xrRigTransform = null;
            }
#endif
        }

        private FlyCamera _flyCamera;
        private Camera    _cameraWithoutXRRig;

        private InteractionMode   _demoMode;

        private void Awake()
        {
            FindGameObjectsAndComponents();
        }

        private void Start()
        {
            if (Camera.main is null) throw new Exception("Demo setup failed. Could not find main camera.");
            
            _flyCamera                 = Camera.main.GetComponent<FlyCamera>();
            _flyCamera.enabled         = false;
            
            _trackedPoseDriver         = Camera.main.GetComponent<TrackedPoseDriver>();
            if (_trackedPoseDriver is { })
            {
                _trackedPoseDriver.enabled = false;
            }
            
#if XRM_EXISTS
            if (XRManager is { activeLoader: { } }) XRManager.DeinitializeLoader();
#endif
            
#if XRM_EXISTS && XRI_EXISTS
            FlexXRDemo0UIDocumentBindings.Bind(_flexXRPanelManagers, _settings, _xrRigTransform is { });
#else
            FlexXRDemo0UIDocumentBindings.Bind(_flexXRPanelManagers, _settings, false);
#endif
            
            AddRootStyleSheets();

            foreach (var panelManager in _flexXRPanelManagers)
            {
                panelManager.runtimeSettings.interactionMode = InteractionMode.Screen;
            }
        }

        private void Update()
        {
#if XRI_EXISTS
            if (_xrRigTransform is { })
            {
                XRInteractionEnabled = _flexXRPanelManagers.Any(panelManager => panelManager.runtimeSettings.interactionMode == InteractionMode.MixedReality);
            }
            
            if (_trackedPoseDriver is { })
            {
                _trackedPoseDriver.enabled = _settings.demoMode == InteractionMode.MixedReality && !AnyTextFieldIsFocused;
            }
            
#endif
            
            _flyCamera.enabled         = _settings.demoMode == InteractionMode.World        && !AnyTextFieldIsFocused;
        }

        private bool AnyTextFieldIsFocused => _flexXRPanelManagers.Any(panelManager => panelManager.TextFieldIsFocused);

        /// <summary>
        /// This Finds one UI Documents from the scene and adds the rootStyleSheet to the parent, i.e. the real root.
        /// </summary>
        private void AddRootStyleSheets()
        {
            foreach (var styleSheet in rootStyleSheets)
            {
                foreach (var panelManager in _flexXRPanelManagers)
                {
                    panelManager.document.rootVisualElement?.parent?.styleSheets.Add(styleSheet);                    
                }
            }
        }
    }
}
