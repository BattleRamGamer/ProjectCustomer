using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;  // Speed of movement
    public float groundDrag; // Drag applied when grounded

    [Header("Ground Check")]
    public float playerHeight;  // Height of the player collider
    public LayerMask whatIsGround;  // Layer mask to define what is considered ground
    bool grounded;  // Flag indicating if the player is grounded

    [Header("Orientation")]
    public Transform orientation;  // Transform used for orientation (typically the player's body)

    float horizontalInput;  // Horizontal input (-1, 0, 1)
    float verticalInput;    // Vertical input (-1, 0, 1)

    Vector3 moveDirection;  // Calculated movement direction

    Rigidbody rb;  // Reference to the Rigidbody component

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component attached to this GameObject
        rb.freezeRotation = true;  // Freeze rotation to prevent physics affecting the orientation
    }

    private void Update()
    {
        // Ground Check using a Raycast
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();      // Read player input
        SpeedControl(); // Control player movement speed

        // Handle Drag based on grounded state
        if (grounded)
        {
            rb.drag = groundDrag;  // Apply ground drag if grounded
        }
        else
        {
            rb.drag = 0;  // No drag if not grounded
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();  // Move the player based on calculated input
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");  // Get horizontal input (left/right keys or A/D keys)
        verticalInput = Input.GetAxisRaw("Vertical");      // Get vertical input (up/down keys or W/S keys)
    }

    private void MovePlayer()
    {
        // Calculate movement direction based on orientation and input
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Apply force to the Rigidbody in the calculated direction
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;  // Return current velocity of the Rigidbody
    }

    public bool IsGrounded()
    {
        return grounded;  // Return whether the player is grounded
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);  // Remove vertical component from velocity

        // Limiting velocity to moveSpeed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;  // Limit velocity magnitude
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);  // Apply limited velocity
        }
    }
}
