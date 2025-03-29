using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float xOffset = 20f;

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        // Only follow in the x plane
        // add offset to 
        transform.position = new Vector3(player.position.x + xOffset, transform.position.y, transform.position.z);
    }
}
