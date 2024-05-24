using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public int delay = 3;
        public UnityEvent onInteract;
        private bool _active;

        private void Start()
        {
            if (!TryGetComponent(out Collider _))
            {
                var meshCollider = gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.isTrigger = true;
            }

            InhibitCollisionAgainstPlayer();
            StartCoroutine(Activate());
        }
        
        private void InhibitCollisionAgainstPlayer()
        {
            var obj = GameObject.FindWithTag("Player");

            if (obj != null)
            {
                Physics.IgnoreCollision(obj.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }

        private IEnumerator Activate()
        {
            yield return new WaitForSeconds(delay);
            _active = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_active)
            {
                onInteract.Invoke();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_active)
            {
                onInteract.Invoke();
            }
        }
    }
}
