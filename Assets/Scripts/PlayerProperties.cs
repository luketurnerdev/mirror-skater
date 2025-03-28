using UnityEngine;

public class PlayerProperties : MonoBehaviour
{

    public int health = 100;
    
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
        Destroy(gameObject);
        
    }
}
