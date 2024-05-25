using UnityEngine;
using UnityEngine.Events;

namespace Script.Interaction
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Sliceable : MonoBehaviour
    {
        public UnityEvent<Slicer> onSlice;
        
        public void Notify(Slicer slicer)
        {
            onSlice?.Invoke(slicer);
        }

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
        }
    }
}
