using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Script.Environment.Collect
{
    public class CollectableItem : XRGrabInteractable
    {
        private float grabTimer = 0.0f;
        [SerializeField]
        [Tooltip("Duration in seconds to hold the item to collect it.")]
        private float grabDurationThreshold = 1.0f; // Threshold in seconds
        private bool timerActive = false;

        [SerializeField]
        [Tooltip("Sound to play when the item is collected.")]
        private AudioClip collectSound;




        void OnTriggerEnter(Collider other)
        {
            Debug.Log("collider: " + other);
            Debug.Log("collider: " + other.tag);

            // Check if the interacting collider is a hand
            if (other.CompareTag("Hand"))
            {
                CollectManager.Instance.CollectItem();
                gameObject.SetActive(false);
                OnCollect();
            }
        }
        // protected void OnSelectEntered()
        // {
        //     grabTimer = 0.0f;
        //     timerActive = true;
        // }
        //
        // protected void OnSelectExited()
        // {
        //     timerActive = false;
        // }
        //
        // private void FixedUpdate()
        // {
        //     if (timerActive)
        //     {
        //         grabTimer += Time.deltaTime;
        //         if (grabTimer >= grabDurationThreshold)
        //         {
        //             timerActive = false; // Prevent multiple logs
        //
        //         }
        //     }
        // }

        private void OnCollect()
        {
            Debug.Log("Held for over one second!");
            PlayCollectSound();
        }

        private void PlayCollectSound()
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
            if (audioSource != null && collectSound != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
        }
    }
}
