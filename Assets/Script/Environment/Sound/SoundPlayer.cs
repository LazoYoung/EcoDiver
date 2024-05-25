using System.Collections;
using UnityEngine;

namespace Script.Environment.Sound
{
    public class SoundPlayer : MonoBehaviour
    {
        public AudioClip clip;
        public float delay;
        private SoundManager _manager;
        private AudioSource _source;

        public void PlayOnce()
        {
            StartCoroutine(PlayWithDelay());
        }

        private IEnumerator PlayWithDelay()
        {
            yield return new WaitForSeconds(delay);
            _source.PlayOneShot(clip);
        }

        private void Start()
        {
            if (clip == null)
            {
                Debug.LogError("No audio clip assigned.");
                enabled = false;
                return;
            }
            
            _manager = FindObjectOfType<SoundManager>();
            _source = _manager.audioSource;
        }
    }
}
