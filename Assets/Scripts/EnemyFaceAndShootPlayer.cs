using UnityEngine;

public class EnemyShooting : MonoBehaviour, IDamagable
{
    public float minRange = 5f;  // Minimum range where hit chance is max
    public float maxRange = 50f; // Maximum range where hit chance is 0%
    public float rotationSpeed = 5f; // Speed of rotation when turning to face the player

    private Transform player; // Player's transform will be stored here

    void Start()
    {
        // Search for the player with the tag "Player" at the start
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            //Debug.Log("Player found and stored.");
        }
        else
        {
            //Debug.LogError("No object with tag 'Player' found.");
        }
    }

    void Update()
    {
        // Shoot at the player and rotate towards the player if the player was found
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= maxRange)
            {
                RotateTowardsPlayer();
            }

            ShootAtPlayer();
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        //Debug.Log("Enemy rotating to face player.");
    }

    void ShootAtPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Distance to player: " + distance);

        // Calculate hit chance based on distance
        float hitChance = CalculateHitProbability(distance);
        //Debug.Log("Hit chance based on distance: " + hitChance);

        // Random check to determine if shot hits
        if (Random.value <= hitChance)
        {
            //Debug.Log("Shot hit the player!");
        }
        else
        {
            //Debug.Log("Shot missed the player.");
        }
    }

    float CalculateHitProbability(float distance)
    {
        // Clamp the distance between minRange and maxRange
        distance = Mathf.Clamp(distance, minRange, maxRange);
        //Debug.Log("Clamped distance: " + distance);

        // Normalize the distance to a 0-1 range (0 = minRange, 1 = maxRange)
        float normalizedDistance = (distance - minRange) / (maxRange - minRange);
        //Debug.Log("Normalized distance: " + normalizedDistance);

        // Invert the normalized distance to get hit probability
        float hitChance = 1f - normalizedDistance;
        //Debug.Log("Calculated hit chance: " + hitChance);

        return hitChance;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " damage.");
    }
}
