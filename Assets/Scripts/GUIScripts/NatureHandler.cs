using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NatureHandler : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;             // Prefab to spawn at the clicked object's location
    [SerializeField] private float prefabLifetime = 10f;           // Lifetime of the prefab before it gets destroyed

    private bool isInNaturePlacementMode = false;  // Toggle state to track nature placement mode

    void Update()
    {
        // Toggle nature placement mode when "2" key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isInNaturePlacementMode = !isInNaturePlacementMode;
            Debug.Log(isInNaturePlacementMode ? "Nature Placement Mode Activated!" : "Nature Placement Mode Deactivated!");
        }

        if (isInNaturePlacementMode && Input.GetMouseButtonDown(0)) // Left mouse button
        {
            PlaceTreeAtClick();
        }
    }

    private void PlaceTreeAtClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;

            if (prefabToSpawn != null)
            {
                // Calculate direction to the origin
                Vector3 directionToCenter = Vector3.zero - spawnPosition;
                Quaternion spawnRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);

                // Instantiate the prefab at the hit point with the calculated rotation
                GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
            }
        }
    }
}
