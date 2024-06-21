using UnityEngine;
using UnityEngine.Events;
namespace Script.Interaction
{
    [RequireComponent(typeof(BoxCollider))]
    public class AreaTrigger : MonoBehaviour
    {
        public UnityEvent onEnter;
        public UnityEvent onStay;
        public UnityEvent onExit;
        
        private void Start()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onEnter?.Invoke();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onStay?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                onExit?.Invoke();
            }
        }
    }
}
