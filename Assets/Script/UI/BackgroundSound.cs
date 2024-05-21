using UnityEngine;

namespace Script.UI
{
    public class BackgroundSound: MonoBehaviour
    {
        [SerializeField] [Tooltip("Sound to play when the item is collected.")]
        private AudioClip backgroundSound;

        private void PlaySound()
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && backgroundSound != null)
            {
                audioSource.PlayOneShot(backgroundSound);
            }
            else
            {
                Debug.LogWarning("Audio Source or Background Sound is null!");
            }
        }

        private void Start()
        {
            Debug.Log("Background Sound: " + backgroundSound);
            PlaySound();
        }

        private void OnDestroy()
        {
            Debug.Log("Background Sound DESTORY: " + backgroundSound);
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && backgroundSound != null)
            {
                audioSource.Stop();
            }
        }
    }
}
