using UnityEngine;
using System.Collections;

public class PlayerProperties : MonoBehaviour
{
    public int health;
    private UIController _UIController;
    private ScoreController scoreController;
    private PlayerControls playerControls;
    
    public delegate void PlayerStateChangeHandler(PlayerState newState);
    public PlayerStateChangeHandler OnPlayerStateChange;
    
    public enum PlayerState
    {
        Grounded,
        ApproachingCrouch, // TODO
        Crouching,
        Rising,
        Falling,
        Grinding,
        Dead
    }

    public PlayerState playerState = PlayerState.Grounded;

    // Singleton
    public static PlayerProperties Instance { get; set; }

    // Falling Buffer System
    private bool isFallingBufferActive = false;
    private float fallingBufferDuration = 0.1f; // Duration of the buffer (in seconds)
    private float fallingBufferTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        _UIController = FindObjectOfType<UIController>();
        scoreController = FindObjectOfType<ScoreController>();
        playerControls = FindObjectOfType<PlayerControls>();
        
        health = 100;
        
        // Rigidbody stuff
        Rigidbody rb = GetComponent<Rigidbody>();
        // rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
    
    public void ChangeState(PlayerState newState)
    {
        if (newState == PlayerState.Falling)
        {
            // Activate falling buffer
            isFallingBufferActive = true;
            fallingBufferTimer = 0f;
        }

        // Notify subscribers of the state change
        OnPlayerStateChange?.Invoke(newState);
        
        // Reset score multiplier unless the state is Grinding
        scoreController.ResetMultiplier();
        
        Debug.Log("Changing state to " + newState);

           switch (newState)
        {
            case PlayerState.Crouching:
                playerState = PlayerState.Crouching;
                break;
            case PlayerState.Grounded:
                playerState = PlayerState.Grounded;
                break;
            case PlayerState.ApproachingCrouch:
                playerState = PlayerState.ApproachingCrouch;
                break;
            case PlayerState.Rising:
                playerState = PlayerState.Rising;
                break;
            case PlayerState.Falling:
                playerState = PlayerState.Falling;
                break;
            case PlayerState.Grinding:
                playerState = PlayerState.Grinding;
                scoreController.IncreaseMultiplierForGrinding();
                break;
            case PlayerState.Dead:
                playerState = PlayerState.Dead;
                break;
            default:
                break;
        }
    }

    public bool IsNotAllowedToJump()
    {
        return playerState == PlayerState.Rising ||
            playerState == PlayerState.Falling ||
            playerState == PlayerState.Dead;
    }
    
    public bool IsNotAllowedToCrouch()
    {
        // All of the previous states, plus if we are already crouching.
        return IsNotAllowedToJump() || playerState == PlayerState.Crouching;
    }
    public bool IsFallingBufferActive()
    {
        return isFallingBufferActive;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Time.timeScale = 0;
        _UIController.ShowGameOverScreen();
        
        // pause execution of game 
        // death fx
        // Visually delete player
        // show game over screen / score / respawn button / leaderboard button
    }
    
}
