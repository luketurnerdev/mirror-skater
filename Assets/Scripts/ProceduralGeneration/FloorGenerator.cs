using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class FloorGenerator : MonoBehaviour
{
    public FloorSegmentData segmentPrefab;
    public GameObject floorSegmentsParent;
    public GameObject colliderPrefab;
    public Material redmat;
    public Material bluemat;
    
    // max amount of segments
    public int segmentCount = 10;
    
    // how long the literal segment is ?
    private float segmentLength = 10;
    private Vector2 spawnPosition = Vector2.zero;

    private void Start()
    {
        GenerateFloor();
    }

    public void CleanupPreviousBlocks()
    {
        // Find first 10 children of a floorSegmentsParent and delete them
        DestroyFirstTenChildren(floorSegmentsParent.transform);
    }
    
    void DestroyFirstTenChildren(Transform parentTransform)
    {
        if (parentTransform.childCount < 30)
        {
            return;
        }
        
        int childCount = Mathf.Min(10, parentTransform.childCount);  // Limit to 10 or less if there are fewer children

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);  // Get child at index i
            Destroy(child.gameObject);
        }
    }

    public void GenerateFloor()
    {
        // At the start of the game, generate x amount of floors (say 10)
        // If the player hits the last block (or maybe the last 2 blocks), generate another x amount of floors

        // Generate x amount of floor blocks
        for (int i = 0; i < segmentCount; i++)
        {
            var segmentData = segmentPrefab;

            GameObject segment = Instantiate(segmentData.floorPrefab, spawnPosition, Quaternion.identity);
            segment.transform.parent = floorSegmentsParent.transform;
            
            if (i % 2 == 0)
            {
                segment.GetComponent<Renderer>().material = redmat;
            }
            else
            {
                segment.GetComponent<Renderer>().material = bluemat;
            }
    
            spawnPosition.x += segmentLength;

            //0
            // x = 10
            // x = 20
           

            if (i == segmentCount-4)
            {
                Instantiate(colliderPrefab, segment.transform);
            }
        }
        
        // Garbage Collection
        CleanupPreviousBlocks();
        
        
    }
}