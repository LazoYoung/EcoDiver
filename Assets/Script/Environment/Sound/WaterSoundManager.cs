using Crest;
using UnityEngine;

namespace Script.Environment.Sound
{
    public class WaterSoundManager : MonoBehaviour
    {
        public OceanRenderer oceanRenderer; // Assign the OceanRenderer component from Crest
        public AudioSource audioSource; // Assign the AudioSource component
        public AudioClip divingSound; // Diving sound
        public AudioClip cameOutSound; // Sound for coming out of the water
        public AudioClip underwaterLoopSound; // Underwater loop sound
        public AudioClip surfaceLoopSound; // Surface loop sound

        private bool isUnderwater = false; // Whether the player is currently underwater
        private SampleHeightHelper _sampleHeightHelper; // Helper for sampling height

        private void Start()
        {
            _sampleHeightHelper = new SampleHeightHelper();
            audioSource.clip = surfaceLoopSound;
            audioSource.loop = true; // Set the audio source to loop
            audioSource.Play();
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
                        audioSource.PlayOneShot(divingSound);
                        audioSource.clip = underwaterLoopSound; // Change the loop sound to underwater sound
                        audioSource.loop = true; // Ensure the sound loops
                    }
                    else
                    {
                        audioSource.PlayOneShot(cameOutSound);
                        audioSource.clip = surfaceLoopSound; // Change the loop sound to surface sound
                        audioSource.loop = true; // Ensure the sound loops
                    }
                    audioSource.Play(); // Play the new loop sound
                }
            }
        }
    }
}