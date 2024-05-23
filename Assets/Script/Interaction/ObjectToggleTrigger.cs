using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Interaction
{
    public class ObjectToggleTrigger : MonoBehaviour
    {
        [SerializeField] [Header("Debug")] private bool debugMode;

        [SerializeField]
        [Tooltip("Default state(No Enter)'s Object's mode. If true, the object will be enable by default.")]
        private bool isDefaultDisabled = true;

        [SerializeField] [Tooltip("ObjectDisabler script to disable objects.")]
        private ObjectDisabler objectDisabler;

        private void Start()
        {
            Reset();
        }

        private void OnDisable()
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
            objectDisabler.EnableObjects(debugMode);
        }

        private void disableObjects()
        {
            objectDisabler.DisableObjects(debugMode);
        }
    }
}
