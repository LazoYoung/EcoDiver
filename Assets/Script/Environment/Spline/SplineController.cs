using System.Collections;
using Dreamteck.Splines;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Script.Environment.Spline
{
    [RequireComponent(typeof(ObjectController))]
    public class SplineController : MonoBehaviour
    {
        [SerializeField] private float followDuration = 10f;
        [SerializeField] private bool loop = true;
        [SerializeField] private bool pingPong;
        [Tooltip("Motion quality is degraded if player is out of this range.")]
        [SerializeField] private float renderDistance = 50f;
        [Tooltip("Degraded motion frequency in seconds.")]
        [SerializeField] private float motionFrequency = 0.05f;
        [Tooltip("Position of an entity")]
        [SerializeField] private Transform entity;
        [Tooltip("Position of the player's camera")]
        [SerializeField] private new Transform camera;
        private float DeltaTime => InProximity() ? Time.deltaTime : motionFrequency;
        private ObjectController _objController;
        private float _speed;
        private float _distance;

        private void Start()
        {
            if (camera == null)
            {
                camera = FindObjectOfType<XROrigin>()?.Camera.transform;
            }
            
            _objController = GetComponent<ObjectController>();
            _objController.Spawn();
            _speed = 1 / followDuration;
            
            if (entity == null)
            {
                entity = transform.GetChild((int) Mathf.Floor(transform.childCount / 2f)).transform;
            }
            
            StartCoroutine(UpdateOffsetCoroutine());
            StartCoroutine(UpdateDistanceCoroutine());
        }

        private bool InProximity()
        {
            return _distance <= renderDistance;
        }
        
        private IEnumerator UpdateDistanceCoroutine()
        {
            while (true)
            {
                _distance = Vector3.Distance(camera.position, entity.position);
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator UpdateOffsetCoroutine()
        {
            while (true)
            {
                UpdateOffset();

                if (InProximity())
                {
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    yield return new WaitForSeconds(motionFrequency);
                }
            }
        }

        private void UpdateOffset()
        {
            var offset = _objController.evaluateOffset;
            _objController.evaluateOffset += _speed * DeltaTime;

            if (offset > 1f || offset < 0f)
            {
                if (!loop)
                {
                    _speed = 0;
                }
                else if (pingPong)
                {
                    _speed *= -1;
                }
                else
                {
                    _objController.evaluateOffset = 0f;
                }
            }
        }
    }
}
