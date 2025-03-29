using UnityEngine;

public class DetectGrinding : MonoBehaviour
{
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerProperties.Instance.playerState != PlayerProperties.PlayerState.Grinding)
        { 
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Grinding);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Falling);
        }
    }
}
