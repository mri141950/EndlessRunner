using UnityEngine;

/// <summary>
/// Spawns ground segments randomly and occasionally leaves gaps for player jumps.
/// Attach to an empty GameObject. Set groundPrefabs, playerTransform, and parameters in Inspector.
/// </summary>
public class GroundSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefabs of ground segments to spawn.")]
    public GameObject[] groundPrefabs;

    [Tooltip("Transform of the player (or camera) to track distance for spawning.")]
    public Transform playerTransform;

    [Tooltip("X coordinate at which to start spawning new segments.")]
    public float spawnX = 20f;

    [Tooltip("How far ahead of the player to maintain ground segments.")]
    public float spawnThreshold = 15f;

    [Header("Gap Settings")]
    [Range(0f, 1f)]
    [Tooltip("Probability [0-1] that the next spawn will be a gap instead of ground.")]
    public float gapProbability = 0.2f;

    [Tooltip("Minimum width of gap in world units.")]
    public float minGapWidth = 1f;
    [Tooltip("Maximum width of gap in world units.")]
    public float maxGapWidth = 3f;

    // Tracks the X position where we should spawn next (ground or gap)
    private float nextSpawnX;

    void Start()
    {
        nextSpawnX = spawnX;
    }

    void Update()
    {
        // Keep spawning until we've filled ahead of the player
        while (nextSpawnX < playerTransform.position.x + spawnThreshold)
        {
            TrySpawnSegment();
        }
    }

    /// <summary>
    /// Attempts to spawn either a gap or a ground segment based on gapProbability.
    /// </summary>
    void TrySpawnSegment()
    {
        float roll = Random.value;
        if (roll < gapProbability)
        {
            // Spawn a gap
            float gapWidth = Random.Range(minGapWidth, maxGapWidth);
            nextSpawnX += gapWidth;
        }
        else
        {
            // Spawn a ground segment
            SpawnGround();
        }
    }

    /// <summary>
    /// Picks a random ground prefab, instantiates it, and advances nextSpawnX.
    /// </summary>
    void SpawnGround()
    {
        // Choose a random prefab
        GameObject prefab = groundPrefabs[Random.Range(0, groundPrefabs.Length)];

        // Instantiate at nextSpawnX, preserving prefab Y and Z
        Vector3 spawnPos = new Vector3(nextSpawnX, prefab.transform.position.y, prefab.transform.position.z);
        GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, transform);

        // Determine width of the spawned segment
        float width = 0f;
        var box = go.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            width = box.size.x * go.transform.localScale.x;
        }
        else
        {
            var sprite = go.GetComponent<SpriteRenderer>();
            width = sprite.bounds.size.x;
        }

        // Advance nextSpawnX by the width of this segment
        nextSpawnX += width;
    }
}
