using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    [SerializeField] private float minDistance = -20f;
    [SerializeField] private float maxDistance = -40f;

    [SerializeField] private GameObject planetCore;

    [Range(0, 1)]
    private float zoomValue = 0f;

    private void MoveCamera()
    {
        Vector3 newRotation = planetCore.transform.rotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        planetCore.transform.eulerAngles = newRotation;
    }

    private void ZoomCamera()
    {
        Vector3 newPosition = transform.localPosition += new Vector3(0, 0, Input.mouseScrollDelta.y);
        newPosition.z = Mathf.Clamp(newPosition.z, maxDistance, minDistance);
        transform.localPosition = newPosition;
    }

    private void Update()
    {
        if(Input.GetMouseButton(1)) MoveCamera();
        ZoomCamera();
    }
}