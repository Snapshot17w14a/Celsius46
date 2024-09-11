using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]

public class EnemyShooting : MonoBehaviour
{
    public Transform player; // Player's position
    public float minRange = 5f; // Minimum range where hit chance is max
    public float maxRange = 50f; // Maximum range where hit chance is 0%

    void ShootAtPlayer()
    {
        // Get the distance between the enemy and player
        float distance = Vector3.Distance(transform.position, player.position);

        // Calculate hit chance based on distance
        float hitChance = CalculateHitProbability(distance);

        // Random check to determine if shot hits
        if (Random.value <= hitChance)
        {
            Debug.Log("Hit the player!");
        }
        else
        {
            Debug.Log("Missed the shot.");
        }
    }

    float CalculateHitProbability(float distance)
    {
        // Clamp distance between minRange and maxRange
        distance = Mathf.Clamp(distance, minRange, maxRange);

        // Normalize the distance to a 0-1 range (0 = minRange, 1 = maxRange)
        float normalizedDistance = (distance - minRange) / (maxRange - minRange);

        // Invert the normalized distance to get hit probability
        float hitChance = 1f - normalizedDistance;

        return hitChance;
    }
}

