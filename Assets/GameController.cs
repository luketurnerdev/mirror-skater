using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    private FloorGenerator floorGenerator;
    private ScoreController scoreController;
    private UIController uIController;
    private FollowPlayer followPlayer;
    
    // Prefabs
    public GameObject playerPrefab;
    public GameObject playerSpawnPoint;
    
    public void InitGame()
    {
        StartCoroutine(Init(false));
    }
    
    public void RestartGame()
    {
        // Reset the game state
        // Restart the game
        StartCoroutine(Init(true));
    }

    private IEnumerator Init(bool respawning)
    {
        yield return new WaitForEndOfFrame();

        if (respawning)
        {
            // Destroy the old player
            GameObject currentPlayer = GameObject.FindWithTag("Player");
            if (currentPlayer != null)
            {
                Debug.Log("Destroying player: " + currentPlayer.name);
                Destroy (currentPlayer);
                yield return null;
            }
            
            // yield return new WaitForSeconds(1);
        }
        
        uIController.HideGameOverScreen();
        scoreController.ResetScore();
        floorGenerator.ClearAllBlocks();
        floorGenerator.ResetGenerationOnRespawn();
        floorGenerator.GenerateFloors();
        
        yield return null;
        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.transform.position, playerPrefab.transform.rotation);
        followPlayer.AssignPlayer();
        Time.timeScale = 1f;

    }
    void Awake()
    {
        uIController = FindObjectOfType<UIController>();
        floorGenerator = FindObjectOfType<FloorGenerator>();
        scoreController = FindObjectOfType<ScoreController>();
        followPlayer = FindObjectOfType<FollowPlayer>();
        InitGame();
    }

    void Update()
    {
        
    }
}
