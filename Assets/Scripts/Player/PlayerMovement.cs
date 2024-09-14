using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 public float speed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public int p_DashSpeed = 20; // Speed multiplier during a dash
    public float p_DashDuration = 0.2f; // How long the dash lasts
    public float p_DashCooldown = 1.0f; // Cooldown time between dashes

    public bool isDashing = false;
    private float dashTime;
    private float dashCooldownTime;

    private Vector3 velocity;
    private bool isGrounded;

    private Rigidbody rb;
    public Transform groundCheck;

    public GameObject PlayerWeapon;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation from physics

        // Create a ground check position
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Movement();
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the player
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
