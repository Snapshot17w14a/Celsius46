using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    Vector2 rotation = Vector2.zero;
    public float speed = 3;

    void Update()
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        cameraTransform.eulerAngles = (Vector2)rotation * speed;
    }
}
