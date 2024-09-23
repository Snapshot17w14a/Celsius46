using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPrefabSpawner : MonoBehaviour
{
    [SerializeField] private Building[] landBuildings;  // Array for land-based buildings
    // [SerializeField] private Building[] waterBuildings; // Array for water-based buildings
    [SerializeField] private float planetRadius = 10f;

    [SerializeField] private Texture2D planetTexture;  // Texture for detecting land/water
    [SerializeField] private Mesh planetMesh;  // Mesh to sample UV coordinates

    [Header("Colors for the placement limiter")]
    [SerializeField] private Color landColor;
    [SerializeField] private Color sandColor;
    [SerializeField] private Color snowColor;
    [SerializeField] private Color waterColor;

    private readonly List<Building> spawnableBuildings = new();

    private static PlanetPrefabSpawner instance;
    public static PlanetPrefabSpawner Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<PlanetPrefabSpawner>();
            return instance;
        }
    }

    private enum BuildingLocation
    {
        Land,
        Water,
    }

    public enum BuildingType
    {
        PowerPlant = 0, // Example for land building
    }

    public enum WaterBuildingType
    {
        WaterPowerPlant = 0, // Example for water building
    }

    private void Start()
    {
        UpdateSpawnableBuildings();
        SpawnRandomPrefab(); // For testing; in the final version, this may be triggered externally
    }

    private void UpdateSpawnableBuildings()
    {
        var population = PopulationSimulator.Instance.GetPopulation;
        foreach (var building in landBuildings) CheckBuildingValues(building, population);
        // foreach (var building in waterBuildings) CheckBuildingValues(building, population);
    }

    private void CheckBuildingValues(Building building, int population)
    {
        var populationValues = building.GetPopulationValues;
        if (!spawnableBuildings.Contains(building) && population >= populationValues.Item1 && population <= populationValues.Item2)
            spawnableBuildings.Add(building);
        else if (spawnableBuildings.Contains(building) && (population < populationValues.Item1 || population > populationValues.Item2))
            spawnableBuildings.Remove(building);
    }

    public void SpawnRandomPrefab()
    {
        if (spawnableBuildings.Count > 0)
        {
            SpawnPrefab((BuildingType)Mathf.RoundToInt(Random.Range(0, spawnableBuildings.Count)));
        }
    }

    // This method will now be called externally from a different script
    public void SpawnPrefab(BuildingType buildingType)
    {
    // Generate a random point on a sphere to cast the ray from
    Vector3 randomPoint = Random.onUnitSphere * planetRadius;
    Vector3 directionToCenter = -randomPoint.normalized;

    // Cast a ray from the random point towards the planet's center
    if (Physics.Raycast(randomPoint, directionToCenter, out RaycastHit hit, planetRadius * 2f))
    {
        // Sample the texture at the hit point to determine land or water
        Vector2 uv = GetUVFromHit(planetMesh, hit.triangleIndex, hit);
        
        // If UV is invalid, restart the process
        if (uv == Vector2.negativeInfinity)
        {
            Debug.Log("Invalid UV detected. Restarting process");
            SpawnPrefab(buildingType);
            return;
        }

        Color pixelColor = planetTexture.GetPixelBilinear(uv.x, uv.y);

        // Check for collision with nature or highlight objects
        if (IsCollidingWithTaggedObjects(hit.point))
        {
            Debug.Log("Hit a nature or highlight object. Restarting process");
            SpawnPrefab(buildingType);
            return;
        }

        // Determine if it's land or water and spawn the appropriate building
        if (CanPlaceBuilding(pixelColor, out BuildingLocation buildingLocation))
        {
            if (buildingLocation == BuildingLocation.Land)
            {
                // Spawn a building at the hit point
                SpawnBuildingAtPosition(hit.point, buildingLocation);
            }
            else
            {
                Debug.Log("Hit water. Restarting process");
                SpawnPrefab(buildingType);
            }
        }
        else
        {
            Debug.Log("Undetermined location. Restarting process");
            SpawnPrefab(buildingType);
        }
    }
}


    private void SpawnBuildingAtPosition(Vector3 position, BuildingLocation location)
    {
        Building buildingPrefab;

        // Choose a building prefab from the appropriate array based on the location (land or water)
        if (location == BuildingLocation.Land)
        {
            // Choose a random land building from the array
            buildingPrefab = landBuildings[Random.Range(0, landBuildings.Length)];
            Building newBuilding = Instantiate(buildingPrefab, position, Quaternion.LookRotation((position - Vector3.zero).normalized), transform);

            // Highlight the newly spawned building if the highlight system is active
            HighlightObjects highlightSystem = FindObjectOfType<HighlightObjects>();
            if (highlightSystem != null && highlightSystem.IsHighlightModeActive())
            {
                highlightSystem.HighlightNewObject(newBuilding.gameObject);
            }
        }
        // else // God ignore this comment gore
        {
            // In case water-based buildings are enabled later
            // buildingPrefab = waterBuildings[Random.Range(0, waterBuildings.Length)];
        }
    }

    /// <summary>
    /// Check if there's any object with "Nature" or "Highlight" tag within a certain radius of the position.
    /// </summary>
    private bool IsCollidingWithTaggedObjects(Vector3 position)
    {
        float checkRadius = 1.0f; // Adjust this radius as needed
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Nature") || collider.CompareTag("Highlight"))
            {
                return true; // There's a collision with a tagged object
            }
        }

        return false; // No collisions detected
    }

    /// <summary>
    /// Determines whether the hit point is on land or water based on the texture color.
    /// </summary>
    private bool CanPlaceBuilding(Color pixelColor, out BuildingLocation location)
    {
        bool isLand = CompareColors(pixelColor, landColor) || CompareColors(pixelColor, snowColor) || CompareColors(pixelColor, sandColor);
        bool isWater = CompareColors(pixelColor, waterColor);

        if (isLand)
        {
            location = BuildingLocation.Land;
            return true;
        }
        else if (isWater)
        {
            location = BuildingLocation.Water;
            return true;
        }

        location = BuildingLocation.Land; // Default to land if undetermined
        return false;
    }

    /// <summary>
    /// Compares two colors with a given tolerance.
    /// </summary>
    private bool CompareColors(Color a, Color b, float tolerance = 0.05f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }

    /// <summary>
    /// Gets UV coordinates from the hit point on the mesh.
    /// </summary>
    /// <summary>
    /// Gets UV coordinates from the hit point on the mesh.
    /// </summary>
    private Vector2 GetUVFromHit(Mesh mesh, int triangleIndex, RaycastHit hit)
    {
        // Validate that the mesh has UVs and enough triangles
        if (mesh.uv.Length == 0)
        {
            Debug.LogError("Mesh does not have UVs. Restarting process...");
            return Vector2.negativeInfinity;
        }

        int triangleArrayLength = mesh.triangles.Length / 3;  // Total triangles
        if (triangleIndex < 0 || triangleIndex >= triangleArrayLength)
        {
            Debug.LogError("Triangle index is out of bounds. Restarting process...");
            return Vector2.negativeInfinity;
        }

        int i1 = mesh.triangles[triangleIndex * 3];
        int i2 = mesh.triangles[triangleIndex * 3 + 1];
        int i3 = mesh.triangles[triangleIndex * 3 + 2];

        if (i1 >= mesh.uv.Length || i2 >= mesh.uv.Length || i3 >= mesh.uv.Length)
        {
            Debug.LogError("Triangle vertices are out of UV array bounds. Restarting process...");
            return Vector2.negativeInfinity;
        }

        Vector2 uv1 = mesh.uv[i1];
        Vector2 uv2 = mesh.uv[i2];
        Vector2 uv3 = mesh.uv[i3];

        // Use barycentric coordinates to calculate the UV coordinate at the hit point
        return uv1 * hit.barycentricCoordinate.x + uv2 * hit.barycentricCoordinate.y + uv3 * hit.barycentricCoordinate.z;
    }
}
