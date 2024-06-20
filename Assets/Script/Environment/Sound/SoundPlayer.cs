using System.Collections;
using UnityEngine;

namespace Script.Environment.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        public AudioClip clip;
        public AudioSource source;
        public float delay;

        public void PlayOnce()
        {
            StartCoroutine(PlayWithDelay());
        }

        private IEnumerator PlayWithDelay()
        {
            yield return new WaitForSeconds(delay);
            source.PlayOneShot(clip);
        }

        private void Start()
        {
            if (clip == null)
            {
                Debug.LogError("No audio clip assigned.");
                enabled = false;
                return;
            }

            if (source == null)
            {
                Debug.LogError("No audio source assigned.");
                enabled = false;
                return;
            }
        }
    }
}
