using System.Collections;
using EzySlice;
using UnityEngine;

namespace Script.Interaction
{
    internal record ClothColliders
    {
        internal CapsuleCollider[] CapsuleColliders;
        internal ClothSphereColliderPair[] SphereColliders;
    }

    public class Slicer : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;
        private readonly int _velocityFrames = 5;
        private readonly float _tearDistance = 0.05f;
        private int _sampleCount;
        private Coroutine _routine;
        private Vector3[] _velocitySamples;

        public void Slice(Sliceable target)
        {
            var targetObject = Instantiate(target.gameObject, target.transform.parent);
            var bladeVector = endPoint.position - startPoint.position;
            var sliceNormal = Vector3.Cross(bladeVector, GetVelocityEstimate()).normalized;
            ClothColliders clothColliders = null;

            if (targetObject.TryGetComponent(out Cloth cloth))
            {
                clothColliders = new ClothColliders()
                {
                    CapsuleColliders = cloth.capsuleColliders,
                    SphereColliders = cloth.sphereColliders
                };
                var skinnedMeshRenderer = targetObject.GetComponent<SkinnedMeshRenderer>();
                var material = skinnedMeshRenderer.material;
                DestroyImmediate(cloth);
                DestroyImmediate(skinnedMeshRenderer);
                var meshRenderer = targetObject.AddComponent<MeshRenderer>();
                meshRenderer.material = material;
            }

            var slicedHull = targetObject.Slice(endPoint.position, sliceNormal);

            if (slicedHull == null)
            {
                Destroy(targetObject);
            }
            else
            {
                target.Notify(this);
                StartCoroutine(CreateSlicedObjects(slicedHull, clothColliders, targetObject, target.gameObject));
            }
        }

        private IEnumerator CreateSlicedObjects(SlicedHull slicedHull, ClothColliders clothColliders, GameObject target,
            GameObject original)
        {
            yield return new WaitForFixedUpdate();
            var material = target.GetComponent<Renderer>().material;
            var xsMaterial = Instantiate(material);
            xsMaterial.color = Color.clear;
            var upperHull = slicedHull.CreateUpperHull(target, xsMaterial);
            var lowerHull = slicedHull.CreateLowerHull(target, xsMaterial);

            if (lowerHull && upperHull)
            {
                if (clothColliders != null)
                {
                    ComposeSlicedCloth(upperHull, clothColliders, target.transform, material, true);
                    ComposeSlicedCloth(lowerHull, clothColliders, target.transform, material, false);
                }
                else
                {
                    ComposeSlicedObject(upperHull);
                    ComposeSlicedObject(lowerHull);
                }

                Destroy(target);
                Destroy(original);
            }
            else
            {
                Debug.LogWarning("Failed to create sliced object.");
                Destroy(target);
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
                if (hitInfo.transform.gameObject.TryGetComponent(out Sliceable target))
                {
                    Slice(target);
                }
            }
        }

        private void ComposeSlicedCloth(GameObject hull, ClothColliders clothColliders, Transform tf, Material material,
            bool upper)
        {
            var pos = tf.position;
            float offset = upper ? _tearDistance : -_tearDistance;
            hull.transform.position = new Vector3(pos.x + offset, pos.y, pos.z + offset);

            var skinnedMeshRenderer = hull.AddComponent<SkinnedMeshRenderer>();
            var cloth = hull.AddComponent<Cloth>();
            skinnedMeshRenderer.material = material;
            cloth.useGravity = true;
            cloth.capsuleColliders = clothColliders.CapsuleColliders;
            cloth.sphereColliders = clothColliders.SphereColliders;
            MakeSliceable(hull, 2);
        }

        private void ComposeSlicedObject(GameObject hull)
        {
            var slicedBody = hull.AddComponent<Rigidbody>();
            var meshCollider = hull.AddComponent<MeshCollider>();
            slicedBody.useGravity = true;
            meshCollider.convex = true;
            slicedBody.AddExplosionForce(100f, hull.transform.position, 1f);
            MakeSliceable(hull, 1);
        }

        private void MakeSliceable(GameObject obj, int delayInSeconds)
        {
            StartCoroutine(MakeSliceableCoroutine(obj, delayInSeconds));
        }
        
        private IEnumerator MakeSliceableCoroutine(GameObject obj, int seconds)
        {
            yield return new WaitForSeconds(seconds);
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<BoxCollider>();
            obj.AddComponent<Sliceable>();
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