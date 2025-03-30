using System.Collections;
using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    public bool canFlip = true;

    private Transform playerTransform;
    private Rigidbody playerRb;

    public float flipCooldown = 0.5f;
    public float surfaceSnapDistance = 30f;
    private bool isUpsideDown = false;
    private float nextFlipTime = 0f;

    private Coroutine rotationCoroutine;

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

        if (Time.time >= nextFlipTime && Input.GetKeyDown(KeyCode.G) && canFlip)
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
        canFlip = false; // ⛔ Lock flipping until re-enabled

        // 1. Flip Unity gravity
        Physics.gravity *= -1;

        // 2. Cancel vertical velocity
        Vector3 velocity = playerRb.linearVelocity;
        velocity.y = 0;
        playerRb.linearVelocity = velocity;

        // 3. Stop active rotation, start new 180° Z rotation
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(RotatePlayerByZ(180f, 0.4f));
    }
    
    public bool IsUpsideDown()
    {
        return isUpsideDown;
    }


    private IEnumerator RotatePlayerByZ(float degrees, float duration)
    {
        Quaternion startRotation = playerTransform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, degrees);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            playerTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerTransform.rotation = endRotation;
        rotationCoroutine = null;
    }



    // ✅ Call this when player lands (e.g., from DetectPlayerGrounded)
    public void EnableFlip()
    {
        canFlip = true;
    }
}
