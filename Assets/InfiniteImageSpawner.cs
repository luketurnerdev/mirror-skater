using System.Collections.Generic;
using UnityEngine;

public class InfiniteImageSpawner : MonoBehaviour
{
    public GameObject imagePrefab;       // Prefab of the quad or canvas image
    public float tileWidth = 10f;        // Width of each image in world units

    private Transform player;            // Automatically assigned at runtime
    private HashSet<int> spawnedTiles = new HashSet<int>();

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("InfiniteImageSpawner: No GameObject with tag 'Player' found!");
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("InfiniteImageSpawner: Player not found!");
                return;
            }
        }

        int currentTileIndex = Mathf.FloorToInt(player.position.x / tileWidth);

        if (!spawnedTiles.Contains(currentTileIndex))
        {
            Vector3 spawnPos = new Vector3(currentTileIndex * tileWidth, transform.position.y, transform.position.z);
            Instantiate(imagePrefab, spawnPos, Quaternion.identity, transform);
            spawnedTiles.Add(currentTileIndex);
        }
    }
}