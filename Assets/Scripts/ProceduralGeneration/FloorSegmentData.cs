using UnityEngine;

[System.Serializable]
public class FloorSegmentData
{
    // Floor blocks
    public GameObject floorPrefab;
    
    // Rail stuff
    public GameObject railPrefab;
    public bool hasRail;
    public Transform railSpawnPoint;
    
    
    public bool hasHazard;
}
