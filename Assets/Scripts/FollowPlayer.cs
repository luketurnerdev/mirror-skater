using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform assignedPlayer;
    public float xOffset = 20f;

    void Update()
    {
        Follow();
    }

    public void AssignPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;
        assignedPlayer = GameObject.FindWithTag("Player").transform;
    }

    void Follow()
    {
        // Only follow in the x plane
        // add offset to 
        if (assignedPlayer == null) return;
        transform.position = new Vector3(assignedPlayer.position.x + xOffset, transform.position.y, transform.position.z);
    }
}
