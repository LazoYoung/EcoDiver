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
            // Initialize the default depth level to the player's starting Y position
            if (playerTransform != null)
            {
                _defaultDepthLevel = playerTransform.position.y;
            }
            else
            {
                Debug.LogError("Player transform is not assigned to WaterDepthManager.");
            }
        }

        void Update()
        {
            if (playerTransform != null)
            {
                float currentDepth = _defaultDepthLevel - playerTransform.position.y;
                // Check if depth is negative; if so, player is above water level
                if (currentDepth < 0) currentDepth = 0;

                // Check if the depth change is significant enough to log
                if (Mathf.Abs(_lastLoggedDepth - currentDepth) >= DepthLogInterval)
                {
                    _lastLoggedDepth = currentDepth;
                    Debug.Log($"Water depth changed: {currentDepth:F1} meters");
                }
            }
        }
    }
}
