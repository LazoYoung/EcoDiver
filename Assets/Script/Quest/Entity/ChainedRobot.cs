using System;
using System.Collections;
using Script.Equipment;
using Script.Interaction;
using Unity.XR.CoreUtils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.Quest.Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChainedRobot : MonoBehaviour
    {
        public Action<Robot> OnChainUntied;
        public GameObject robotPrefab;
        public GameObject lastChain;
        public Tool chains;
        private XROrigin _xrOrigin;
        private Equipment.Equipment _equipment;
        private Interactable _interactable;
        private Transform _camera;
        private Transform _body;
        
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
            
            if (robotPrefab == null)
            {
                Debug.LogError("Robot prefab is not assigned!");
            }

            if (lastChain == null)
            {
                Debug.LogError("Last chain is not assigned!");
            }

            if (!TryFindObject(out _xrOrigin))
            {
                Debug.LogError("XR Origin is either missing or inactive!");
            }

            _camera = _xrOrigin?.Camera.transform;
            _body = _xrOrigin?.transform;
            
            InhibitCollisionAgainstPlayer();
            StartCoroutine(MonitorEquipment());
        }

        private IEnumerator MonitorEquipment()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(1f);
                
                if (!_equipment.GetActiveTool().Equals(chains))
                {
                    Untie();
                }
            }
        }
        
        private void Untie()
        {
            var tf = transform;
            var obj = Instantiate(robotPrefab, tf.position, tf.rotation, tf.parent);
            var rot = obj.transform.eulerAngles;
            obj.transform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
            OnChainUntied?.Invoke(obj.GetComponent<Robot>());
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (_camera)
            {
                var pos = _camera.position;
                var chainBody = lastChain.GetComponent<Rigidbody>();
                var newPos = (-0.5f * _body.up) + (0.2f * _body.right) + pos;
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
        
        private static bool TryFindObject<T>(out T instance) where T : Object
        {
            instance = FindObjectOfType<T>();
            return instance != null;
        }
    }
}
