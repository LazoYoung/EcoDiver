using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Script.Collect
{
    public class CollectableItem : XRGrabInteractable
    {
        private float grabTimer = 0.0f;
        private const float grabDurationThreshold = 1.0f; // Threshold in seconds
        private bool timerActive = false;

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            grabTimer = 0.0f;
            timerActive = true;
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            timerActive = false;
        }

        private void FixedUpdate()
        {
            if (timerActive)
            {
                grabTimer += Time.deltaTime;
                if (grabTimer >= grabDurationThreshold)
                {
                    Collect();
                }
            }
        }

        private void Collect()
        {
            Debug.Log("Held for over one second!");
            timerActive = false; // Prevent multiple logs

            CollectManager.Instance.CollectItem();
            gameObject.SetActive(false);
        }
    }
}
