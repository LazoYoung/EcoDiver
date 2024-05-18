using System;
using UnityEngine;

namespace Script.Interaction
{
    public class Sliceable : MonoBehaviour
    {
        public Action<Slicer> onSlice;
        
        public void Notify(Slicer slicer)
        {
            onSlice?.Invoke(slicer);
        }

        private void Awake()
        {
            if (!TryGetComponent(out Collider _))
            {
                Debug.LogWarning("A collider is supposed to be attached to this Sliceable object.");
            }
        }
    }
}
