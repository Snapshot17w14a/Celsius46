using UnityEngine;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Player Movement")]
    [SerializeField] private float startingHealth = 100.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float maxRunningSpeed = 10.0f;
    [SerializeField] private float jumpForce = 15.0f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Camera Movement")]
    [SerializeField] private float horizontalTurnSpeed = 25.0f;
    [SerializeField] private float verticalTurnSpeed = 25.0f;
    [SerializeField] private float maxTiltAngle = 10.0f;
    [SerializeField] private float adjustSpeed = 5.0f;

    [Header("Weapon Handling")]
    [SerializeField] private ShootingController shootingController;

    private float health;

    private float smoothedTiltRotation;

    private Transform mainCameraTransform;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CameraUpdate();
    }

    private void CameraUpdate()
    {
        // Set the camera rotation to the correct value range
        Vector3 currentEulerAngles = mainCameraTransform.localEulerAngles;
        if(currentEulerAngles.x > 180) currentEulerAngles.x -= 360;

        // Rotate the camera based on the mouse movement
        Vector3 mouseDelta = new Vector3(-Input.GetAxisRaw("Mouse Y") * verticalTurnSpeed * Time.deltaTime, Input.GetAxisRaw("Mouse X") * horizontalTurnSpeed * Time.deltaTime, 0) + currentEulerAngles;

        // Clamp the camera rotation
        mouseDelta.x = Mathf.Clamp(mouseDelta.x, -90, 90);

        // Tilt the camera based on the horizontal input
        smoothedTiltRotation = Mathf.SmoothStep(smoothedTiltRotation, -Input.GetAxis("Horizontal") * maxTiltAngle, Time.deltaTime * adjustSpeed);
        mouseDelta.z = smoothedTiltRotation;

        // Apply the rotation
        mainCameraTransform.localEulerAngles = mouseDelta;
    }

    private void MovePlayer()
    {
        // Move the player based on the input with force
        //rb.AddForce(Input.GetAxis("Vertical") * speed * transform.forward);
        //rb.AddForce(Input.GetAxis("Horizontal") * speed * transform.right);

        // Move the player based on the input with velocity
        Vector3 movementDelta = mainCameraTransform.forward * Input.GetAxis("Vertical") + mainCameraTransform.right * Input.GetAxis("Horizontal");
        movementDelta *= (Input.GetKey(sprintKey) ? maxRunningSpeed : maxSpeed);

        // Jump
        if(Input.GetKeyDown(jumpKey)) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Clamp the velocity
        //rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));

        // Apply the movement
        rb.velocity = new Vector3(movementDelta.x, rb.velocity.y, movementDelta.z); 
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public void Die()
    {
        Debug.Log("Player died");
    }
}
