using UnityEngine;

public class DetectGrinding : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerProperties.Instance.ChangeState(PlayerProperties.PlayerState.Grinding);
        }
    }
}
