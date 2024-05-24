using UnityEngine;

namespace Script.Quest.Entity
{
    public class Robot : MonoBehaviour
    {
        public GameObject chainedPrefab;
        public new Camera camera;
        private bool _virgin = true;
        
        private void Start()
        {
            if (chainedPrefab == null)
            {
                Debug.LogError("Chained prefab is not assigned!");
            }

            if (camera == null)
            {
                camera = Camera.main;
                Debug.LogWarning("It's recommended to manually assign the camera.");
            }

            if (camera == null)
            {
                Debug.LogError("Could not locate the camera!");
            }

            SetupBoxCollider();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_virgin && other.CompareTag("Hand"))
            {
                var tf = transform;
                var obj = Instantiate(chainedPrefab, tf.position, tf.rotation, tf.parent);
                var rot = obj.transform.eulerAngles;
                obj.transform.eulerAngles = new Vector3(rot.x, rot.y, 0f);
                var chainedRobot = obj.GetComponentInChildren<ChainedRobot>();
                _virgin = false;
                
                chainedRobot.SetCameraTransform(camera.transform);
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
    }
}
