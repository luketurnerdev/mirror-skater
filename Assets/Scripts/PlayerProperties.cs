using UnityEngine;

public class PlayerProperties : MonoBehaviour
{

    public int health = 100;

    private UIController _UIController;
    
    void Start()
    {
        _UIController = FindObjectOfType<UIController>();
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
        // pause execution of game         // pause the sidescrolling
        // death fx
        // Visually delete player
        // show game over screen / score / respawn button / leaderboard button
    }
}
