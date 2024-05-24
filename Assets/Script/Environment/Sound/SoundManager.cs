using Crest;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio; // Added to use AudioMixer

public class SoundManager : MonoBehaviour
{
    public OceanRenderer oceanRenderer; // Assign the OceanRenderer component from Crest
    public AudioSource audioSource; // Assign the AudioSource component

    // Added AudioMixerGroup
    public AudioMixerGroup effectsMixerGroup;

    // Audio clips for different situations
    public AudioClip startSound;
    public AudioClip underwaterSound;
    public AudioClip touchMeshSound;

    // Delay times for each sound
    public float startSoundDelay = 0f;
    public float underwaterSoundDelay = 0f;
    public float touchMeshSoundDelay = 0f;

    private bool isUnderwater = false; // Whether the player is currently underwater
    private SampleHeightHelper _sampleHeightHelper; // Helper for sampling height

    public GameObject targetMesh; // The specific mesh object to set in the inspector

    private bool hasPlayedTouchMeshSound = false; // To ensure the touch mesh sound plays only once

    private void Start()
    {
        _sampleHeightHelper = new SampleHeightHelper();

        // Assign AudioMixerGroup to AudioSource
        audioSource.outputAudioMixerGroup = effectsMixerGroup;

        // Play the start sound after a delay
        if (startSound) StartCoroutine(PlaySoundWithDelay(startSound, startSoundDelay));
    }

    private void Update()
    {
        _sampleHeightHelper.Init(Camera.main.transform.position, 0f);

        if (_sampleHeightHelper.Sample(out float waterHeight))
        {
            float cameraHeight = Camera.main.transform.position.y;
            bool currentlyUnderwater = cameraHeight < waterHeight;

            if (isUnderwater != currentlyUnderwater)
            {
                isUnderwater = currentlyUnderwater;

                if (isUnderwater)
                {
                    StartCoroutine(PlaySoundWithDelay(underwaterSound, underwaterSoundDelay));
                }
            }
        }

        CheckCameraInsideMesh();
    }

    // Check if the main camera's center is inside the target mesh and play the sound once
    private void CheckCameraInsideMesh()
    {
        if (targetMesh == null) return;

        // Get the bounds of the target mesh
        Bounds meshBounds = targetMesh.GetComponent<Renderer>().bounds;

        // Check if the main camera's position is within the mesh bounds
        if (meshBounds.Contains(Camera.main.transform.position))
        {
            if (!hasPlayedTouchMeshSound)
            {
                StartCoroutine(PlaySoundWithDelay(touchMeshSound, touchMeshSoundDelay));
                hasPlayedTouchMeshSound = true;
            }
        }
        else
        {
            // Reset the flag when the camera exits the mesh bounds, allowing the sound to be played again if re-entered
            hasPlayedTouchMeshSound = false;
        }
    }

    IEnumerator PlaySoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(clip);
    }
}
