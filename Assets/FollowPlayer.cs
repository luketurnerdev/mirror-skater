using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        // Only follow in the x plane
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
    }
}
