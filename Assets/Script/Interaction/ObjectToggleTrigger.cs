using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Interaction
{
    public class ObjectToggleTrigger : MonoBehaviour
    {
        [FormerlySerializedAs("rayObjects")] [SerializeField] [Tooltip("Array of GameObjects To Toggle.")]
        private GameObject[] toggleObjects; // Array of ray GameObjects

        [SerializeField] [Header("Debug")] private bool debugMode;

        [SerializeField]
        [Tooltip("Default state(No Enter)'s Object's mode. If true, the object will be enable by default.")]
        private bool isDefaultDisabled = true;

        private void Start()
        {
            Reset();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            Enter();
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if the exiting object has the tag "Player"
            if (other.CompareTag("Player"))
            {
                Reset();
            }
        }

        private void Enter()
        {
            if (isDefaultDisabled)
            {
                enableObjects();
            }
            else
            {
                disableObjects();
            }
        }

        private void Reset()
        {
            if (isDefaultDisabled)
            {
                disableObjects();
            }
            else
            {
                enableObjects();
            }
        }

        private void enableObjects()
        {
            foreach (var rayObject in toggleObjects)
            {
                if (rayObject != null)
                {
                    if (debugMode)
                    {
                        Debug.Log("ObjectToggleTrigger "+ this +": Enable");
                    }

                    rayObject.SetActive(true); // Enable all rays
                }
            }
        }

        private void disableObjects()
        {
            foreach (var rayObject in toggleObjects)
            {
                if (rayObject != null)
                {
                    if (debugMode)
                    {
                        Debug.Log("ObjectToggleTrigger "+ this +": Disabling ");
                    }

                    rayObject.SetActive(false); // Disable all rays
                }
            }
        }
    }
}