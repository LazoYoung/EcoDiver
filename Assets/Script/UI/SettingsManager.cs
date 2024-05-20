using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;

    public static SettingsManager Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }

    private static void SetupInstance()
    {
        instance = FindObjectOfType<SettingsManager>();
        if (instance == null)
        {
            GameObject gameObj = new GameObject("SettingsManager");
            instance = gameObj.AddComponent<SettingsManager>();
            DontDestroyOnLoad(gameObj);
        }
    }

    private const float initialBrightnessValue = 100f;
    private const float initialSoundEffectValue = 100f;

    private float brightness = initialBrightnessValue; // Default brightness value
    private float soundVolume = initialSoundEffectValue; // Default sound volume value


    [SerializeField] [Tooltip("Post Process Profile")]
    private PostProcessProfile _postProcessProfile;
    [SerializeField] [Tooltip("Post Process Layer")]
    private PostProcessLayer _postProcessLayer;

    private AutoExposure _autoExposure;

    public float Brightness
    {
        get => brightness;
        set
        {
            brightness = value;
            AdjustBrightness(brightness);
        }
    }

    public float SoundVolume
    {
        get => soundVolume;
        set
        {
            soundVolume = value;
            // Update sound volume in your game
            AdjustSoundEffect(soundVolume);
        }
    }

    public void ClearSettings()
    {
        Brightness = initialBrightnessValue;
        SoundVolume = initialSoundEffectValue;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    private void OnApplicationQuit()
    {
        _autoExposure.keyValue.value = 1f;
    }

}
