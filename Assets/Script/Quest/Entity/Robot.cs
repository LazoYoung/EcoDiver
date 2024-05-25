using System;
using Script.Equipment;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Object = UnityEngine.Object;

namespace Script.Quest.Entity
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class Robot : MonoBehaviour
    {
        public Action<ChainedRobot> OnChainTied;
        public GameObject chainedPrefab;
        public Tool chains;
        private Equipment.Equipment _equipment;
        private XRGrabInteractable _grabInteractable;

        private void Start()
        {
            if (!TryFindObject(out _equipment))
            {
                Debug.LogError("Equipment system is either missing or inactive!");
            }
            
            if (chains == null)
            {
                Debug.LogError("Chains tool is not assigned!");
            }
            
            if (chainedPrefab == null)
            {
                Debug.LogError("Chained prefab is not assigned!");
            }
            
            SetupBoxCollider();
            SetupGrabInteractable();
        }

        private void SetupGrabInteractable()
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
            _grabInteractable.selectEntered.AddListener(OnGrab);
        }

        private void OnGrab(SelectEnterEventArgs arg0)
        {
            if (_equipment.GetActiveTool().Equals(chains))
            {
                var tf = transform;
                var obj = Instantiate(chainedPrefab, tf.position, tf.rotation, tf.parent);
                var rot = obj.transform.eulerAngles;
                obj.transform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
                OnChainTied?.Invoke(obj.GetComponent<ChainedRobot>());

                _grabInteractable.selectEntered.RemoveListener(OnGrab);
                Destroy(gameObject);
            }
        }
        
        private void SetupBoxCollider()
        {
            if (!TryGetComponent(out BoxCollider boxCollider))
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(1f, 1f, 5f);
        }
        
        private static bool TryFindObject<T>(out T instance) where T : Object
        {
            instance = FindObjectOfType<T>();
            return instance != null;
        }
    }
}
