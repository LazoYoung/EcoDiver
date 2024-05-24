using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Crest;
using UnityEngine;

namespace Script.Interaction
{
    public class Sinkable : MonoBehaviour
    {
        [UnityEngine.Range(0.5f, 1f)]
        public float buoyancy = 0.98f;
        private SampleHeightHelper _sampler;
        private Rigidbody _rigidbody;
        private bool _sink;

        private void Start()
        {
            if (OceanRenderer.Instance == null)
            {
                enabled = false;
                return;
            }
            
            _sampler = new SampleHeightHelper();
            SetupRigidbody();
            StartCoroutine(TrackState());
        }

        private void SetupRigidbody()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            _rigidbody.useGravity = true;
        }

        [SuppressMessage("ReSharper", "IteratorNeverReturns")]
        private IEnumerator TrackState()
        {
            while (true)
            {
                _sink = IsUnderwater();
                yield return new WaitForSeconds(2f);
            }
        }

        private void FixedUpdate()
        {
            if (_sink)
            {
                _rigidbody.AddForce(-buoyancy * Physics.gravity, ForceMode.Acceleration);
            }
        }

        private bool IsUnderwater()
        {
            _sampler.Init(transform.position);
            _sampler.Sample(out float waterHeight);
            return transform.position.y < waterHeight;
        }
    }
}