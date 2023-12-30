using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 3.0f;
    public float sprintSpeed = 6.0f; // Add this line
    public float jumpSpeed = 8.0f;
    public float gravity = 10.0f;
    public float mouseSensitivity = 100.0f;
    public Transform playerCamera;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private float yRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX); // Rotate the parent object

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed; // Modify this line

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }
}