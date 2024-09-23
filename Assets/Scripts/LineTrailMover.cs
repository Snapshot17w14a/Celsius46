using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrailMover : MonoBehaviour
{
    [SerializeField] float velocity = 0.5f;
    [SerializeField] float lifetime = 2.0f;  // Example lifetime
    [SerializeField] TrailRenderer trailRenderer; // Reference to the TrailRenderer

    private float startTime;

    void Start()
    {
        // Record the start time when the object is spawned
        startTime = Time.time;

        // Destroy the object after the specified lifetime
        Destroy(gameObject, lifetime);

        // Ensure the TrailRenderer is assigned
        if (trailRenderer == null)
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }
    }

    void Update()
    {
        // Move the object along the Y axis over time
        transform.Translate(Vector3.up * velocity * Time.deltaTime);

        // Check if the TrailRenderer still exists before trying to fade it
        if (trailRenderer != null)
        {
            // Gradually fade out the trail over the entire lifetime
            FadeOutTrail();
        }
    }

    void FadeOutTrail()
    {
        // Calculate how much time has passed since the object was spawned
        float elapsedTime = Time.time - startTime;

        // Calculate how far along in the lifetime the object is (0 to 1)
        float lifetimeProgress = Mathf.Clamp01(elapsedTime / lifetime);

        // Invert the lifetime progress so it starts full and fades to 0
        float fadeFactor = 1.0f - lifetimeProgress;

        // Get the current start and end colors of the trail
        Color startColor = trailRenderer.startColor;
        Color endColor = trailRenderer.endColor;

        // Set the alpha (opacity) of the colors based on the fade factor
        startColor.a = fadeFactor;
        endColor.a = fadeFactor;

        // Apply the new colors to the TrailRenderer
        trailRenderer.startColor = startColor;
        trailRenderer.endColor = endColor;
    }
}
