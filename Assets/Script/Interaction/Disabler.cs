using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Interaction
{
    public abstract class Disabler : MonoBehaviour
    {
        public virtual void Enable(bool debugMode)
        {
            Debug.LogWarning("Enable is not implemented.");
        }

        public virtual void Disable(bool debugMode)
        {

            Debug.LogWarning("Disable is not implemented.");
        }
    }
}
