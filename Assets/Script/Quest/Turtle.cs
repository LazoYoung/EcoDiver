using System.Collections;
using Script.Interaction;
using UnityEngine;

namespace Script.Quest
{
    [RequireComponent(typeof(Rigidbody))]
    public class Turtle : MonoBehaviour
    {
        [SerializeField] private Sliceable fishingNet;

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            body.freezeRotation = true;
            
            if (fishingNet == null)
            {
                fishingNet = FindObjectOfType<Sliceable>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hand") && fishingNet != null)
            {
                var slicer = FindFirstObjectByType<Slicer>();

                if (slicer)
                {
                    StartCoroutine(SimulateSlice(slicer));
                }
            }
        }

        private IEnumerator SimulateSlice(Slicer slicer)
        {
            yield return new WaitForFixedUpdate();
            slicer.Slice(fishingNet);
        }
    }
}
