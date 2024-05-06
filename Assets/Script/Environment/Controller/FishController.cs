using Dreamteck.Splines;
using UnityEngine;

namespace Script.Environment.Controller
{
    [RequireComponent(typeof(ObjectController))]
    public class FishController : MonoBehaviour
    {
        [SerializeField] private float followDuration = 10f;

        [SerializeField] private bool loop = true;

        [SerializeField] private bool pingPong = false;

        private ObjectController _objController;

        private float speed;

        private void Start()
        {
            _objController = GetComponent<ObjectController>();
            speed = 1 / followDuration;
        }

        private void Update()
        {
            UpdateOffset();
        }

        private void UpdateOffset()
        {
            var offset = _objController.evaluateOffset;
            _objController.evaluateOffset += speed * Time.deltaTime;

            if (offset > 1f || offset < 0f)
            {
                if (!loop)
                {
                    speed = 0;
                }
                else if (pingPong)
                {
                    speed *= -1;
                }
                else
                {
                    _objController.evaluateOffset = 0f;
                }
            }
        }
    }
}
