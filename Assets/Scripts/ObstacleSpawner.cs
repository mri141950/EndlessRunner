using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject groundObstaclePrefab;   // Prefab for obstacles on the ground
    public GameObject airObstaclePrefab;      // Prefab for obstacles in the air

    [Header("Spawn Settings")]
    public Transform player;                  // Reference to the player
    public float spawnDistance = 25f;         // How far ahead of the player to spawn
    public float minSpawnInterval = 10f;      // Minimum horizontal distance between spawns
    public float maxSpawnInterval = 15f;      // Maximum horizontal distance between spawns

    [Header("Air Obstacle Settings")]
    [Range(0f, 1f)]
    public float airObstacleChance = 0.3f;    // Probability to spawn an air obstacle
    public float groundY = 0f;                // Y position for ground obstacles (ensure visibility)
    public float airMinY = 1f;                // Minimum Y position for air obstacles
    public float airMaxY = 3f;                // Maximum Y position for air obstacles

    private float nextSpawnX;

    void Start()
    {
        // Schedule the first spawn a random distance ahead of the player
        nextSpawnX = player.position.x + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        // If the player is within spawnDistance of nextSpawnX, spawn a new obstacle
        if (player.position.x + spawnDistance > nextSpawnX)
        {
            // Decide whether to spawn an air obstacle or a ground obstacle
            bool spawnAir = Random.value < airObstacleChance;

            // Determine vertical position based on obstacle type
            float yPos = spawnAir
                ? Random.Range(airMinY, airMaxY)
                : groundY;

            // Compute spawn position in world space
            Vector3 spawnPos = new Vector3(nextSpawnX, yPos, 0f);

            // Instantiate the chosen obstacle prefab
            Instantiate(
                spawnAir ? airObstaclePrefab : groundObstaclePrefab,
                spawnPos,
                Quaternion.identity
            );

            // Schedule the next spawn by adding a random interval to the previous spawn X
            nextSpawnX += Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
