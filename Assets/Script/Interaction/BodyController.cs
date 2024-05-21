using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Script.Interaction
{
    /**
     * <summary>
     * BodyController takes control of the player's capsule collider.
     * </summary>
     */
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

        [SerializeField]
        private CapsuleCollider bodyCollider;

        [SerializeField]
        private LocomotionSystem locomotionSystem;

        [SerializeField]
        private bool freeze;

        private void Start()
        {
            if (cameraTransform == null)
            {
                Debug.LogError("Camera transform is required to drive BodyController!");
                enabled = false;
                return;
            }

            if (bodyCollider == null)
            {
                Debug.LogError("Body collider is required to drive BodyController!");
                enabled = false;
                return;
            }

            if (locomotionSystem == null)
            {
                locomotionSystem = FindObjectOfType<LocomotionSystem>();
            }

            if (freeze && locomotionSystem != null)
            {
                Freeze();
            }

            bodyCollider.hideFlags = HideFlags.NotEditable;
        }

        private void Freeze()
        {
            foreach (var locomotionProvider in locomotionSystem.GetComponentsInChildren<LocomotionProvider>())
            {
                locomotionProvider.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            var pos = cameraTransform.localPosition;
            var bodyHeight = Mathf.Clamp(pos.y, minHeight, maxHeight);
            bodyCollider.height = bodyHeight;
            bodyCollider.center = new Vector3(pos.x, bodyHeight / 2, pos.z);
        }
    }
}