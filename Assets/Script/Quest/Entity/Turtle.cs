using Script.Interaction;
using UnityEngine;

namespace Script.Quest.Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class Turtle : MonoBehaviour
    {
        public Sliceable fishingNet;
        public AnimationClip idleMotion;
        public AnimationClip freeMotion;
        private Animation _animation;
        private string _clip;
        private readonly string _idle = "idle";
        private readonly string _free = "free";

        private void Start()
        {
            var body = GetComponent<Rigidbody>();
            body.freezeRotation = true;
            _clip = _idle;
            _animation = gameObject.AddComponent<Animation>();
            
            if (fishingNet == null)
            {
                fishingNet = FindObjectOfType<Sliceable>();
            }

            if (fishingNet != null)
            {
                fishingNet.onSlice.AddListener(OnFree);
            }

            idleMotion.legacy = true;
            freeMotion.legacy = true;
            _animation.AddClip(idleMotion, _idle);
            _animation.AddClip(freeMotion, _free);
        }

        private void Update()
        {
            if (_animation)
            {
                _animation.Play(_clip);
            }
        }

        private void OnFree(Slicer slicer)
        {
            if (_animation)
            {
                _animation.Stop();
                _clip = _free;
            }
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag("Hand") && fishingNet != null)
        //     {
        //         var slicer = FindFirstObjectByType<Slicer>();
        //
        //         if (slicer)
        //         {
        //             StartCoroutine(SimulateSlice(slicer));
        //         }
        //     }
        // }
        //
        // private IEnumerator SimulateSlice(Slicer slicer)
        // {
        //     yield return new WaitForFixedUpdate();
        //     slicer.Slice(fishingNet);
        // }
    }
}
