using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;

namespace Script.UI
{
    public class SettingsMenuEvent : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private Slider _soundEffectSlider;
        private Slider _brightnessSlider;

        // private PostProcessProfile _postProcessProfile;
        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();

            // Reference the sliders from the UI
            _soundEffectSlider = _uiDocument.rootVisualElement.Q<Slider>("SoundEffectSlider");
            if (_soundEffectSlider == null)
            {
                Debug.LogError("SoundEffectSlider is null!");
            }
            else
            {
                _soundEffectSlider.RegisterValueChangedCallback(OnSoundEffectSliderChanged);
            }

            _brightnessSlider = _uiDocument.rootVisualElement.Q<Slider>("BrightnessSlider");
            if (_brightnessSlider == null)
            {
                Debug.LogError("BrightnessSlider is null!");
            }
            else
            {
                _brightnessSlider.RegisterValueChangedCallback(OnBrightnessSliderChanged);
            }
        }

        private void OnSoundEffectSliderChanged(ChangeEvent<float> evt)
        {
            float volume = evt.newValue / 100f; // Convert from 0-100 to 0-1 range
            Debug.Log($"Sound Effect Volume set to: {volume * 100}%");
            // SetSoundEffectVolume(volume);
        }

        private void OnBrightnessSliderChanged(ChangeEvent<float> evt)
        {
            float brightness = evt.newValue / 100f; // Convert from 0-100 to 0-1 range
            Debug.Log($"Brightness set to: {brightness * 100}%");
            // SetBrightness(brightness);
        }

        // private void SetSoundEffectVolume(float volume)
        // {
        //     // Example: Assuming you have an AudioMixer to control the volume
        //     // AudioManager.Instance.SetSoundEffectsVolume(volume);
        //
        //     // For simplicity, let's assume you are directly setting AudioListener volume
        //     AudioListener.volume = volume;
        //
        //     Debug.Log($"Sound Effect Volume set to: {volume * 100}%");
        // }
        //
        //
        // private void OnBrightnessSliderChanged(ChangeEvent<float> evt)
        // {
        //     float brightness = evt.newValue / 100f; // Convert from 0-100 to 0-1 range
        //     SetBrightness(brightness);
        // }
        //
        // private void SetBrightness(float brightness)
        // {
        //     if (_exposure != null)
        //     {
        //         // Adjust the fixed exposure value
        //         _exposure.fixedExposure.value = Mathf.Lerp(-2f, 2f, brightness); // Adjust range as needed
        //         Debug.Log($"Brightness set to: {brightness * 100}% (Exposure value: {_exposure.fixedExposure.value})");
        //     }
        // }
    }
}
