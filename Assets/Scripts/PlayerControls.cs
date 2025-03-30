using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 60f;
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
            MaybeDoRandomTrick();
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
        if (animatorController == null) return;
        if (!animatorController.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            animatorController.Play(animName);
        }
    }
    
    private void ApplyGravityForce()
    {
        Vector3 gravityForce = Physics.gravity.normalized * fallMultiplier;
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }


    private void AddOlliePopForce()
    {
        Vector3 popForce = -Physics.gravity.normalized * olliePopForce;
        rb.AddForce(popForce, ForceMode.Impulse);
    }

    // Runs in update
    private void ApplyRisingForce()
    {
        Vector3 risingForce = -Physics.gravity.normalized * jumpMultiplier;
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
    
    public void DoFrontflip(float duration = 1f)
    {
        StartCoroutine(RotatePlayer(Vector3.right, 360f, duration));
    }

    public void DoBackflip(float duration = 1f)
    {
        StartCoroutine(RotatePlayer(Vector3.right, -360f, duration));
    }

    public void SpinRight(float duration = 1f)
    {
        StartCoroutine(RotatePlayer(Vector3.up, 360f, duration));
    }

    public void SpinLeft(float duration = 1f)
    {
        StartCoroutine(RotatePlayer(Vector3.up, -360f, duration));
    }
    
    private bool isRotating = false;
    

    public void SnapUprightToSurface(bool isUpsideDown)
    {
        Vector3 upFacing = isUpsideDown ? Vector3.down : Vector3.up;
        Quaternion uprightRotation = Quaternion.LookRotation(Vector3.forward, upFacing);
        transform.rotation = uprightRotation;
    }


    public IEnumerator RotatePlayer(Vector3 axis, float degrees, float duration)
    {
        if (isRotating) yield break;
        isRotating = true;

        float elapsed = 0f;
        float totalRotation = 0f;
        float lastFrameRotation = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float currentRotation = Mathf.Lerp(0f, degrees, t);
            float deltaRotation = currentRotation - lastFrameRotation;

            transform.Rotate(axis, deltaRotation, Space.Self);

            lastFrameRotation = currentRotation;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Apply final leftover rotation in case of floating point drift
        float finalDelta = degrees - lastFrameRotation;
        transform.Rotate(axis, finalDelta, Space.Self);

        isRotating = false;
    }


    
    public void MaybeDoRandomTrick()
    {
        Debug.Log("MaybeDoRandomTrick called");
        
        if (isRotating) return; // don't start another if already flipping/spinning

        float chance = Random.value; // returns 0.0 to 1.0

        // if (chance < 0.7f)
        // {
        //     // 70% chance: do nothing
        //     return;
        // }

        // 30% chance: do a random trick
        int trickIndex = Random.Range(0, 4); // 0 to 3

        switch (trickIndex)
        {
            case 0:
                DoFrontflip();
                break;
            case 1:
                DoBackflip();
                break;
            case 2:
                SpinLeft();
                break;
            case 3:
                SpinRight();
                break;
        }
    }
    

}