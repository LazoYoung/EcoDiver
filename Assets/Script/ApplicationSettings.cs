using UnityEngine;

namespace Script
{
    public class ApplicationSettings : MonoBehaviour
    {
        public int maxFrameRate = 50;

        private void Start()
        {
            Application.targetFrameRate = maxFrameRate;
            Time.fixedDeltaTime = 1f / maxFrameRate;
            Debug.Log("Framerate set: " + maxFrameRate);
        }
    }
}
