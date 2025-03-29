using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crouchAmount = 0.5f;
    public float jumpForce = 10f;
    private Rigidbody rb;
    public Material crouchingMat;
    public Material normalMat;
    public float fallMultiplier = 5f;
    public float jumpMultiplier = 5f;
    public float jumpingMaxVelocity = 5f;
    public float olliePopForce = 5f;
    public Animator animatorController;
    [FormerlySerializedAs("velocityDescentTime")] public float velocityDescentAmount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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
    
    public void SetIsGrounded()
    {
        // If we are actually crouching when we interact with the ground, set crouch instead
        if (Input.GetKey(KeyCode.Space))
        {
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Crouching);
            return;
        }
        PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Grounded);
    }

    private void FixedUpdate()
    {
        float verticalVelocity = rb.linearVelocity.y;

        if (PlayerProperties.Instance.playerState == PlayerProperties.PlayerState.Grounded)
        {
            animatorController.Play("Skating");
        }
        else if (verticalVelocity > 0)
        {
            // Player is moving upwards (jumping)
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Rising);
            ApplyJumpForce();
            animatorController.Play("in air");
        }
        
        // when grinding we don't want to think we are falling
        else if (verticalVelocity < 0 && PlayerProperties.Instance.playerState != PlayerProperties.PlayerState.Grinding)
        {
            // Player is moving downwards (falling)
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Falling);
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
        if (PlayerProperties.Instance.IsNotAllowedToCrouch()) return;
        PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Crouching);
    }

    // On release
    void Jump()
    {
        // Don't allow double jumps
        if (PlayerProperties.Instance.IsNotAllowedToJump()) return;
        
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