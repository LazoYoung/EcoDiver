using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UIElements;
using TouchPhase = UnityEngine.TouchPhase;

namespace FlexXR.Runtime.FlexXRPanel
{
    public enum InteractionMode
    {
        None,
        Screen,
        World,
        MixedReality
    }

    [Serializable]
    public class FlexXRPanelTemplates
    {
        [Tooltip("The PanelSettings that will be copied and modified at runtime. If None, the initial PanelSettings from the UIDocument will be used.")]
        public PanelSettings panelSettings;

        [Tooltip("The material for the mesh where the GUI will be rendered in world space.")]
        public Material worldMaterial;
        
        [Tooltip("The texture where the world GUI will be rendered. This will be assigned to the world mesh material.")]
        public RenderTexture worldRenderTexture;
    }
    
    [Serializable] internal class FlexXRPanelInfo
    {
        [Tooltip("The camera being used. Usually the main camera.")]
        [SerializeField] internal Camera camera;

        [Tooltip("The mesh where the GUI is rendered and interacted with in world space.")]
        [SerializeField] internal MeshFilter worldMesh;

        [Tooltip("Whether not a raycast (from mouse or XR pointer) is hitting the world space panel.")]
        [SerializeField] internal bool    raycastIsHitting;

        [Tooltip("Whether not a pointer (mouse or XR interactor) is activating the world space panel.")]
        [SerializeField] internal bool    activating;
        
        [Tooltip("Pixel position of the active pointer on the panel.")]
        [SerializeField] internal Vector2 flexPosition;
        
        [Tooltip("Name of the element hovered by the active pointer on the panel.")]
        [SerializeField] internal string  hoveredElementName;
        
        [Tooltip("Type of the element hovered by the active pointer on the panel.")]
        [SerializeField] internal string  hoveredElementType;
        
        [Tooltip("Name of the element focused on the panel.")]
        [SerializeField] internal string  focusedElementName;
        
        [Tooltip("Type of the element focused on the panel.")]
        [SerializeField] internal string  focusedElementType;
    }

    public partial class FlexXRPanelManager : MonoBehaviour
    {
        [Tooltip("The UI Document defining the GUI content for the panel. If None, then this game object's UI Document will be used if available.")]
        public UIDocument    document;
        
        [Tooltip("These will be cloned at runtime.")]
        [SerializeField] public  FlexXRPanelTemplates       templates       = new();
        
        [Tooltip("Runtime settings that will immediately affect the panel.")]
        [SerializeField] public  FlexXRPanelRuntimeSettings runtimeSettings = new();
        
        [Tooltip("Read-only information about the panel.")]
        [SerializeField] private FlexXRPanelInfo            info            = new();

        private MeshRenderer  _worldMeshRenderer;
        private MeshCollider  _worldMeshCollider;
        private Material      _worldMaterial;
        private RenderTexture _worldRenderTexture;

        public FlexXRPanelElements flexXRPanelElements;
        
        private VisualElement _cursor;
        
        #region PanelSettings
        private void ModifyPanelSettingsFor(InteractionMode interactionMode)
        {
            switch (interactionMode)
            {
                case InteractionMode.None:
                    break;
                case InteractionMode.Screen:
                    document.panelSettings.targetTexture     = null;
                    document.panelSettings.clearColor        = false;
                    document.panelSettings.clearDepthStencil = false;
                    break;
                case InteractionMode.World:
                case InteractionMode.MixedReality:
                    document.panelSettings.targetTexture     = _worldRenderTexture;
                    document.panelSettings.clearColor        = true;
                    document.panelSettings.clearDepthStencil = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactionMode), interactionMode, null);
            }
        }

        private                 PanelRaycaster           _builtinPanelRaycaster;
        private bool BuiltInPanelRaycastsDisabled
        {
            get => _builtinPanelRaycaster is not null && !_builtinPanelRaycaster.enabled;
            set
            {
                if (value == BuiltInPanelRaycastsDisabled) return;
                if (_builtinPanelRaycaster is null)
                {
                    var builtinPanelRaycasters = info.camera.GetComponentsInChildren<PanelRaycaster>();
                    foreach (var raycaster in builtinPanelRaycasters)
                    {
                        if (!raycaster.name.Equals(document.panelSettings.name)) continue;
                        _builtinPanelRaycaster = raycaster;
                        break;
                    }
                }
                if (_builtinPanelRaycaster is null) return;
                _builtinPanelRaycaster.enabled = !value;
            }
        }

        #endregion

        
        private Transform _worldContentTransform;
        private bool WorldContentActive
        {
            get => _worldContentTransform.gameObject.activeSelf;
            set => _worldContentTransform.gameObject.SetActive(value);
        }
        
        private FlexXRPanelMesh  _flexXRPanelMesh;

        private InteractionMode _activeInteractionMode;
        private InteractionMode ActiveInteractionMode
        {
            get => _activeInteractionMode;
            set
            {
                if (value == _activeInteractionMode) return;
                
                _activeInteractionMode = value;
                
                ModifyPanelSettingsFor(value);
            }
        }
        
        private static readonly List<FlexXRPanelManager> GlobalFlexXRPanelManagers = new();
        
        public bool TextFieldIsFocused => _focusedElement is TextField;
        

        #region MonoBehaviour
        private void Awake()
        {
            GlobalFlexXRPanelManagers.Add(this);

            _worldContentTransform  =  transform.Find("World Content");
            if (_worldContentTransform is null)
            {
                throw new Exception("FlexXR Panel Manager failed to find its world content container game object. Disabling.");
            }
            
            info.worldMesh = _worldContentTransform.Find("Flex Mesh")?.GetComponent<MeshFilter>();
            if (info.worldMesh is null)
            {
                throw new Exception("FlexXRPanelManager failed to find its Flex Mesh game object's Mesh Filter. Disabling.");
            }

            _worldMeshRenderer = info.worldMesh.GetComponent<MeshRenderer>();
            if (_worldMeshRenderer is null)
            {
                throw new Exception("FlexXRPanelManager failed to find its Flex Mesh game object's Mesh Renderer. Disabling.");
            }

            _worldMeshCollider = _worldMeshRenderer.GetComponent<MeshCollider>();
            if (_worldMeshCollider is null)
            {
                throw new Exception("FlexXRPanelManager failed to find its Flex Mesh game object's Mesh Collider. Disabling.");
            }
            
            _cursor = new VisualElement
            {
                name = "FlexXR Cursor",
                pickingMode = PickingMode.Ignore,
                style =
                {
                    position        = Position.Absolute,
                    width           = runtimeSettings.mixedReality.cursor.inactiveSize,
                    height          = runtimeSettings.mixedReality.cursor.inactiveSize,
                    backgroundImage = runtimeSettings.mixedReality.cursor.texture
                }
            };

            document ??= GetComponent<UIDocument>();
            if (document is null)
            {
                throw new Exception("FlexXRPanelManager requires a UIDocument to be set.");
            }

            if (document.visualTreeAsset is null)
            {
                throw new Exception("FlexXRPanelManager requires the UIDocument to have a source asset.");
            }

            if (templates.panelSettings is null)
            {
                if (document.panelSettings is null)
                {
                    throw new Exception("FlexXRPanelManager requires either panelSettingsTemplate or its game object's UI Document's panelSettings to be set.");
                }
                
                templates.panelSettings = document.panelSettings;
            }
            
            _worldRenderTexture      = Instantiate(templates.worldRenderTexture);
            _worldRenderTexture.name = $"World RenderTexture {GlobalFlexXRPanelManagers.Count - 1}";
            
            _worldMaterial              = Instantiate(templates.worldMaterial);
            _worldMaterial.name         = $"World Material {GlobalFlexXRPanelManagers.Count - 1}";
            _worldMaterial.mainTexture  = _worldRenderTexture;
            _worldMeshRenderer.material = _worldMaterial;
            
            /* Have to rename the panel settings template before cloning because UI Toolkit
            creates a Panel Settings game object under the camera controlling events and raycasts 
            based on this name and we need them to be unique. */
            var panelSettingsTemplateName = templates.panelSettings.name;
            templates.panelSettings.name         = $"Panel Settings {GlobalFlexXRPanelManagers.Count - 1}";
            document.panelSettings               = Instantiate(templates.panelSettings);
            templates.panelSettings.name         = panelSettingsTemplateName;
            document.panelSettings.targetTexture = _worldRenderTexture;
            if (document.panelSettings.scaleMode != PanelScaleMode.ConstantPixelSize)
            {
                Debug.LogWarning("FlexXR only supports PanelScaleMode.ConstantPixelSize in PanelSettings. Modifying the runtime copy of the PanelSettings to use this mode.");
                document.panelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
            }

            flexXRPanelElements = new FlexXRPanelElements(document);
            flexXRPanelElements.RootVisualElement.Add(_cursor);
            _cursor.style.display = DisplayStyle.None;

#if XRI_EXISTS
            AwakeMixedReality();
#endif
        }

        private void Start()
        {
            info.camera = Camera.main;
            
            if (info.camera is null)
            {
                throw new Exception("FlexXRPanelManager failed to find the main camera at start.");
            }
            
            var inputSystemUIInputModule = FindObjectOfType<InputSystemUIInputModule>();
            if (inputSystemUIInputModule is null) info.camera.gameObject.AddComponent<InputSystemUIInputModule>();
            
            _worldMeshRenderer.transform.localScale = Vector3.one;  // ! This was set to { 1.92, 1.08, 1} so that the initial state in the Editor looks right given a 1920x1080 PanelSettings resolution.
            var flexMeshFilter = _worldMeshRenderer.transform.GetComponent<MeshFilter>();
            _flexXRPanelMesh = new FlexXRPanelMesh(flexXRPanelElements, flexMeshFilter, 1);

            FlexXRPanelUIElementWorkarounds.Init(this);
            
            RegisterPointerCaptureCallbacks();
        }
        
        private void Update()
        {
            if (!document.enabled) return;

            ActiveInteractionMode = runtimeSettings.interactionMode;

#if UNITY_EDITOR
            _focusedElement         = (VisualElement) document.rootVisualElement.panel.focusController.focusedElement;
            info.focusedElementName = _focusedElement is null ? "null" : _focusedElement.name;
            info.focusedElementType = _focusedElement is null ? "null" : _focusedElement.GetType().ToString();
#endif
            BuiltInPanelRaycastsDisabled = ActiveInteractionMode != InteractionMode.Screen;
            
            document.panelSettings.scale = runtimeSettings.panelScale;
            
            _cursor.style.display     = DisplayStyle.None;
            
            WorldContentActive = ActiveInteractionMode != InteractionMode.Screen;
            if (!WorldContentActive) return;

            UpdateMeshFitting();

            _flexXRPanelMesh.UpdateVertexDependenciesIfStale();
            
#if XRI_EXISTS
            if (ActiveInteractionMode == InteractionMode.MixedReality && XRRayCount == 0)
            {
                PointerRaycastIsHitting = false;
                return;
            }
#endif

            UpdateRaycast();
            if (!PointerRaycastIsHitting)
            {
                FlexPosition = InvalidPixelPosition;
                return;
            }
            
#if XRI_EXISTS
            UpdateMixedReality();
#endif
            
            var newFlexPosition = new Vector2(
                document.panelSettings.targetTexture.width  / document.panelSettings.scale * _raycastHit.textureCoord.x,
                document.panelSettings.targetTexture.height / document.panelSettings.scale * (1 - _raycastHit.textureCoord.y)); 
            
            if ((newFlexPosition - _lastFlexPosition).magnitude > runtimeSettings.advanced.pointerSensitivity)
            {
                FlexPosition               = newFlexPosition;
                
                _cursor.transform.position = new Vector3(
                    FlexPosition.x - _cursor.localBound.width/2,
                    FlexPosition.y - _cursor.localBound.height/2,
                    _cursor.transform.position.z);
                
                SendPointerMoveEvent();
            }
            
#if UNITY_EDITOR
            _hoveredElement         = document.rootVisualElement.panel.Pick(FlexPosition);
            info.hoveredElementName = _hoveredElement is null ? "null" : _hoveredElement.name;
            info.hoveredElementType = _hoveredElement is null ? "null" : _hoveredElement.GetType().ToString();
#endif
            
            if (ActiveInteractionMode == InteractionMode.World && PointerRaycastIsHitting)
            {   // ! Only forwarding left-click so the functionality is like a simple XR pointer without many controller buttons.
                if (Mouse.current.leftButton.wasPressedThisFrame)  SendPointerDownEvent();
                if (Mouse.current.leftButton.wasReleasedThisFrame) SendPointerUpEvent();
            }
        }

        private void FitMeshBasedOnFittingMode(bool forceRefit = false)
        {
            switch (runtimeSettings.worldMesh.fittingMode)
            {
                case FittingMode.None:
                    break;
                case FittingMode.FillRenderTexture:
                    _flexXRPanelMesh.ResetMeshToFillRenderTexture(forceRefit);
                    break;
                case FittingMode.CropToFlexXRContent:
                    _flexXRPanelMesh.CropMeshToGUIBounds(forceRefit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateMeshFitting()
        {
            if (runtimeSettings.worldMesh.fittingMode == FittingMode.CropToFlexXRContent && flexXRPanelElements.FlexXRContentElement == flexXRPanelElements.RootVisualElement)
            {
                Debug.LogWarning("FlexXRPanel cropping to FlexXRContent requires usage of the FlexXRContent UIElement in the UI Document. Disabling cropping.");
                runtimeSettings.worldMesh.fittingMode = FittingMode.FillRenderTexture;
            }
            
            FitMeshBasedOnFittingMode();

            WorldContentActive = runtimeSettings.worldMesh.fittingMode != FittingMode.CropToFlexXRContent || _flexXRPanelMesh.ElementBounds.Width > 3 && _flexXRPanelMesh.ElementBounds.Height > 3;
            
            if (!WorldContentActive) return;
            
            if (Math.Abs(runtimeSettings.worldMesh.screenToWorldScale - _flexXRPanelMesh.ScreenToWorldScale) > float.Epsilon)
            {
                _flexXRPanelMesh.ScreenToWorldScale = runtimeSettings.worldMesh.screenToWorldScale;
                FitMeshBasedOnFittingMode(true);
            }
            
            if (runtimeSettings.worldMesh.shape != _flexXRPanelMesh.Shape)
            {
                _flexXRPanelMesh.Shape = runtimeSettings.worldMesh.shape;
                FitMeshBasedOnFittingMode(true);
            }

            if (runtimeSettings.worldMesh.shape == MeshShape.Cylindrical)
            {
                if (Math.Abs(runtimeSettings.worldMesh.curvatureRadius - _flexXRPanelMesh.CurvatureRadius) > float.Epsilon)
                {
                    _flexXRPanelMesh.CurvatureRadius = runtimeSettings.worldMesh.curvatureRadius;
                    FitMeshBasedOnFittingMode(true);
                }

                if (runtimeSettings.worldMesh.curvatureSegments != _flexXRPanelMesh.HorizontalSegments)
                {
                    _flexXRPanelMesh.HorizontalSegments = runtimeSettings.worldMesh.curvatureSegments;
                    FitMeshBasedOnFittingMode(true);
                }
            }
        }
        
        private void OnDisable()
        {
            _builtinPanelRaycaster = null;  // Otherwise we keep trying to access the PanelRaycaster that was destroyed by UI Toolkit.
        }
        #endregion
        

        #region Raycasting
        private Vector3    _rayOrigin;
        private Vector3    _rayDirection;
        private float      _rayMaxDistance;
        private RaycastHit _raycastHit;

        private bool PointerRaycastIsHitting
        {
            get => info.raycastIsHitting;
            set
            {
                if (value == false && PointerRaycastIsHitting) OnPointerLeavingPanel();
                info.raycastIsHitting = value;
            }
        }

        private void UpdateRaycast()
        {
            Ray ray;
            switch (ActiveInteractionMode)
            {
                case InteractionMode.None:
                case InteractionMode.Screen:
                    PointerRaycastIsHitting = false;
                    return;
                case InteractionMode.World:
                    ray             = info.camera.ScreenPointToRay(Mouse.current.position.ReadValue());
                    _rayMaxDistance = 100;
                    break;
                case InteractionMode.MixedReality:
#if XRI_EXISTS
                    _rayOrigin      = XRRayOrigin;
                    _rayDirection   = XRRayDirection;
                    ray             = new Ray(_rayOrigin, _rayDirection);
                    _rayMaxDistance = XRRayMaxDistance;
                    break;
#else
                    enabled = false;
                    throw new Exception("FlexXRPanel MixedReality interaction requires Unity's XR Interaction Toolkit package to be installed. Disabled.");
#endif
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var hitAPanel = Physics.Raycast(
                ray,
                out _raycastHit,
                maxDistance: _rayMaxDistance);

            PointerRaycastIsHitting = hitAPanel && _raycastHit.collider == _worldMeshCollider;
        }
        
        #endregion



        #region UI Toolkit (UIElements)
        private        VisualElement _hoveredElement;
        private        VisualElement _focusedElement;
        private static Vector2       InvalidPixelPosition => Vector2.negativeInfinity;
        private Vector2 FlexPosition
        {
            get => info.flexPosition;
            set
            {
                _lastFlexPosition = value == InvalidPixelPosition ? InvalidPixelPosition : info.flexPosition;
                
                info.flexPosition = value;
            }
        }
        private Vector2 _lastFlexPosition;
        
        private static Touch Touch(TouchPhase phase, Vector2 pixelUV, Vector2 pixelUVLastFrame)
        {
            return new Touch 
            {
                type          = TouchType.Indirect,
                phase         = phase,
                tapCount      = 1,
                rawPosition   = pixelUV,
                position      = pixelUV,
                deltaPosition = pixelUVLastFrame == InvalidPixelPosition ? Vector2.zero : pixelUV - pixelUVLastFrame
            };
        }
        
        private PointerDownEvent WorldPanelPointerDownEvent()
        {
            return PointerDownEvent.GetPooled(Touch(TouchPhase.Began, FlexPosition, _lastFlexPosition));
        }
        private PointerMoveEvent WorldPanelPointerMoveEvent()
        {
            return PointerMoveEvent.GetPooled(Touch(TouchPhase.Moved, FlexPosition, _lastFlexPosition));
        }
        private PointerUpEvent WorldPanelPointerUpEvent()
        {
            return PointerUpEvent.GetPooled(Touch(TouchPhase.Ended, FlexPosition, _lastFlexPosition));
        }
        
        private void SendPointerDownEvent()
        {
            info.activating = true;
            document.rootVisualElement.SendEvent(WorldPanelPointerDownEvent());
        }

        private void SendPointerUpEvent()
        {
            info.activating = false;
            document.rootVisualElement.SendEvent(WorldPanelPointerUpEvent());
        }

        private void SendPointerMoveEvent() => document.rootVisualElement.SendEvent(WorldPanelPointerMoveEvent());


        private int _capturedPointerId;
        private void RegisterPointerCaptureCallbacks()
        {
            document.rootVisualElement.RegisterCallback<PointerCaptureEvent>(evt =>
            {
                _capturedPointerId = evt.pointerId;
            });
        }
        
        private void OnPointerLeavingPanel()
        {
            var capturingElement = document.rootVisualElement.panel.GetCapturingElement(_capturedPointerId);
            capturingElement?.ReleasePointer(_capturedPointerId);

            SendPointerUpEvent();
            FlexPosition = Vector2.zero;
            SendPointerMoveEvent();
            
            document.rootVisualElement.Focus();  // * Reset focused element so the one previously capturing the pointer isn't still highlighted.
        }
        #endregion
    }
}

