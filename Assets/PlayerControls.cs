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
    public bool isGrounded;
    public Animator animatorController;
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
    
    public void SetIsGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

    private void FixedUpdate()
    {
        Debug.Log("IsGrounded: " + isGrounded);
        float verticalVelocity = rb.linearVelocity.y;

        if (isGrounded)
        {
            // Player is on the ground, reset booleans
            Debug.Log("Player is grounded");
            isJumping = false;
            isFalling = false;
            animatorController.Play("Skating");
        }
        else if (verticalVelocity > 0)
        {
            // Player is moving upwards (jumping)
            Debug.Log("Player is jumping");
            isJumping = true;
            isFalling = false;
            ApplyJumpForce();
            animatorController.Play("in air");
        }
        else if (verticalVelocity < 0)
        {
            // Player is moving downwards (falling)
            Debug.Log("Player is falling");
            isJumping = false;
            isFalling = true;
            ApplyGravityForce();
            animatorController.Play("in air");
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
        // Dont allow double jumps
        if (!isGrounded) return;
        
        isCrouching = false;
        isGrounded = false;
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