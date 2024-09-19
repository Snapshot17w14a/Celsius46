using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPrefabSpawner : MonoBehaviour
{
    [SerializeField] private Building[] buildingPrefabs; // Serialize to allow editing in the Inspector
    [SerializeField] private float planetRadius = 10f; // Serialize for Inspector adjustment
    [SerializeField] private int numberOfPrefabs = 10; // Serialize for Inspector adjustment
    [SerializeField] private float spawnInterval = 1f; // Serialize for Inspector adjustment

    [SerializeField] private bool spawnOnInterval = false;

    private bool isSpawning = true;
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

    public enum BuildingType
    {
        PowerPlant       = 0,
    }

    void Start()
    {
        UpdateSpawnableBuidings();
        if (spawnOnInterval) StartCoroutine(SpawnPrefabsWithInterval());
    }

    private void Update()
    {
        UpdateSpawnableBuidings();
    }

    private IEnumerator SpawnPrefabsWithInterval()
    {
        while (isSpawning)
        {
            //Debug.Log("Spawning prefab " + (++spawnedPrefabs) + " of " + numberOfPrefabs);

            SpawnPrefab((BuildingType)Mathf.RoundToInt(Random.Range(0, spawnableBuildings.Count)));

            yield return new WaitForSeconds(spawnInterval);
        }
        //Debug.Log("Finished spawning all prefabs.");
    }

    public void SpawnRandomPrefab()
    {
       SpawnPrefab((BuildingType)Mathf.RoundToInt(Random.Range(0, spawnableBuildings.Count)));
    }

    public void SpawnPrefab(BuildingType buildingType)
    {
        int prefabIndex = (int)buildingType;
        //Debug.Log("Spawning prefab index: " + prefabIndex);

        Vector3 randomPosition = Random.onUnitSphere * planetRadius;

        Building prefabToSpawn;

        // Check if a specific prefab index was passed
        prefabToSpawn = spawnableBuildings[prefabIndex];
        //Debug.Log("Spawning specific prefab: " + prefabToSpawn + " at position " + randomPosition);

        //else
        //{
        //    // Randomly select a prefab from the array if no valid index is passed
        //    prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
        //    Debug.Log("Spawning random prefab: " + prefabToSpawn.name + " at position " + randomPosition);
        //}

        Building newPrefab = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

        // Align the prefab to face away from the planet's center (0,0,0)
        Vector3 directionFromCenter = (newPrefab.transform.position - Vector3.zero).normalized; // Direction pointing away from the center
        newPrefab.transform.rotation = Quaternion.LookRotation(directionFromCenter); // Set rotation to face away from the center

        newPrefab.transform.parent = transform;
    }

    private void UpdateSpawnableBuidings()
    {
        var population = PopulationSimulator.Instance.GetPopulation;
        foreach (var building in buildingPrefabs)
        {
            var populationValues = building.GetPopulationValues;
            if(!spawnableBuildings.Contains(building) && population >= populationValues.Item1 && population <= populationValues.Item2) spawnableBuildings.Add(building);
            else if (spawnableBuildings.Contains(building) && population <= populationValues.Item1 && population >= populationValues.Item2) spawnableBuildings.Remove(building);
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        //Debug.Log("Spawning stopped.");
    }
}
