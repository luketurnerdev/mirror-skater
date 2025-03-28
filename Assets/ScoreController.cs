using UnityEngine;
using TMPro;
public class ScoreController : MonoBehaviour
{
    float playerScore;
    
    // Multipliers
    public int scoreMultiplier = 10;
    private int originalScoreMultiplier;
    public int grindingMultiplierIncrement = 10;
    public TextMeshProUGUI scoreText;
    
    /*
     * Add to score over time while player is alive
     * Add extra points for every frame we are grinding
     * Add extra points for defeating enemies
     * Add extra points for doing tricks
     */

    public void IncreaseMultiplierForGrinding()
    {
        scoreMultiplier += grindingMultiplierIncrement;
    }
    
    public void ResetMultiplier()
    {
        scoreMultiplier = originalScoreMultiplier;
    }
    void AddScoreAsWeProgress()
    {
        playerScore += (Time.deltaTime * scoreMultiplier);
        // Only show text to 0 dp;
        scoreText.text = "Score: " + Mathf.Round(playerScore);
    }
    void Start()
    {
        originalScoreMultiplier = scoreMultiplier;
        playerScore = 0;
    }
    
    void FixedUpdate()
    {
        AddScoreAsWeProgress();
    }
}
