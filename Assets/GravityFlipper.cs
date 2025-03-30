using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody playerRb;

    public float flipCooldown = 0.5f;
    public float surfaceSnapDistance = 30f;
    private bool isUpsideDown = false;
    private float nextFlipTime = 0f;

    void Start()
    {
        // Ensure gravity starts downward
        Physics.gravity = Vector3.down * 9.81f;

        FindPlayer();
    }

    void Update()
    {
        if (playerTransform == null || playerRb == null)
        {
            FindPlayer();
            if (playerTransform == null || playerRb == null) return;
        }

        if (Time.time >= nextFlipTime && Input.GetKeyDown(KeyCode.G))
        {
            FlipGravity();
            nextFlipTime = Time.time + flipCooldown;
        }
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRb = player.GetComponent<Rigidbody>();
        }
    }

    void FlipGravity()
    {
        isUpsideDown = !isUpsideDown;

        // 1. Flip gravity
        Physics.gravity *= -1;

        // 2. Flip visual scale
        Vector3 scale = playerTransform.localScale;
        scale.y *= -1;
        playerTransform.localScale = scale;

        // 3. Cancel vertical velocity
        Vector3 velocity = playerRb.linearVelocity;
        velocity.y = 0;
        playerRb.linearVelocity = velocity;
    }
}