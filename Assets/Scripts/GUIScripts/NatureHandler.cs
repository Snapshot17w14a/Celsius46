using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NatureHandler : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;  // Prefab to spawn at the clicked object's location
    [SerializeField] private float minDistance = 2.0f;  // Minimum distance between placed objects
    [SerializeField] private float collisionRadius = 2.0f;  // Radius to check for collisions (make sure it's large enough)

    private bool isInNaturePlacementMode = false;  // Toggle state to track nature placement mode
    private bool isDragging = false;  // Tracks if the mouse button is held down
    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;  // Stores the position of the last spawned object

    void Update()
    {
        // Toggle nature placement mode when "2" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isInNaturePlacementMode = !isInNaturePlacementMode;
            Debug.Log(isInNaturePlacementMode ? "Nature Placement Mode Activated!" : "Nature Placement Mode Deactivated!");
        }

        // Start placing objects on mouse drag
        if (isInNaturePlacementMode && Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            PlaceTreeAtClick();
        }

        // Continue placing objects while dragging
        if (isDragging && Input.GetMouseButton(0))
        {
            PlaceTreeAtClick();
        }

        // Stop placing objects when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            lastSpawnPosition = Vector3.positiveInfinity;
        }
    }

    private void PlaceTreeAtClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;

            if (prefabToSpawn != null
                && Vector3.Distance(lastSpawnPosition, spawnPosition) > minDistance
                && !IsPositionOccupied(spawnPosition))
            {
                Vector3 directionToCenter = Vector3.zero - spawnPosition;
                Quaternion spawnRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);

                GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, spawnRotation, transform);

                lastSpawnPosition = spawnPosition;
            }
        }
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, collisionRadius);

        //Debug.Log("Colliders detected: " + colliders.Length);

        foreach (Collider collider in colliders)
        {
            //Debug.Log("Detected object: " + collider.gameObject.name + ", Tag: " + collider.gameObject.tag);

            if (collider.CompareTag("Highlight") || collider.CompareTag("Nature"))
            {
                Debug.Log("Collision detected with object tagged as 'Highlighted' or 'Nature'. Cannot place tree here.");
                return true;
            }
        }

        return false;
    }
}