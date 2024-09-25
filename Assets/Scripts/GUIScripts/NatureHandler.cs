using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NatureHandler : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawnOnWater;  // Prefab to spawn at the clicked object's location
    [SerializeField] private GameObject prefabToSpawnOnLand;  // Prefab to spawn at the clicked object's location
    [SerializeField] private GameObject prefabToSpawnSunflower; // Prefab to spawn at the clicked object's location
    [SerializeField] private float minDistance = 2.0f;  // Minimum distance between placed objects
    [SerializeField] private float collisionRadius = 2.0f;  // Radius to check for collisions (make sure it's large enough)
    [SerializeField] private Texture2D planetTexture;  // Texture to sample for nature placement
    [SerializeField] private Mesh planetMesh;  // Mesh to sample for UV coordinates

    [SerializeField] ModeSwitch ModeHandler;

    [Header("Colors for the placement limiter")]
    [SerializeField] private Color landColor;
    [SerializeField] private Color sandColor;
    [SerializeField] private Color snowColor;
    [SerializeField] private Color waterColor;

    private int seaweedActionCost;
    private int treeActionCost;

    private bool sunFlowerMode = false;
    private bool isInNaturePlacementMode = false;  // Toggle state to track nature placement mode
    private bool isDragging = false;  // Tracks if the mouse button is held down
    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;  // Stores the position of the last spawned object

    private enum PlantType
    {
        Tree,
        Seaweed,
        Sunflower
    }

    private void Start()
    {
        seaweedActionCost = prefabToSpawnOnWater.GetComponent<Plant>().GetActionCost;
        treeActionCost = prefabToSpawnOnLand.GetComponent<Plant>().GetActionCost;
    }

    void Update()
    {
        // Toggle nature placement mode when "2" key is pressed
        if (ModeHandler.currentMode == 0)
        {
            isInNaturePlacementMode = !isInNaturePlacementMode;
            isDragging = false;  // Stop dragging when nature placement mode is toggled
            Debug.Log(isInNaturePlacementMode ? "Nature Placement Mode Activated!" : "Nature Placement Mode Deactivated!");
        }

        if (ModeHandler.currentMode == 1)
        {
            isInNaturePlacementMode = false;
            isDragging = false;  // Stop dragging when deactivating the mode
            Debug.Log("Nature Placement Mode Deactivated!");
        }

        // Start placing objects on mouse drag
        if (isInNaturePlacementMode && Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            PlaceAtClick();
        }

        // Continue placing objects while dragging
        if (isDragging && Input.GetMouseButton(0) && isInNaturePlacementMode)  // Ensure placement mode is still active
        {
            PlaceAtClick();
        }

        // Stop placing objects when the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            lastSpawnPosition = Vector3.positiveInfinity;
        }

        //if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q))
        //{
        //    sunFlowerMode = !sunFlowerMode;
        //}
    }


    private void PlaceAtClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Planet")))
        {
            Vector3 spawnPosition = hit.point;
            Vector2 uv;

            int triangleIndex = hit.triangleIndex;
            
            // Get UV coordinates from the triangle hit
            uv = GetUVFromHit(planetMesh, triangleIndex, hit);

            Color pixelColor = planetTexture.GetPixelBilinear(uv.x, uv.y);

            if (prefabToSpawnOnWater != null
                && Vector3.Distance(lastSpawnPosition, spawnPosition) > minDistance
                && !IsPositionOccupied(spawnPosition) && CanPlacePlant(pixelColor, PlantType.Seaweed))
            {
                Vector3 directionToCenter = Vector3.zero - spawnPosition;
                Quaternion spawnRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);

                if(PopulationSimulator.Instance.AvailableActionPoints >= seaweedActionCost)
                {
                    PopulationSimulator.Instance.SubtractActionPoint(Instantiate(prefabToSpawnOnWater, spawnPosition, spawnRotation, transform).GetComponent<Plant>().GetActionCost);
                    lastSpawnPosition = spawnPosition;
                }
            }
            else if (prefabToSpawnOnLand != null
                && Vector3.Distance(lastSpawnPosition, spawnPosition) > minDistance
                && !IsPositionOccupied(spawnPosition) && (CanPlacePlant(pixelColor, PlantType.Tree) || CanPlacePlant(pixelColor, PlantType.Sunflower)))
            {
                Vector3 directionToCenter = Vector3.zero - spawnPosition;
                Quaternion spawnRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);

                if (PopulationSimulator.Instance.AvailableActionPoints >= treeActionCost)
                {
                    PopulationSimulator.Instance.SubtractActionPoint(Instantiate(prefabToSpawnOnLand, spawnPosition, spawnRotation, transform).GetComponent<Plant>().GetActionCost); 
                    lastSpawnPosition = spawnPosition;
                }
                
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

    Vector2 GetUVFromHit(Mesh mesh, int triangleIndex, RaycastHit hit)
    {
        Debug.Log(triangleIndex + " " + mesh.uv[mesh.triangles[triangleIndex * 3]]);

        Vector2 uv1 = mesh.uv[mesh.triangles[triangleIndex * 3]];
        Vector2 uv2 = mesh.uv[mesh.triangles[triangleIndex * 3 + 1]];
        Vector2 uv3 = mesh.uv[mesh.triangles[triangleIndex * 3 + 2]];

        return uv1 * hit.barycentricCoordinate.x + uv2 * hit.barycentricCoordinate.y + uv3 * hit.barycentricCoordinate.z;
    }

    bool CanPlacePlant(Color pixelColor, PlantType plantType)
    {
        bool isLand = CompareColors(pixelColor, landColor) || CompareColors(pixelColor, snowColor) || CompareColors(pixelColor, sandColor);
        bool isWater = CompareColors(pixelColor, waterColor);
        Debug.Log($"land {isLand}, water {isWater}.");

        return plantType switch
        {
            PlantType.Tree => isLand,
            PlantType.Seaweed => isWater,
            PlantType.Sunflower => isLand,
            _ => false,
        };
    }

    bool CompareColors(Color a, Color b, float tolerance = 0.05f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}