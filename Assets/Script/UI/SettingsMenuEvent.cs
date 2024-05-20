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
                _soundEffectSlider.RegisterValueChangedCallback(OnSoundEffectSliderChanged);
                _soundEffectSlider.value = initialSoundEffectValue;
            }

            _brightnessSlider = _uiDocument.rootVisualElement.Q<Slider>("BrightnessSlider");
            if (_brightnessSlider == null)
            {
                Debug.LogWarning("BrightnessSlider is null!");
            }
            else
            {
                _brightnessSlider.RegisterValueChangedCallback(OnBrightnessSliderChanged);
                _brightnessSlider.value = initialBrightnessValue;
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
            float volume = evt.newValue / 100f; // Convert from 0-100 to 0-1 range
            if (verbose)
            {
                Debug.Log($"Sound Effect Volume set to: {volume * 100}%");
            }
            SettingsManager.Instance.SoundVolume = volume;
        }

        private void OnBrightnessSliderChanged(ChangeEvent<float> evt)
        {
            float brightness = evt.newValue / 100f; // Convert from 0-100 to 0-1 range
            if (verbose)
            {
                Debug.Log($"Brightness set to: {brightness * 100}%");
            }
            SettingsManager.Instance.Brightness = brightness;
        }

        private void OnClearButtonClicked(ClickEvent _)
        {
            if (verbose)
            {
                Debug.Log("Clearing all settings...");
            }
            SettingsManager.Instance.ClearSettings();
            _soundEffectSlider.value = initialSoundEffectValue;
            _brightnessSlider.value = initialBrightnessValue;
        }
    }
}
