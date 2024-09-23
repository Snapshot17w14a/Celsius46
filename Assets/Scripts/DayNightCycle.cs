using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private float dayDuration = 120f; // 2 minutes for a full day

    void FixedUpdate()
    {
        float rotationSpeed = 360f / dayDuration;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
