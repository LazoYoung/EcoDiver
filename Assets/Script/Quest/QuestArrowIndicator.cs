using UnityEngine;

public class FootprintArrowIndicator : MonoBehaviour
{
    public void UpdateArrow(Vector3 direction, float distance)
    {
        direction.y = 0; // Ignore the y component of the direction
        // Rotate the arrow to match the direction to the quest
        transform.rotation = Quaternion.LookRotation(-direction);

        // Adjust the arrow's width (x scale) based on the distance
        // You might want to clamp the distance to avoid overly wide arrows
        // float clampedDistance = Mathf.Clamp(distance, 1f, 10f); // Example clamping
        // transform.localScale = new Vector3( transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
