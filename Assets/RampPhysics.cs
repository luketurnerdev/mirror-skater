using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RampPhysics : MonoBehaviour
{
    public float rampLaunchForce = 15f;  // How hard the player is launched off the ramp
    public float rampClimbSpeedMultiplier = 1.2f;  // How fast the player moves when climbing the ramp
    public float reducedGravityMultiplier = 0.3f;  // How much gravity is reduced while on the ramp
    
    private PlayerControls playerControls;
    
    private void Awake()
    {
        playerControls = FindObjectOfType<PlayerControls>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerProperties.PlayerState currentState = PlayerProperties.Instance.playerState;
            if (currentState != PlayerProperties.PlayerState.OnRamp)
            { 
                PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.OnRamp);
            }
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Get the normal of the ramp at the contact point
                Vector3 rampNormal = collision.contacts[0].normal;

                // Find the direction parallel to the ramp's surface
                Vector3 rampDirection = Vector3.Cross(Vector3.right, rampNormal).normalized;

                // Apply upward force to counteract gravity slightly
                Vector3 reducedGravity = Vector3.down * Physics.gravity.y * reducedGravityMultiplier;
                playerRb.AddForce(reducedGravity, ForceMode.Acceleration);

                // Apply force to push the player along the ramp surface
                playerRb.AddForce(rampDirection * rampClimbSpeedMultiplier, ForceMode.Force);
                
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // When the player leaves the ramp, apply an upward and forward force
                Vector3 launchDirection = (Vector3.up + playerRb.transform.forward).normalized;
                playerRb.AddForce(launchDirection * rampLaunchForce, ForceMode.Impulse);

                
                if (playerControls == null) {playerControls = FindObjectOfType<PlayerControls>();}

                // Give a permanent speed boost
                playerControls.moveSpeed += 0.05f;

                // Set the player's state to Rising
                PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Rising);
            }
        }
    }
}
