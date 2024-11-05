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
    private float startingDrag;
    public float maxMagnitude;

    public int p_DashSpeed = 20; // Speed multiplier during a dash
    public float p_DashDuration = 0.2f; // How long the dash lasts
    public float p_DashCooldown = 1.0f; // Cooldown time between dashes

    public bool isDashing = false;
    private float dashTime;
    private float dashCooldownTime;

    private Vector3 velocity;
    public bool isGrounded;

    private Rigidbody rb;
    public Transform groundCheck;

    public GameObject PlayerWeapon;

    private AudioSource audioSource;
    public AudioClip[] walkSounds;
    private float walkAudioCooldown = 0.4f;
    private float walkAudioCurrent;

    public bool canFly = false;
    public bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation from physics
        // Create a ground check position
        startingDrag = rb.drag;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isGrounded == true)
        {
            rb.drag = startingDrag;
        }
        else
        {
            rb.drag = startingDrag / 2;
        }

        if (canFly == false)
        {
            // Check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            Movement();
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small downward force to keep grounded
            }
        }
        else
        {
            Flying();
        }

        walkAudioCurrent += Time.deltaTime;
        if (walkAudioCurrent >= walkAudioCooldown)
        {
            walkAudioCurrent = walkAudioCooldown;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxMagnitude)
        {
            rb.velocity *= 0.75f;
        }
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the player
        // rb.MovePosition(rb.position + move * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward.normalized, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right.normalized * -1f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.forward.normalized * -1f, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right.normalized, ForceMode.Impulse);
        }

        if (isGrounded == true && walkAudioCurrent == walkAudioCooldown)
        {
            if (move.x > 0 || move.z > 0)
            {
                walkAudioCurrent = 0;
                PlayWalkSound();
            }
        }    

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.drag = startingDrag / 2;
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void Flying()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move the player
        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    void PlayWalkSound()
    {
        AudioClip walkSound = walkSounds[Random.Range(0, walkSounds.Length)];
        audioSource.pitch = (Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(walkSound);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
