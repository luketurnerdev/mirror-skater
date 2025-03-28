using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crouchAmount = 0.5f;
    public float jumpForce = 10f;
    public bool isCrouching;
    private Rigidbody rb;
    public Material crouchingMat;
    public Material normalMat;
    public bool isFalling;
    public bool isJumping;
    public float fallMultiplier = 5f;
    public float jumpMultiplier = 5f;
    public float jumpingMaxVelocity = 5f;
    public float olliePopForce = 5f;
    public bool hasOlliePopped;
    [FormerlySerializedAs("velocityDescentTime")] public float velocityDescentAmount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveHorizontally();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);  // Adjust the distance as needed
        float verticalVelocity = rb.linearVelocity.y;

        if (isGrounded)
        {
            // Player is on the ground, reset booleans
            isJumping = false;
            isFalling = false;
        }
        else if (verticalVelocity > 0)
        {
            // Player is moving upwards (jumping)
            isJumping = true;
            isFalling = false;
            ApplyJumpForce();
        }
        else if (verticalVelocity < 0)
        {
            // Player is moving downwards (falling)
            isJumping = false;
            isFalling = true;
            ApplyGravityForce();
        }
    }

    private void ApplyGravityForce()
    {
        Vector3 gravityForce = Vector3.down * fallMultiplier;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }

    private void AddOlliePopForce()
    {
        Vector3 popForce = new Vector3(0, olliePopForce, 0);
        rb.AddForce(popForce, ForceMode.Impulse);
    }
    // Runs in update
    private void ApplyJumpForce()
    {
        Vector3 jumpForce = Vector3.up * jumpMultiplier;
        rb.AddForce(jumpForce, ForceMode.Acceleration);
    }

    // On held
    void Crouch()
    {
        if (isCrouching) return;
        gameObject.GetComponent<MeshRenderer>().material = crouchingMat;

        // Simulate crouching by moving downward
        isCrouching = true;
    }

    // On release
    void Jump()
    {
        // if (isJumping) return;
        // if (!isCrouching) return;
        //
        isCrouching = false;
        gameObject.GetComponent<MeshRenderer>().material = normalMat;
        
        isJumping = true;
        
        // On jump, give an extra boost
        // before adding acceleration
        AddOlliePopForce();
    }

    void MoveHorizontally()
    {
        Vector3 horizontalMovement = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        rb.MovePosition(transform.position + horizontalMovement);
    }
}