using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerProperties playerProperties = other.gameObject.GetComponent<PlayerProperties>();

            if (playerProperties == null) return;
            
            playerProperties.TakeDamage(100);
        }
    }
}
