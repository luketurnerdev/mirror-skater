using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FloorGenerator : MonoBehaviour
{
    public List<GameObject> floorSegments = new List<GameObject>();
    public GameObject floorPrefab;
    public GameObject railPrefab;
    public GameObject rampPrefab;
    public GameObject floorSegmentsParent;
    public GameObject colliderPrefab;
    public Material redFloorMat; 
    public Material blueFoorMat;
    
    public bool lastBlockHadRamp = false;
    public bool lastBlockHadRail = false;

    public bool hasRails = true;
    public bool hasRamps = true;
    
    public float railSpawnChancePercentage = 25;
    public float rampSpawnChancePercentage = 50;
    
    // max amount of segments
    public int segmentCount = 10;
    
    // Actual length of each block (determined at runtime)
    private float segmentLength;
    [SerializeField] private int howManyBlocksBackToSpawnNewBlocks;
    
    // Where to spawn the next segment
    private Vector2 currentSegmentSpawnPos = Vector2.zero;

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

    public void ClearAllBlocks()
    {
        foreach (GameObject segment in floorSegments)
        {
            Destroy(segment);
        }
        floorSegments.Clear();
    }
    
    private int floorSegmentIndex = 0;
    private int roofSegmentIndex = 0;

    public void GenerateBlocks()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            GenerateSegmentPair();
        }
        
        floorSegmentIndex = 0;
        roofSegmentIndex = 0;

        CleanupPreviousBlocks();
    }
    
    
   

    private void GenerateSegmentPair()
    {
        
        Vector2 floorPos = currentSegmentSpawnPos;

        // 1. Create the floor segment
        GameObject floorSegment = Instantiate(floorPrefab, floorPos, Quaternion.identity);
        floorSegment.transform.parent = floorSegmentsParent.transform;
        AlternateFloorMaterial(floorSegment, floorSegmentIndex);
    
        // 2. Add ramp/rail randomly
        FloorSegmentData floorData = new FloorSegmentData();
        SometimesAddStuffToSegment(floorData, floorSegment);
        floorSegments.Add(floorSegment);

        // 3. Create mirrored roof block
        GameObject roofSegment = Instantiate(floorSegment);
        roofSegment.transform.position = new Vector3(
            floorSegment.transform.position.x,
            floorSegment.transform.position.y + 20f,
            floorSegment.transform.position.z
        );
        roofSegment.transform.parent = floorSegmentsParent.transform;
        roofSegment.transform.rotation = Quaternion.Euler(0, 0, 180);

        // 4. Flip X pos of ramps/rails & rotate ramps
        foreach (Transform child in roofSegment.transform)
        {
            Vector3 localPos = child.localPosition;
            localPos.x *= -1;
            child.localPosition = localPos;

            string name = child.name.ToLower();

            if (name.Contains("ramp"))
            {
                child.localRotation *= Quaternion.Euler(0, 0, 180); // Flip ramp for skatable orientation
            }

            if (name.Contains("rail"))
            {
                child.localRotation *= Quaternion.Euler(0, 180, 0); // Flip rail front-to-back
            }
        }


        AlternateFloorMaterial(roofSegment, roofSegmentIndex); // Separate alternating logic
        floorSegments.Add(roofSegment);

        // 5. Collider if needed
        if (floorSegmentIndex == segmentCount - howManyBlocksBackToSpawnNewBlocks)
        {
            Instantiate(colliderPrefab, floorSegment.transform);
        }

        // 6. Advance
        segmentLength = floorSegment.GetComponent<Renderer>().bounds.size.x;
        currentSegmentSpawnPos.x += segmentLength;

        floorSegmentIndex++;
        roofSegmentIndex++;
    }



    
    void SpawnRampOnSegment(GameObject parent, bool mirrored = false)
    {
        GameObject ramp = Instantiate(rampPrefab, currentSegmentSpawnPos, rampPrefab.transform.rotation);
        ramp.transform.parent = parent.transform;

        Vector3 offset = new Vector3(-0.3f, 0.5f, 0);
        if (mirrored) offset.y *= -1;

        ramp.transform.localPosition += offset;
    }

    void SpawnRailOnSegment(GameObject parent, bool mirrored = false)
    {
        GameObject rail = Instantiate(railPrefab, currentSegmentSpawnPos, Quaternion.identity);
        rail.transform.parent = parent.transform;

        Vector3 offset = new Vector3(0, 2, 0);
        if (mirrored) offset.y *= -1;

        rail.transform.localPosition += offset;
    }

    
    void SometimesAddStuffToSegment(FloorSegmentData data, GameObject segment)
    {
        SometimesSpawnRailOnSegment(data, segment);
        SometimesSpawnRampOnSegment(data, segment);
    }


    void SometimesSpawnRampOnSegment(FloorSegmentData data, GameObject parent)
    {
        if (data.hasRail || data.hasRamp || !hasRamps) return;
        
        bool shouldSpawnRamp = Random.value < (rampSpawnChancePercentage / 100);
        if (!shouldSpawnRamp || lastBlockHadRamp)
        {
            lastBlockHadRamp = false;
            return;
        }
        
        lastBlockHadRamp = true;
        data.hasRamp = true;
        GameObject ramp = Instantiate(rampPrefab, currentSegmentSpawnPos, rampPrefab.transform.rotation);
        ramp.transform.parent = parent.transform;
        
        // Move it up a bit
        ramp.transform.localPosition += new Vector3(-0.3f, 0.5f, 0);
    }
    void SometimesSpawnRailOnSegment(FloorSegmentData data, GameObject parent)
    {
        if (data.hasRail || data.hasRamp || !hasRails) return;
        
        // if the random value is greater than the spawn chance as a percentage, spawn the rail
        
        bool shouldSpawnRail = Random.value < (railSpawnChancePercentage / 100);
        if (!shouldSpawnRail || lastBlockHadRail)
        {
            lastBlockHadRail = false;
            return;
        }
        
        lastBlockHadRail = true;
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