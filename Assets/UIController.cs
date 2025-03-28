using UnityEngine;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour
{
    public GameObject gameOverCanvas;
    public GameObject UICanvas;
    
    public void ShowGameOverScreen()
    {
        gameOverCanvas.SetActive(true);
        UICanvas.SetActive(false);
    }
    
    public void HideGameOverScreen()
    {
        gameOverCanvas.SetActive(false);
        UICanvas.SetActive(true);
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        HideGameOverScreen();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    
}
