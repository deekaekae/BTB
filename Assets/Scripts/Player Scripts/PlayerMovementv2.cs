using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    public float jumpForce = 5f;  // Jump force
    public LayerMask groundLayers;  // Layer mask to specify what is ground
    public float jumpCooldown = 1f;  // Cooldown time between jumps

    private Rigidbody rb;
    private bool isGrounded;  // To check if the player is on the ground
    private bool jumpPressed;  // To check if jump button was pressed
    private float lastJumpTime;  // Time of the last jump
    public Transform groundCheck;  // Empty GameObject to check ground collision
    public float groundCheckRadius = 0.1f;  // Radius of the ground check sphere
    private Animator animator;
    private PlayerCombat playerCombat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the player.");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from the player.");
        }

        playerCombat = GetComponent<PlayerCombat>();
        if (playerCombat == null)
        {
            Debug.LogError("PlayerCombat component missing from the player.");
        }
    }

    void Update()
    {
        // Check if the player is grounded
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers);
            animator.SetBool("isGrounded", isGrounded);
        }

        // Check if jump button is pressed and cooldown has passed
        if (Input.GetButtonDown("Jump") && isGrounded && Time.time > lastJumpTime + jumpCooldown)
        {
            jumpPressed = true;
        }

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Determine movement direction
        bool isMoving = moveHorizontal != 0 || moveVertical != 0;

        // Rotate the player to face the mouse
        RotatePlayerToMouse();

        // Update animator parameters based on the relative direction
        UpdateAnimationParameters(moveHorizontal, moveVertical, isMoving);
    }

    void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        // Check if the player is blocking
        if (playerCombat != null && playerCombat.IsBlocking())
        {
            return;
        }

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Get the forward direction of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;  // Keep movement strictly horizontal
        cameraForward.Normalize();

        // Get the right direction of the camera
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;  // Keep movement strictly horizontal
        cameraRight.Normalize();

        // Calculate movement direction relative to the camera
        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;

        // Apply movement using Rigidbody
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Handle jump
        if (jumpPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPressed = false;
            lastJumpTime = Time.time;  // Record the time of this jump

            // Determine movement direction again for jump animation
            bool isMoving = moveHorizontal != 0 || moveVertical != 0;
            animator.SetTrigger(isMoving ? "JumpWhileMoving" : "JumpFromIdle");
        }
    }

    void RotatePlayerToMouse()
    {
        // Get the mouse position in world space
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 pointToLook = ray.GetPoint(rayLength);
            Vector3 direction = (pointToLook - transform.position).normalized;

            // Rotate the player to face the direction of the mouse
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
    }

    void UpdateAnimationParameters(float moveHorizontal, float moveVertical, bool isMoving)
    {
        // Reset direction parameters
        animator.SetBool("isMovingForward", false);
        animator.SetBool("isMovingBack", false);

        if (!isMoving)
        {
            // If not moving, reset isMoving and return
            animator.SetBool("isMoving", false);
            return;
        }

        // Determine the direction to the mouse
        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 directionToMouse = (mousePosition - transform.position).normalized;
        float angleToMouse = Mathf.Atan2(directionToMouse.z, directionToMouse.x) * Mathf.Rad2Deg;

        // Determine the movement direction relative to the player
        float moveAngle = Mathf.Atan2(moveVertical, moveHorizontal) * Mathf.Rad2Deg;

        // Calculate the relative angle
        float relativeAngle = Mathf.DeltaAngle(angleToMouse, moveAngle);

        // Set the appropriate animation parameter based on the relative angle
        if (relativeAngle > -90 && relativeAngle <= 90)
        {
            animator.SetBool("isMovingForward", true);
        }
        else
        {
            animator.SetBool("isMovingBack", true);
        }

        // Update the isMoving parameter
        animator.SetBool("isMoving", isMoving);
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(ray, out rayLength))
        {
            return ray.GetPoint(rayLength);
        }

        return Vector3.zero;
    }
}
