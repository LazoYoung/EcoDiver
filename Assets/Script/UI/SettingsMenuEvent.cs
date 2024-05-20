using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

namespace Script.UI
{
    public class SettingsMenuEvent : MonoBehaviour
    {
        [SerializeField] [Tooltip("Verbose Mode")]
        private bool verbose = false;

        private UIDocument _uiDocument;
        private Slider _soundEffectSlider;
        private Slider _brightnessSlider;
        private Button _clearButton;

        private const float initialSoundEffectValue = 100f;
        private const float initialBrightnessValue = 100f;

        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();

            // Reference the sliders from the UI
            _soundEffectSlider = _uiDocument.rootVisualElement.Q<Slider>("SoundEffectSlider");
            if (_soundEffectSlider == null)
            {
                Debug.LogWarning("SoundEffectSlider is null!");
            }
            else
            {
                _soundEffectSlider.value = initialSoundEffectValue;
                _soundEffectSlider.RegisterValueChangedCallback(OnSoundEffectSliderChanged);
            }

            _brightnessSlider = _uiDocument.rootVisualElement.Q<Slider>("BrightnessSlider");
            if (_brightnessSlider == null)
            {
                Debug.LogWarning("BrightnessSlider is null!");
            }
            else
            {
                _brightnessSlider.value = initialBrightnessValue;
                _brightnessSlider.RegisterValueChangedCallback(OnBrightnessSliderChanged);
            }

            _clearButton = _uiDocument.rootVisualElement.Q<Button>("ClearButton");
            if (_clearButton == null)
            {
                Debug.LogWarning("ClearButton is null!");
            }
            else
            {

                _clearButton.RegisterCallback<ClickEvent>(OnClearButtonClicked);
            }
        }

        private void OnSoundEffectSliderChanged(ChangeEvent<float> evt)
        {
            float volume = evt.newValue;
            if (verbose)
            {
                Debug.Log($"Sound Effect Volume set to: {volume}%");
            }
            SettingsManager.Instance.SoundVolume = volume;
        }

        private void OnBrightnessSliderChanged(ChangeEvent<float> evt)
        {
            float brightness = evt.newValue;
            if (verbose)
            {
                Debug.Log($"Brightness set to: {brightness}%");
            }
            SettingsManager.Instance.Brightness = brightness;
        }

        private void OnClearButtonClicked(ClickEvent _)
        {
            if (verbose)
            {
                Debug.Log("Clearing all settings...");
            }
            // Unregister callbacks
            _soundEffectSlider.UnregisterValueChangedCallback(OnSoundEffectSliderChanged);
            _brightnessSlider.UnregisterValueChangedCallback(OnBrightnessSliderChanged);

            _soundEffectSlider.value = initialSoundEffectValue;
            _brightnessSlider.value = initialBrightnessValue;

            SettingsManager.Instance.ClearSettings();

            _soundEffectSlider.RegisterValueChangedCallback(OnSoundEffectSliderChanged);
            _brightnessSlider.RegisterValueChangedCallback(OnBrightnessSliderChanged);

            _soundEffectSlider.MarkDirtyRepaint();
            _brightnessSlider.MarkDirtyRepaint();
        }
    }
}
