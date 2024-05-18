using System.Collections;
using EzySlice;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Interaction
{
    public class Slicer : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        private readonly int _velocityFrames = 5;
        private int _sampleCount;
        private Coroutine _routine;
        private Vector3[] _velocitySamples;

        public void Slice(Sliceable target)
        {
            var targetObject = target.gameObject;
            var bladeVector = endPoint.position - startPoint.position;
            var sliceNormal = Vector3.Cross(bladeVector, GetVelocityEstimate()).normalized;
            
            if (target.TryGetComponent(out Cloth cloth))
            {
                var skinnedMeshRenderer = target.GetComponent<SkinnedMeshRenderer>();
                var material = skinnedMeshRenderer.material;
                Destroy(cloth);
                Destroy(skinnedMeshRenderer);
                var meshRenderer = target.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
            }

            var slicedHull = targetObject.Slice(endPoint.position, sliceNormal);
            
            if (slicedHull != null)
            {
                var upperHull = slicedHull.CreateUpperHull(targetObject);
                var lowerHull = slicedHull.CreateLowerHull(targetObject);
            
                ComposeSlicedObject(targetObject, upperHull, true);
                ComposeSlicedObject(targetObject, lowerHull, false);
                Destroy(targetObject); 
            }
        }
        
        private void Start()
        {
            _velocitySamples = new Vector3[_velocityFrames];
        }

        private void OnEnable()
        {
            BeginEstimatingVelocity();
        }

        private void OnDisable()
        {
            FinishEstimatingVelocity();
        }

        private void FixedUpdate()
        {
            if (Physics.Linecast(startPoint.position, endPoint.position, out var hitInfo))
            {
                if (hitInfo.transform.TryGetComponent(out Sliceable target))
                {
                    Slice(target);
                    target.Notify(this);                    
                }
            }
        }

        private void ComposeSlicedObject(GameObject target, GameObject hull, bool upper)
        {
            if (target.TryGetComponent(out Cloth _))
            {
                var tf = hull.transform;
                var pos = tf.position;
                var meshRenderer = hull.GetComponent<MeshRenderer>();
                var material = meshRenderer.material;
                Destroy(meshRenderer);
                var skinnedMeshRenderer = hull.AddComponent<SkinnedMeshRenderer>();
                var cloth = hull.AddComponent<Cloth>();
                float offset = upper ? 0.1f : -0.1f;
                skinnedMeshRenderer.material = material;
                cloth.useGravity = true;
                tf.position = new Vector3(pos.x + offset, pos.y, pos.z + offset);
            }
            else
            {
                var slicedBody = hull.AddComponent<Rigidbody>();
                var meshCollider = hull.AddComponent<MeshCollider>();
                slicedBody.useGravity = true;
                meshCollider.convex = true;
                slicedBody.AddExplosionForce(100f, hull.transform.position, 1f);  
            }
        }

        private Vector3 GetVelocityEstimate()
        {
            var velocity = Vector3.zero;
            int sampleCount = Mathf.Min(_sampleCount, _velocitySamples.Length);

            if (sampleCount != 0)
            {
                for (var i = 0; i < sampleCount; i++)
                {
                    velocity += _velocitySamples[i];
                }

                velocity *= 1.0f / sampleCount;
            }

            return velocity;
        }

        private void BeginEstimatingVelocity()
        {
            FinishEstimatingVelocity();
            _routine = StartCoroutine(EstimateVelocityCoroutine());
        }

        private void FinishEstimatingVelocity()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);
                _routine = null;
            }
        }

        private IEnumerator EstimateVelocityCoroutine()
        {
            _sampleCount = 0;
            var prevPos = transform.position;

            while (true)
            {
                yield return new WaitForEndOfFrame();

                float velocityFactor = 1.0f / Time.deltaTime;
                int v = _sampleCount % _velocitySamples.Length;
                _sampleCount++;
                _velocitySamples[v] = velocityFactor * (transform.position - prevPos);
                prevPos = transform.position;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
