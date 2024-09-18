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

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            DetectHighlightedObject();
        }
    }

    // Function to highlight objects with the specified tag
    private void HighlightTaggedObjects()
    {
        // Find all objects with the specified tag
        GameObject[] objectsToHighlight = GameObject.FindGameObjectsWithTag(tagToHighlight);

        foreach (GameObject obj in objectsToHighlight)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                if (renderer.materials.Length > 1)
                {
                    if (!originalMaterials.ContainsKey(obj))
                    {
                        originalMaterials[obj] = renderer.materials;
                    }

                    Material[] materials = renderer.materials;
                    materials[1] = highlightMaterial;
                    renderer.materials = materials;
                }
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

                            // Start coroutine to handle prefab lifetime
                            Destroy(spawnedPrefab, prefabLifetime);

                            Destroy(clickedObject.transform.parent.transform.parent.gameObject);
                        }
                    }
                }
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
        //bool sameColor = material.color == highlightMaterial.color;
        bool sameShader = material.shader == highlightMaterial.shader;

        // Adjust the comparison logic based on your needs
        return sameShader;
    }

    // Coroutine to destroy prefab after a certain time
    //private IEnumerator<WaitForSeconds> DestroyAfterTime(GameObject obj, float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    Destroy(obj);
    //}
}
