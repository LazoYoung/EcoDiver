using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static Unity.XR.CoreUtils.XROrigin.TrackingOriginMode;

namespace Script.UI
{
    public class SettingsManager : MonoBehaviour
    {
        private static SettingsManager _instance;

        public static SettingsManager Instance
        {
            get
            {
                if (!_instance)
                {
                    SetupInstance();
                }

                return _instance;
            }
        }

        private static void SetupInstance()
        {
            _instance = FindObjectOfType<SettingsManager>();
            if (_instance == null)
            {
                GameObject gameObj = new GameObject("SettingsManager");
                _instance = gameObj.AddComponent<SettingsManager>();
                DontDestroyOnLoad(gameObj);
            }
        }

        private const float InitialBrightnessValue = 100f;
        private const float InitialSoundEffectValue = 100f;
        private const XROrigin.TrackingOriginMode InitialTrackingMode = Device;

        private float _brightness = InitialBrightnessValue; // Default brightness value
        private float _soundVolume = InitialSoundEffectValue; // Default sound volume value
        private XROrigin.TrackingOriginMode _trackingMode = InitialTrackingMode;

        [SerializeField] [Tooltip("Limit frame-rate")]
        private int maxFrameRate = 60;
        [SerializeField] [Tooltip("Post Process Profile")]
        private PostProcessProfile _postProcessProfile;
        [SerializeField] [Tooltip("Post Process Layer")]
        private PostProcessLayer _postProcessLayer;

        private AutoExposure _autoExposure;

        public XROrigin.TrackingOriginMode TrackingMode
        {
            get => _trackingMode;
            set
            {
                _trackingMode = value;
                UpdateXROrigin();
            }
        }

        public float Brightness
        {
            get => _brightness;
            set
            {
                _brightness = value;
                AdjustBrightness(_brightness);
            }
        }

        public float SoundVolume
        {
            get => _soundVolume;
            set
            {
                _soundVolume = value;
                // Update sound volume in your game
                AdjustSoundEffect(_soundVolume);
            }
        }

        public bool IsFloorInitialized { get; set; }

        public void ClearSettings()
        {
            Brightness = InitialBrightnessValue;
            SoundVolume = InitialSoundEffectValue;
            TrackingMode = InitialTrackingMode;
        }
        
        public void Reload()
        {
            UpdateXROrigin();
            UpdateFrameRate();
        }

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            if(_postProcessProfile == null)
            {
                Debug.LogError("Post Process Profile is null!");
                return;
            }
            if(_postProcessLayer == null)
            {
                Debug.LogWarning("Post Process Layer is null! Please Set Main Camera.");
            }
            _postProcessProfile.TryGetSettings(out _autoExposure);
            UpdateFrameRate();
        }

        private void AdjustSoundEffect(float volume)
        {
            // To prevent the volume from temporarily changing to 0.
            if (volume != 0f)
            {
                AudioListener.volume = volume / 100f;
            }
            else
            {
                AudioListener.volume = 0.03f;
            }
        }

        private void AdjustBrightness(float volume)
        {
            // To prevent the volume from temporarily changing to 0.
            if (volume != 0f)
            {
                _autoExposure.keyValue.value = volume / 100f;
            }
            else
            {
                _autoExposure.keyValue.value = 0.03f;
            }
        }

        private void UpdateXROrigin()
        {
            var xrOrigin = FindObjectOfType<XROrigin>();
            xrOrigin.RequestedTrackingOriginMode = _trackingMode;
        }

        private void UpdateFrameRate()
        {
            Application.targetFrameRate = maxFrameRate;
            Time.fixedDeltaTime = 1f / maxFrameRate;
        }
    
        private void OnApplicationQuit()
        {
            _autoExposure.keyValue.value = 1f;
        }
    }
}
