using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject[] groundPrefabs;
    public GameObject groundObstaclePrefab;
    public GameObject airObstaclePrefab;
    public Transform player;

    [Header("Spawn Settings")]
    public float startX = 10f;
    public float spawnDistanceAhead = 20f;

    [Header("Segment Settings")]
    public float minGroundWidth = 5f;
    public float maxGroundWidth = 8f;

    public float minGapWidth = 1f;
    public float maxGapWidth = 3f;

    [Range(0f, 1f)] public float obstacleChance = 0.5f;
    [Range(0f, 1f)] public float airObstacleChance = 0.3f;

    public float groundY = 0f;
    public float airMinY = 1.5f;
    public float airMaxY = 3f;

    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = startX;
    }

    void Update()
    {
        while (nextSpawnX < player.position.x + spawnDistanceAhead)
        {
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        bool spawnGround = Random.value > 0.2f;

        if (spawnGround)
        {
            float groundWidth = Random.Range(minGroundWidth, maxGroundWidth);

            GameObject prefab = groundPrefabs[Random.Range(0, groundPrefabs.Length)];
            Vector3 spawnPos = new Vector3(nextSpawnX + groundWidth / 2f, groundY, 0f);
            GameObject ground = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            ground.transform.localScale = new Vector3(groundWidth, ground.transform.localScale.y, 1);

            if (Random.value < obstacleChance)
            {
                bool isAir = Random.value < airObstacleChance;
                float yPos = isAir ? Random.Range(airMinY, airMaxY) : groundY;
                Vector3 obstaclePos = new Vector3(nextSpawnX + groundWidth * 0.75f, yPos, 0f);
                GameObject obstacle = Instantiate(
                    isAir ? airObstaclePrefab : groundObstaclePrefab,
                    obstaclePos,
                    Quaternion.identity,
                    transform
                );
            }

            nextSpawnX += groundWidth;
        }
        else
        {
            float gapWidth = Random.Range(minGapWidth, maxGapWidth);
            nextSpawnX += gapWidth;
        }
    }
}
