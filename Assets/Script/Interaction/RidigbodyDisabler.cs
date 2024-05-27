using UnityEngine;

namespace Script.Interaction
{
    public class RigidbodyDisabler : Disabler
    {
        [SerializeField] [Tooltip("The GameObject whose Rigidbody will be disabled.")]
        private GameObject targetObject;

        private Rigidbody targetRigidbody;

        void Start()
        {
            if (targetObject != null)
            {
                targetRigidbody = targetObject.GetComponent<Rigidbody>();
                
                if (targetRigidbody == null)
                {
                    Debug.LogWarning("Rigidbody component not found on " + targetObject.name);
                }
            }
            else
            {
                Debug.LogWarning("Target Object is not assigned.");
            }
        }

        public override void Disable(bool debugMode)
        {
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = true; // This will disable physics interactions
                // targetRigidbody.detectCollisions = false; // Optionally disable collision detection

                if (debugMode)
                {
                    Debug.Log("Rigidbody on " + targetObject.name + " has been disabled.");
                }
            }
            else if (debugMode)
            {
                Debug.LogWarning("Target Rigidbody is not assigned or already disabled.");
            }
        }

        public override void Enable(bool debugMode)
        {
            if (targetRigidbody != null)
            {
                targetRigidbody.isKinematic = false; // Re-enable physics interactions
                // targetRigidbody.detectCollisions = true; // Re-enable collision detection

                if (debugMode)
                {
                    Debug.Log("Rigidbody on " + targetObject.name + " has been enabled.");
                }
            }
            else if (debugMode)
            {
                Debug.LogWarning("Target Rigidbody is not assigned or already enabled.");
            }
        }
    }
}