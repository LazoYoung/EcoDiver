using UnityEngine.UIElements;

namespace FlexXR.Runtime.FlexXRPanel
{
    public static class FlexXRPanelUIElementWorkarounds
    {
        private static bool _isInitialized;
        internal static void Init(FlexXRPanelManager panelManager)
        {
            if (_isInitialized) return;
            
            RegisterSliderWorkarounds(panelManager);
        }

        private static float InvalidFloatValue => float.NegativeInfinity;
        private static bool IsValid(float value)
        {
            return !value.Equals(InvalidFloatValue);
        }
        
        private class SliderWorkaroundInfo
        {
            internal float revertValue = InvalidFloatValue;
        }
        private static void RegisterSliderWorkarounds(FlexXRPanelManager panelManager)
        {
            foreach (var slider in panelManager.document.rootVisualElement.Query<Slider>().ToList())
            {
                RegisterWorkarounds(slider, panelManager);
            }
        }
        private static void RegisterWorkarounds(Slider slider, FlexXRPanelManager panelManager)
        {
            var workaroundInfo = new SliderWorkaroundInfo();

            slider.RegisterCallback<PointerDownEvent>(_ =>
            {
                workaroundInfo.revertValue = slider.value;  // * This is here instead of at PointerCaptureEvent because the value is already modified on capture.
            });
            slider.RegisterCallback<PointerCaptureOutEvent>(_ =>
            {
                workaroundInfo.revertValue = slider.value;
            });
            panelManager.document.rootVisualElement.RegisterCallback<PointerUpEvent>(_ =>
            {
                if (!IsValid(workaroundInfo.revertValue)) return;
                slider.value = workaroundInfo.revertValue;
            });
        }
    }
}