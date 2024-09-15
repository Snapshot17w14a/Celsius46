using System.Collections;
using UnityEngine;

public class PlanetPrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs; // Serialize to allow editing in the Inspector
    [SerializeField] private float planetRadius = 10f; // Serialize for Inspector adjustment
    [SerializeField] private int numberOfPrefabs = 10; // Serialize for Inspector adjustment
    [SerializeField] private float spawnInterval = 1f; // Serialize for Inspector adjustment

    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnPrefabsWithInterval());
    }

    private IEnumerator SpawnPrefabsWithInterval()
    {
        int spawnedPrefabs = 0;

        while (spawnedPrefabs < numberOfPrefabs && isSpawning)
        {
            Debug.Log("Spawning prefab " + (spawnedPrefabs + 1) + " of " + numberOfPrefabs);

            SpawnPrefab();

            spawnedPrefabs++;

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log("Finished spawning all prefabs.");
    }

    public void SpawnPrefab(int? prefabIndex = null)
    {
        Vector3 randomPosition = Random.onUnitSphere * planetRadius;

        GameObject prefabToSpawn;

        // Check if a specific prefab index was passed
        if (prefabIndex.HasValue && prefabIndex >= 0 && prefabIndex < prefabs.Length)
        {
            prefabToSpawn = prefabs[prefabIndex.Value];
            Debug.Log("Spawning specific prefab: " + prefabToSpawn.name + " at position " + randomPosition);
        }
        else
        {
            // Randomly select a prefab from the array if no valid index is passed
            prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
            Debug.Log("Spawning random prefab: " + prefabToSpawn.name + " at position " + randomPosition);
        }

        GameObject newPrefab = Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

        // Align the prefab to face away from the planet's center (0,0,0)
        Vector3 directionFromCenter = (newPrefab.transform.position - Vector3.zero).normalized; // Direction pointing away from the center
        newPrefab.transform.rotation = Quaternion.LookRotation(directionFromCenter); // Set rotation to face away from the center

        newPrefab.transform.parent = this.transform;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("Spawning stopped.");
    }
}
