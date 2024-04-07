using UnityEngine;

public class QuestDistanceAndDirection : MonoBehaviour
{
    // The player's Transform. This is set in the Start method by finding the player GameObject.
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player GameObject by tag and get its Transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the vector from the quest to the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;

            // The distance to the player is the magnitude of the direction vector
            float distanceToPlayer = directionToPlayer.magnitude;

            // Normalize the direction vector to get the direction alone
            Vector3 normalizedDirection = directionToPlayer.normalized;

            // Log the distance and direction to the Console for debugging
            Debug.Log($"Distance to Player: {distanceToPlayer}");
            Debug.Log($"Direction to Player: {normalizedDirection}");
        }
    }
}
