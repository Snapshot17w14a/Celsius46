using UnityEngine;
using System.Collections.Generic;

public class HighlightObjects : MonoBehaviour
{
    [SerializeField] private string tagToHighlight = "Highlight";  // Tag of objects to highlight
    [SerializeField] private Material highlightMaterial;           // Material used for highlighting
    [SerializeField] private Material defaultMaterial;             // Default material to apply when resetting
    [SerializeField] private GameObject prefabToSpawn;             // Prefab to spawn at the clicked object's location
    [SerializeField] private float prefabLifetime = 5f;            // Lifetime of the prefab before it gets destroyed

    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>(); // Store original materials to restore later
    private bool isHighlighted = false;  // Toggle state to track whether objects are highlighted or reset  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isHighlighted)
            {
                ResetObjectsMaterial();
            }
            else
            {
                HighlightTaggedObjects();
            }
            isHighlighted = !isHighlighted;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isHighlighted)
            {
                ResetObjectsMaterial();
                isHighlighted = false;  // Ensure that highlight mode is turned off
            }
        }


        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (PopulationSimulator.Instance.AvailableActionPoints == 0) return;
            DetectHighlightedObject();
        }
    }

    // Add a method to check if highlight mode is active
    public bool IsHighlightModeActive()
    {
        return isHighlighted;
    }

    // Add a method to highlight newly spawned objects
    public void HighlightNewObject(GameObject newObject)
    {
        HighlightObject(newObject);
    }

    // Function to highlight objects with the specified tag
    private void HighlightTaggedObjects()
    {
        // Find all objects with the specified tag
        GameObject[] objectsToHighlight = GameObject.FindGameObjectsWithTag(tagToHighlight);

        foreach (GameObject obj in objectsToHighlight)
        {
            HighlightObject(obj); // Use HighlightObject to apply highlight
        }
    }

    // Function to highlight a single object
    private void HighlightObject(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.materials.Length > 1)
            {
                // Store the original materials only if they are not already stored
                if (!originalMaterials.ContainsKey(obj))
                {
                    originalMaterials[obj] = renderer.materials;
                }

                // Replace the second material with the highlight material
                Material[] materials = renderer.materials;
                materials[1] = highlightMaterial;
                renderer.materials = materials;
            }
        }
    }

    // Function to reset the highlighted objects to their original materials
    public void ResetObjectsMaterial()
    {
        GameObject[] objectsToReset = GameObject.FindGameObjectsWithTag(tagToHighlight);

        foreach (GameObject obj in objectsToReset)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                if (renderer.materials.Length > 1)
                {
                    if (originalMaterials.ContainsKey(obj))
                    {
                        renderer.materials = originalMaterials[obj];
                    }
                    else if (defaultMaterial != null)
                    {
                        Material[] materials = renderer.materials;
                        materials[1] = defaultMaterial;
                        renderer.materials = materials;
                    }
                }
            }
        }

        originalMaterials.Clear();
    }

    private void DetectHighlightedObject()
    {
        // Cast a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            GameObject clickedObject = hit.collider.gameObject;

            // Check if the object is tagged
            if (clickedObject.CompareTag(tagToHighlight))
            {
                Renderer renderer = clickedObject.GetComponent<Renderer>();

                if (renderer != null && renderer.materials.Length > 1)
                {
                    if (IsHighlightMaterial(renderer.materials[1]))
                    {
                        if (prefabToSpawn != null)
                        {
                            Vector3 spawnPosition = hit.point;

                            // Calculate direction to the origin
                            Vector3 directionToCenter = Vector3.zero - spawnPosition;
                            Quaternion spawnRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);

                            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);
                            PopulationSimulator.Instance.SubtractActionPoint(spawnedPrefab.GetComponent<Plant>().GetActionCost);

                            // If we are in highlight mode, highlight the newly spawned prefab
                            if (isHighlighted)
                            {
                                // Track and highlight the prefab
                                TrackAndHighlightNewPrefab(spawnedPrefab);
                            }

                            // Start coroutine to handle prefab lifetime
                            Destroy(spawnedPrefab, prefabLifetime);

                            // Destroy the clicked object
                            Destroy(clickedObject.transform.parent.transform.parent.gameObject);
                        }
                    }
                }
            }
        }
    }

    // Function to track and highlight a newly spawned prefab
    private void TrackAndHighlightNewPrefab(GameObject spawnedPrefab)
    {
        // Add the prefab to the dictionary with its current materials for tracking
        Renderer[] renderers = spawnedPrefab.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (!originalMaterials.ContainsKey(spawnedPrefab) && renderer.materials.Length > 1)
            {
                originalMaterials[spawnedPrefab] = renderer.materials;

                // Apply the highlight material immediately
                Material[] materials = renderer.materials;
                materials[1] = highlightMaterial;
                renderer.materials = materials;
            }
        }
    }

    private bool IsHighlightMaterial(Material material)
    {
        if (material == null || highlightMaterial == null)
        {
            return false;
        }

        // Compare properties like color or shader name if needed
        bool sameShader = material.shader == highlightMaterial.shader;

        // Adjust the comparison logic based on your needs
        return sameShader;
    }
}
