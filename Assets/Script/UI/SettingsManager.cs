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

    private float brightness = 1f; // Default brightness value
    private float soundVolume = 1f; // Default sound volume value


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
        Brightness = 1f;
        SoundVolume = 1f;
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
        _postProcessProfile.TryGetSettings(out _autoExposure);
    }

    private void AdjustSoundEffect(float volume)
    {
        AudioListener.volume = volume / 100f;
    }

    private void AdjustBrightness(float volume)
    {
        if (volume != 0f)
        {
            _autoExposure.keyValue.value = volume;
        }
        else
        {
            _autoExposure.keyValue.value = 0.03f;
        }
    }

    void OnDisable()
    {
        Debug.Log("SettingsManager is disabled");
        _autoExposure.keyValue.value = 1f;
    }

}
