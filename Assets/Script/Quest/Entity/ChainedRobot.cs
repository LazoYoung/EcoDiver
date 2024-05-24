using Script.Interaction;
using UnityEngine;

namespace Script.Quest.Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChainedRobot : MonoBehaviour
    {
        public GameObject lastChain;
        private Interactable _interactable;
        private Transform _camera;

        public void SetCameraTransform(Transform tf)
        {
            _camera = tf;
        }
        
        private void Start()
        {
            InhibitCollisionAgainstPlayer();
            // _interactable = lastChain.GetComponent<Interactable>(); 
            // _interactable.onInteract.AddListener(OnInteract);
        }

        // private void OnInteract()
        // {
        //     _grab = true;
        //     _interactable.onInteract.RemoveListener(OnInteract);
        //     InhibitCollisionAgainstPlayer();
        // }

        private void FixedUpdate()
        {
            if (_camera)
            {
                var pos = _camera.position;
                var chainBody = lastChain.GetComponent<Rigidbody>();
                var newPos = -0.3f * _camera.forward - 0.3f * _camera.up + pos;
                chainBody.isKinematic = true;
                chainBody.MovePosition(newPos);
                chainBody.MoveRotation(_camera.rotation);
            }
        }

        private void InhibitCollisionAgainstPlayer()
        {
            var obj = GameObject.FindWithTag("Player");

            if (obj != null)
            {
                Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }
}
