using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintModifier;

    [Header("Player Jump Settings")]
    [SerializeField] private int jumpCount;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;

    [Header("Player Camera Settings")]
    [SerializeField] private float cameraSens;
    [SerializeField] private bool invertCamera;
    [SerializeField] private Vector2 cameraClampBounds;

    [Header("Cache")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerCamera;

    // Private Variables
    private float cameraRotX;
    private Vector3 playerVelocity;
    private int jumpsRemaining;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        jumpsRemaining = jumpCount;
    }

    void Update()
    {
        CameraMovement();
        PlayerMovement();
    }

    private void CameraMovement()
    {
        // Get Camera Input
        float mouseX = Input.GetAxis("Mouse X") * cameraSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSens * Time.deltaTime;

        // Clamp Camera and Invert if selected
        if (invertCamera) { mouseY *= -1; }
        cameraRotX -= mouseY;
        cameraRotX = Mathf.Clamp(cameraRotX, cameraClampBounds.x, cameraClampBounds.y);

        // Rotate player and camera
        playerCamera.localRotation = Quaternion.Euler(cameraRotX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMovement()
    {
        // Reset velocity and jumps if grounded
        if (controller.isGrounded)
        {
            jumpsRemaining = jumpCount;
            playerVelocity = Vector3.zero;
        }

        // Get movement vector
        Vector3 moveDir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");

        // Apply sprint modifier
        float speed = moveSpeed;
        if (Input.GetButton("Sprint"))
        {
            speed *= sprintModifier;
        }

        // Move player
        controller.Move(moveDir * speed * Time.deltaTime);

        // Check for jump input
        PlayerJumping();

        // Move player for jumping and update gravity
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    private void PlayerJumping()
    {
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            playerVelocity.y = jumpHeight;
            jumpsRemaining--;
        }
    }
}
