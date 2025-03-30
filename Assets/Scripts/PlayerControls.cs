using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 30f;
    private Rigidbody rb;
    
    // Jump physics
    public float fallMultiplier = 5f;
    public float jumpMultiplier = 5f;
    public float olliePopForce = 5f;
    
    // Animations
    public Animator animatorController;

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
        
        if (verticalVelocity > 0)
        {
            // Player is moving upwards (jumping)
            ApplyRisingForce();
        }
        
        else if (verticalVelocity < 0 )
        {
            // Player is moving downwards (falling)
            ApplyGravityForce();
        }
        
        // Now handle state-changing logic
        UpdatePlayerState(verticalVelocity);
    }

    private void UpdatePlayerState(float verticalVelocity)
    {
        var playerProps = PlayerProperties.Instance;
        var currentState = playerProps.playerState;

        if (currentState == PlayerProperties.PlayerState.Grounded || currentState == PlayerProperties.PlayerState.Crouching)
        {
            PlayAnimIfNotAlreadyPlaying("Skating");
        }
        else if (verticalVelocity > 0 && currentState != PlayerProperties.PlayerState.Rising
                 && currentState != PlayerProperties.PlayerState.OnRamp)
        {
            // Player is moving upwards (jumping)
            playerProps.ChangeState(PlayerProperties.PlayerState.Rising);
        
            PlayAnimIfNotAlreadyPlaying("in air");
        }
        else if (verticalVelocity < 0 && currentState != PlayerProperties.PlayerState.Grinding 
                                      && currentState != PlayerProperties.PlayerState.Falling
                                      && currentState != PlayerProperties.PlayerState.OnRamp)
        {
            // Player is moving downwards (falling) and not grinding
            playerProps.ChangeState(PlayerProperties.PlayerState.Falling);

            PlayAnimIfNotAlreadyPlaying("in air");
        }
    }

    private void PlayAnimIfNotAlreadyPlaying(string animName)
    {
        if (!animatorController.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animatorController.Play(animName);
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
    private void ApplyRisingForce()
    {
        Vector3 risingForce = Vector3.up * jumpMultiplier;
        rb.AddForce(risingForce, ForceMode.Acceleration);
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