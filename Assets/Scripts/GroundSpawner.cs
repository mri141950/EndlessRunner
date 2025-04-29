using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Ground Spawning")]
    public GameObject groundPrefab;
    public Transform player;
    public float spawnDistance = 20f;
    public float groundTileWidth = 20f;

    [Header("Gap Settings")]
    [Range(0f, 1f)]
    public float gapChance = 1f;
    public int minGapTiles = 1;
    public int maxGapTiles = 3;

    private float nextSpawnX;

    void Start()
    {
        Debug.Log($"[GroundSpawner] Start, player.x={player.position.x}");
        nextSpawnX = player.position.x;
        Debug.Log($"[GroundSpawner] Initial nextSpawnX={nextSpawnX}");

        for (int i = 0; i < 3; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        Debug.Log($"[GroundSpawner] Update, nextSpawnX={nextSpawnX}");
        if (player.position.x + spawnDistance > nextSpawnX)
        {
            Debug.Log($"[GroundSpawner] In spawnDistance range");
            float rand = Random.value;
            Debug.Log($"[GroundSpawner] rand={rand:F2}, gapChance={gapChance}");
            if (rand < gapChance)
            {
                int gapCount = Random.Range(minGapTiles, maxGapTiles + 1);
                Debug.Log($"[GroundSpawner] SKIP {gapCount} tiles");
                nextSpawnX += groundTileWidth * gapCount;
            }
            else
            {
                Debug.Log($"[GroundSpawner] SPAWN one tile");
                SpawnTile();
            }
        }
    }

    private void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(nextSpawnX, transform.position.y, 0f);
        Instantiate(groundPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"[GroundSpawner] Spawned tile at X={nextSpawnX}");
        nextSpawnX += groundTileWidth;
    }
}
