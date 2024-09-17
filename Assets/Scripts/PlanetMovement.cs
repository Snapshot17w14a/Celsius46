using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    [SerializeField] private float minDistance = -20f;
    [SerializeField] private float maxDistance = -40f;

    [SerializeField] private float zoomSpeed = 1f;

    private float targetZPos = -10;

    [SerializeField] private GameObject planetCore;

    private void MoveCamera()
    {
        Vector3 newRotation = planetCore.transform.rotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        planetCore.transform.eulerAngles = newRotation;
    }

    private void ZoomCamera()
    {
        targetZPos = Mathf.Clamp(targetZPos + Input.mouseScrollDelta.y, maxDistance, minDistance);
        Vector3 newPosition = transform.localPosition;
        newPosition.z = Mathf.SmoothStep(newPosition.z, targetZPos, Time.deltaTime * zoomSpeed);
        transform.localPosition = newPosition;
    }

    private void Update()
    {
        if(Input.GetMouseButton(1)) MoveCamera();
        ZoomCamera();
    }
}