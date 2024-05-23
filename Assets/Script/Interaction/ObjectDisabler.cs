using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Interaction
{
    public class ObjectDisabler : MonoBehaviour
    {
        [FormerlySerializedAs("toggle objects")] [SerializeField] [Tooltip("Array of GameObjects To Toggle.")]
        private GameObject[] toggleObjects; // Array of GameObjects

        public void EnableObjects(bool debugMode)
        {
            foreach (var obj in toggleObjects)
            {
                if (obj != null)
                {
                    if (debugMode)
                    {
                        Debug.Log("ObjectDisabler: Enabling " + obj.name);
                    }
                    obj.SetActive(true); // Enable all objects
                }
            }
        }

        public void DisableObjects(bool debugMode)
        {
            foreach (var obj in toggleObjects)
            {
                if (obj != null)
                {
                    if (debugMode)
                    {
                        Debug.Log("ObjectDisabler: Disabling " + obj.name);
                    }
                    obj.SetActive(false); // Disable all objects
                }
            }
        }
    }
}
