using Crest;
using UnityEngine;

namespace Script.Display
{
    public class WaterDepthManager : MonoBehaviour
    {
        [SerializeField] [Tooltip("Assign the player's transform for default depth size.")]
        private Transform playerTransform; // Assign the player's transform in the inspector

        private float _defaultDepthLevel; // This will be set to the player's starting Y position
        private float _lastLoggedDepth = 0.0f; // To track when to log depth changes

        private const float DepthLogInterval = 0.1f; // Log every 0.1m change

        void Start()
        {
            _defaultDepthLevel = 0;
            // // Initialize the default depth level to the player's starting Y position
            // if (playerTransform != null)
            // {
            //     playerTransform.position.y;
            // }
            // else
            // {
            //     Debug.LogError("Player transform is not assigned to WaterDepthManager.");
            // }
        }

        void Update()
        {
            //TODO #46 바닷물 조절
            if (OceanRenderer.Instance != null)
            {
                UpdateWaterDepth();
            }
        }

        private void UpdateWaterDepth()
        {
            float currentDepth = -OceanRenderer.Instance.ViewerHeightAboveWater;
            // Check if depth is negative; if so, player is above water level
            if (currentDepth < 0) currentDepth = 0;

            // Check if the depth change is significant enough to log
            if (Mathf.Abs(_lastLoggedDepth - currentDepth) >= DepthLogInterval)
            {
                _lastLoggedDepth = currentDepth;

                DisplayManager.Instance.WaterDepth = _lastLoggedDepth;
            }
        }
    }
}
