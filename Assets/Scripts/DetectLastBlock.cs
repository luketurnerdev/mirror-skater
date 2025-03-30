using System;
using UnityEngine;

public class DetectLastBlock : MonoBehaviour
{
    
    private FloorGenerator floorSegmentData;

    private void Start()
    {
        floorSegmentData = FindObjectOfType<FloorGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collider fire");
        if (other.gameObject.CompareTag("Player"))
        {
            floorSegmentData.GenerateFloors();
        }
    }
}
