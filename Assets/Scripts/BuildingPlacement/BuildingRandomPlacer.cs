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
    [SerializeField] private Color waterColor;
    [SerializeField] private Color snowColor;

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
        UpdateSpawnableBuidings();
        SpawnRandomPrefab();
    }

    private void UpdateSpawnableBuidings()
    {
        var population = PopulationSimulator.Instance.GetPopulation;
        foreach (var building in landBuildings) CheckBuildingValues(building, population);
        // foreach (var building in waterBuildings) CheckBuildingValues(building, population);
    }

    private void CheckBuildingValues(Building building, int population)
    {
        var populationValues = building.GetPopulationValues;
        if (!spawnableBuildings.Contains(building) && population >= populationValues.Item1 && population <= populationValues.Item2) spawnableBuildings.Add(building);
        else if (spawnableBuildings.Contains(building) && population <= populationValues.Item1 && population >= populationValues.Item2) spawnableBuildings.Remove(building);
    }

    public void SpawnRandomPrefab()
    {
        SpawnPrefab((BuildingType)Mathf.RoundToInt(Random.Range(0, spawnableBuildings.Count)));
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
            Color pixelColor = planetTexture.GetPixelBilinear(uv.x, uv.y);

            // Determine if it's land or water and spawn the appropriate building
            if (CanPlaceBuilding(pixelColor, out BuildingLocation buildingLocation))
            {
                // Spawn a building at the hit point based on the location (land or water)
                SpawnBuildingAtPosition(hit.point, buildingLocation);
            }
        }
    }

    private void SpawnBuildingAtPosition(Vector3 position, BuildingLocation location, int retryCount = 5)
    {
        // Check for collision with objects tagged as "Nature" or "Highlight"
        if (IsCollidingWithTaggedObjects(position))
        {
            if (retryCount > 0)
            {
                // Try again with a new random position
                SpawnRandomPrefab(); // Retry spawning at a new location
            }
            return; // Exit if retries are exhausted or we can't place it
        }

        Building buildingPrefab;

        // Choose a building prefab from the appropriate array based on the location (land or water)
        if (location == BuildingLocation.Land)
        {
            // Choose a random land building from the array
            buildingPrefab = landBuildings[Random.Range(0, landBuildings.Length)];
            Building newBuilding = Instantiate(buildingPrefab, position, Quaternion.LookRotation((position - Vector3.zero).normalized), transform);

            HighlightObjects highlightSystem = FindObjectOfType<HighlightObjects>();
            if (highlightSystem != null && highlightSystem.IsHighlightModeActive())
            {
                highlightSystem.HighlightNewObject(newBuilding.gameObject);
            }
        }
        // else // God ignore this comment gore
        {
            // Choose a random water building from the array
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
        bool isLand = CompareColors(pixelColor, landColor) || CompareColors(pixelColor, snowColor);
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
    private Vector2 GetUVFromHit(Mesh mesh, int triangleIndex, RaycastHit hit)
    {
        Vector2 uv1 = mesh.uv[mesh.triangles[triangleIndex * 3]];
        Vector2 uv2 = mesh.uv[mesh.triangles[triangleIndex * 3 + 1]];
        Vector2 uv3 = mesh.uv[mesh.triangles[triangleIndex * 3 + 2]];

        return uv1 * hit.barycentricCoordinate.x + uv2 * hit.barycentricCoordinate.y + uv3 * hit.barycentricCoordinate.z;
    }
}
