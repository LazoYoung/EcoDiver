using System;
using UnityEngine;

namespace Script.Quest
{
    public class QuestArrowIndicator : MonoBehaviour
    {
        public Transform questTransform;

        public void UpdateArrow(Vector3 direction, float distance)
        {
            direction.y = 0; // Ignore the y component of the direction
            // Rotate the arrow to match the direction to the quest
            transform.rotation = Quaternion.LookRotation(direction);

            // Adjust the arrow's width (x scale) based on the distance
            // You might want to clamp the distance to avoid overly wide arrows
            // float clampedDistance = Mathf.Clamp(distance, 1f, 10f); // Example clamping
            // transform.localScale = new Vector3( transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        private void FixedUpdate()
        {
            if (questTransform)
            {
                Vector3 directionToQuest = questTransform.position - transform.position;
                float distanceToQuest = directionToQuest.magnitude;
                Vector3 normalizedDirection = directionToQuest.normalized;

                // Debug.Log($"Distance to Quest: {distanceToQuest}");
                // Debug.Log($"Direction to Quest: {normalizedDirection}");

                UpdateArrow(normalizedDirection, distanceToQuest);
            }
        }
    }
}
