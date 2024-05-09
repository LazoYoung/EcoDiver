using UnityEngine;

namespace Script.Interaction
{
    /**
     * <summary>
     * BodyController takes control of the player's capsule collider.
     * </summary>
     */
    [RequireComponent(typeof(CapsuleCollider))]
    public class BodyController : MonoBehaviour
    {
        [SerializeField]
        private Transform cameraTransform;
        
        [SerializeField]
        [Range(0, 3)]
        private float minHeight = 1f;
        
        [SerializeField]
        [Range(0, 3)]
        private float maxHeight = 2f;

        private CapsuleCollider _collider;

        private void Start()
        {
            if (cameraTransform == null)
            {
                Debug.LogError("Camera transform is required to drive BodyController!");
                enabled = false;
                return;
            }

            _collider = GetComponent<CapsuleCollider>();
            _collider.radius = 0.1f;
            _collider.hideFlags = HideFlags.NotEditable;
        }

        private void FixedUpdate()
        {
            var pos = cameraTransform.localPosition;
            float bodyHeight = Mathf.Clamp(pos.y, minHeight, maxHeight);
            _collider.height = bodyHeight;
            _collider.center = new Vector3(pos.x, bodyHeight / 2, pos.z);
        }
    }
}
