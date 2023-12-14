using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed = 50f;
    public Transform playerCamera;
    public float groundDrag;
    public float mouseSensitivity = 2.0f;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public KeyCode jumpKey = KeyCode.Space;
    public float playerHeight;
    public LayerMask whatIsGround;

    private bool readyToJump = true;
    private bool grounded;
    private float xRotation;
    private Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        HandleInput();
        HandleDrag();
        HandleRotation();
        MovePlayer();

    }

    private void FixedUpdate()
    {
    }

    private void HandleInput()
    {
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void HandleDrag()
    {
        rb.drag = grounded ? groundDrag : 0;
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get the forward direction of the camera
        // Vector3 forward = playerCamera.transform.forward;
        // // Remove any Y-axis movement
        // forward.y = 0;
        // forward.Normalize();
        Vector3 inputVector = new Vector3(horizontalInput, 0f, verticalInput);

        // Get the right direction of the camera
        // Vector3 right = playerCamera.transform.right;

        // Vector3 velocity = rb.velocity;
        Vector3 force = playerCamera.transform.TransformDirection(inputVector);
        force.y = 0f; // set y component to 0
        force *= moveSpeed * 60 * Time.deltaTime;

        rb.AddForce(force, ForceMode.Acceleration); //* (grounded ? 1 : airMultiplier)

        // This is the desired move direction
        // Vector3 desiredMoveDirection = forward * verticalInput + right * horizontalInput;

        // rb.AddForce(desiredMoveDirection.normalized * moveSpeed * Time.deltaTime * 10f * (grounded ? 1 : airMultiplier), ForceMode.Force);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        // LimitVelocity();
    }

    private void LimitVelocity()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}