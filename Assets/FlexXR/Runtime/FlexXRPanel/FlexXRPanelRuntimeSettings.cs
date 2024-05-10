using System;
using UnityEngine;

namespace FlexXR.Runtime.FlexXRPanel
{
    [Serializable] public class FlexXRPanelRuntimeSettings
    {
        [Tooltip("Whether the panel is in screen or world space and if it's interacted with using the mouse or the XR controller.")]
        public InteractionMode interactionMode = InteractionMode.None;
        
        [Tooltip("Scale the UIElements panel.")]
        [Range(0.1f, 10f)] public float panelScale = 1.0f;

        [Tooltip("World space panel mesh settings used in world and mixed reality interaction modes.")]
        [SerializeField] public FlexXRPanelWorldMeshSettings    worldMesh    = new();
        [Tooltip("Mixed reality settings that are only relevant in mixed reality interaction mode.")]
        [SerializeField] public FlexXRPanelMixedRealitySettings mixedReality = new();
        [Tooltip("Advanced settings that ideally should only be relevant to developers of FlexXR or advanced users.")]
        [SerializeField] public FlexXRPanelAdvancedSettings     advanced     = new ();
    }

    [Serializable]
    public class FlexXRPanelWorldMeshSettings
    {
        [Tooltip("The unit conversion factor from screen space to world space distance. Increase to scale up the world panel.")]
        [Range(0.0001f, 0.01f)] public float screenToWorldScale = 0.001f;
        
        [Tooltip("Whether or not the world mesh is automatically cropped to fit the GUI content. If not, then the full RenderTexture size is used (at a factor of screenToWorldScale).")]
        public FittingMode fittingMode = FittingMode.CropToFlexXRContent;
        
        [Tooltip("The shape of the world mesh.")]
        public MeshShape shape             = MeshShape.Flat;
        
        [Tooltip("The number of mesh segments used to approximate the world mesh when using cylindrical shape. Increase for a smoother surface.")]
        public int       curvatureSegments = 12; 
        
        [Tooltip("The radius of the world mesh when using cylindrical shape.")]
        public float     curvatureRadius   = 1;
    }

    [Serializable]
    public class FlexXRPanelMixedRealitySettings
    {
        [Tooltip("Mixed reality controller settings, e.g. haptic (vibration) feedback.")]
        [SerializeField] public FlexXRPanelMixedRealityControllerSettings controller = new();
        [Tooltip("Mixed reality GUI panel cursor settings.")]
        [SerializeField] public FlexXRPanelMixedRealityCursorSettings     cursor     = new();
    }
    
    [Serializable] public class FlexXRPanelMixedRealityCursorSettings
    {
        [Tooltip("Whether or not the a cursor is shown on the panel where the active XR controller is pointing.")]
        public bool show = true;
        
        [Tooltip("Texture for the cursor element.")]
        public Texture2D texture;
        
        [Tooltip("The size of the cursor when the XR pointer is not activating an element.")]
        [Range(1, 100)] public float inactiveSize = 20;
        
        [Tooltip("The size of the cursor when the XR pointer is activating an element.")]
        [Range(1, 100)] public float activeSize = 15;
    }
    
    [Serializable] public class FlexXRPanelMixedRealityControllerSettings
    {
        [Tooltip("The strength of haptic feedback when an XR controller enters a panel.")]
        [Range(0f, 1f)] public float hapticAmplitude = 0.2f;
        
        [Tooltip("The duration of haptic feedback when an XR controller enters a panel.")]
        [Range(0f, 1f)] public float hapticDuration  = 0.1f;
    }
    
    [Serializable] public class FlexXRPanelAdvancedSettings
    {
        [Tooltip("The distance in pixel space that a XR pointer must move on a panel to send a move event to `UIElements`.")]
        [Range(0.01f, 1f)]  public float pointerSensitivity = 0.1f;
    }
}