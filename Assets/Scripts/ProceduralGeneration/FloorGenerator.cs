using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public FloorSegmentData segmentPrefab;
    public GameObject floorSegmentsParent;
    public GameObject colliderPrefab;
    public Material redFloorMat; 
    public Material blueFoorMat;
    
    // max amount of segments
    private int segmentCount = 10;
    
    // Actual length of each block (determined at runtime)
    private float segmentLength;
    [SerializeField] private int howManyBlocksBackToSpawnNewBlocks;
    
    // Where to spawn the next segment
    private Vector2 currentSegmentSpawnPos = Vector2.zero;

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

            GameObject segment = Instantiate(segmentData.floorPrefab, currentSegmentSpawnPos, Quaternion.identity);
            segment.transform.parent = floorSegmentsParent.transform;
    
            segmentLength = segment.GetComponent<Renderer>().bounds.size.x;
            // Move the spawn position to the right by the length of the segment
            currentSegmentSpawnPos.x += segmentLength;
            
            AlternateFloorMaterial(segment, i);
            // Add stuff to the segment
            
            // SometimesAddStuffToSegment(segmentData, segment);
            
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

    void SometimesAddStuffToSegment(FloorSegmentData data, GameObject parent)
    {
        SometimesSpawnRailOnSegment(data, parent);
    }
    void SometimesSpawnRailOnSegment(FloorSegmentData data, GameObject parent)
    {
        data.hasRail = Random.value > 0.5f;
        if (!data.hasRail) return;
        
        GameObject rail = Instantiate(data.railPrefab, currentSegmentSpawnPos, Quaternion.identity);
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