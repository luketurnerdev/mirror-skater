using UnityEngine;

public class GrindController : MonoBehaviour
{
    
    private ScoreController scoreController;
    
    /*
     *
     * When we are grinding, we want to get points for every frame we are grinding.
     * Play a particle effect or something.
     * Animation
     * 
     */

    void Start()
    {
        scoreController = FindObjectOfType<ScoreController>();
    }
    public bool isGrinding;

    public void SetIsGrinding(bool grinding)
    {
        isGrinding = grinding;

        if (isGrinding)
        {
            scoreController.IncreaseMultiplierForGrinding();
        }
        else
        {
            scoreController.ResetMultiplier();
        }
    }
}
