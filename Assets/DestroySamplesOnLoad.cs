using UnityEngine;

public class DestroySamplesOnLoad : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject);
    }
}
