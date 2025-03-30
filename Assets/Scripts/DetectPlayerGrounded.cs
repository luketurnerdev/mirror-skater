using UnityEngine;

public class DetectPlayerGrounded : MonoBehaviour
{
    private PlayerControls playerControls;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerControls = other.gameObject.GetComponent<PlayerControls>();
            if (playerControls == null)
            {
                Debug.LogError("PlayerControls component not found on the player object");
                return;
            }

            playerControls.SetIsGrounded();

            GravityFlipper flipper = FindObjectOfType<GravityFlipper>();
            if (flipper != null)
            {
                flipper.EnableFlip();

                // ✅ Snap upright when landing
                playerControls.SnapUprightToSurface(flipper.IsUpsideDown());
            }
        }
    }


    void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
    }
}