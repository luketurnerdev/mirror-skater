using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject railPrefab;
    public GameObject rampPrefab;
    public GameObject floorSegmentsParent;
    public GameObject colliderPrefab;
    public Material redFloorMat; 
    public Material blueFoorMat;

    public bool hasRails = true;
    public bool hasRamps = true;
    
    public float railSpawnChance = 0.5f;
    public float rampSpawnChance = 0.5f;
    
    // max amount of segments
    public int segmentCount = 10;
    
    // Actual length of each block (determined at runtime)
    private float segmentLength;
    [SerializeField] private int howManyBlocksBackToSpawnNewBlocks;
    
    // Where to spawn the next segment
    private Vector2 currentSegmentSpawnPos = Vector2.zero;

    private void Start()
    {
        GenerateFloor();
    }

    public void ResetGenerationOnRespawn()
    {
        currentSegmentSpawnPos = Vector2.zero;
    }
    public void CleanupPreviousBlocks()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
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
            // Instantiate new data class instance and associated gameobject
            FloorSegmentData segmentData = new FloorSegmentData();
            GameObject segment = Instantiate(floorPrefab, currentSegmentSpawnPos, Quaternion.identity);
            
            // Set parent
            segment.transform.parent = floorSegmentsParent.transform;
            
            // Move the spawn position to the right by the length of the segment
            segmentLength = segment.GetComponent<Renderer>().bounds.size.x;
            currentSegmentSpawnPos.x += segmentLength;
            
            // Change material for clarity
            AlternateFloorMaterial(segment, i);
            
            // Add stuff randomly to the segment
            SometimesAddStuffToSegment(segmentData, segment);
            
            // The index of the block where we want to spawn a new block
            // When user crosses it

            int blockIndexForNewSpawn = segmentCount - howManyBlocksBackToSpawnNewBlocks;
            if (i == blockIndexForNewSpawn)
            {
                Instantiate(colliderPrefab, segment.transform);
            }
        }
        
        // Garbage Collection
        CleanupPreviousBlocks();
    }

    void SometimesAddStuffToSegment(FloorSegmentData data, GameObject segment)
    {
        SometimesSpawnRailOnSegment(data, segment);
        SometimesSpawnRampOnSegment(data, segment);
    }

    void SometimesSpawnRampOnSegment(FloorSegmentData data, GameObject parent)
    {
        if (data.hasRail || data.hasRamp || !hasRamps) return;
        
        bool shouldSpawnRamp = Random.value > rampSpawnChance;
        if (!shouldSpawnRamp) return;
        
        data.hasRamp = true;
        GameObject ramp = Instantiate(rampPrefab, currentSegmentSpawnPos, rampPrefab.transform.rotation);
        ramp.transform.parent = parent.transform;
        
        // Move it up a bit
        ramp.transform.localPosition += new Vector3(0, 0.5f, 0);
    }
    void SometimesSpawnRailOnSegment(FloorSegmentData data, GameObject parent)
    {
        if (data.hasRail || data.hasRamp || !hasRails) return;
        
        bool shouldSpawnRail = Random.value > railSpawnChance;
        if (!shouldSpawnRail) return;
        
        data.hasRail = true;
        
        GameObject rail = Instantiate(railPrefab, currentSegmentSpawnPos, Quaternion.identity);
        rail.transform.parent = parent.transform;
        
        // Move it up a bit
        rail.transform.localPosition += new Vector3(0, 2, 0);
    }

    void AlternateFloorMaterial(GameObject segment, int i)
    {
        if (i % 2 == 0)
        {
            segment.GetComponent<Renderer>().material = redFloorMat;
        }
        else
        {
            segment.GetComponent<Renderer>().material = blueFoorMat;
        }
    }
}