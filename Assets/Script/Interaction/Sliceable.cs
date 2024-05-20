using System;
using UnityEngine;

namespace Script.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Sliceable : MonoBehaviour
    {
        public Action<Slicer> OnSlice;
        
        public void Notify(Slicer slicer)
        {
            OnSlice?.Invoke(slicer);
        }

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
        }
    }
}
