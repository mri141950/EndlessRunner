using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Spawning")]
    public GameObject obstaclePrefab;    // Obstacle prefab to spawn
    public Transform player;             // Reference to the player
    public float spawnDistance = 25f;    // How far ahead of player to spawn
    public float minSpawnInterval = 10f; // Min horizontal distance between obstacles
    public float maxSpawnInterval = 15f; // Max horizontal distance between obstacles

    private float nextSpawnX;

    void Start()
    {
        // Schedule first obstacle
        nextSpawnX = player.position.x + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        // ����ҿ��� nextSpawnX ʱ������
        if (player.position.x + spawnDistance > nextSpawnX)
        {
            Vector3 spawnPos = new Vector3(nextSpawnX, transform.position.y, 0f);
            Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
            // ������һ�ε� nextSpawnX �ۼ�һ��������
            nextSpawnX += Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }
}
