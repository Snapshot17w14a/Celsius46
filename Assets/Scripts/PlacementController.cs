using UnityEngine;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private float indicatorFloat = 0.01f;

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            SetIndicatorPosition(hit.point, hit.normal);
        }
    }

    private void SetIndicatorPosition(Vector3 hitPosition, Vector3 hitNormal)
    {
        transform.position = hitPosition + hitNormal * indicatorFloat;
        transform.forward = hitNormal;
    }
}