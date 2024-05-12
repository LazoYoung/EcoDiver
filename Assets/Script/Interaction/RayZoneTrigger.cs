using UnityEngine;

namespace Script.Interaction
{
    public class RayZoneTrigger : MonoBehaviour
    {
        [SerializeField] [Tooltip("Array of ray GameObjects To Manage.")]
        private GameObject[] rayObjects;  // Array of ray GameObjects

        [SerializeField] [Header("Debug")]
        private bool debugMode;

        private void Start()
        {
            disableRay();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter By: " + other.tag);
            // Check if the entering object has the tag "Player"
            if (other.CompareTag("Player"))
            {
                enableRay();
            }
        }

        private void enableRay()
        {
            foreach (var rayObject in rayObjects)
            {
                if (rayObject != null)
                {
                    if (debugMode) {
                        Debug.Log("RayZoneTrigger: Enable ray");
                    }
                    rayObject.SetActive(true);  // Enable all rays
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if the exiting object has the tag "Player"
            if (other.CompareTag("Player"))
            {
                disableRay();
            }
        }

        private void disableRay()
        {
            foreach (var rayObject in rayObjects)
            {
                if (rayObject != null)
                {
                    if (debugMode) {
                        Debug.Log("RayZoneTrigger: Disabling ray");
                    }
                    rayObject.SetActive(false);  // Disable all rays
                }
            }
        }
    }
}
