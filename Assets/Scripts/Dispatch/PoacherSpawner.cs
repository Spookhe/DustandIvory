using UnityEngine;
using System.Collections;

public class PoacherSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject poacherPrefab;       // Prefab of the poacher
    public int numberToSpawn = 5;          // Total poachers to spawn
    public float spawnDuration = 10f;      // Time over which to spawn them
    public float spawnRadius = 20f;        // Distance around player to spawn

    [Header("References")]
    public Transform player;               // Player to spawn around

    void Start()
    {
        if (player != null && poacherPrefab != null)
            StartCoroutine(SpawnPoachers());
    }

    private IEnumerator SpawnPoachers()
    {
        float interval = spawnDuration / Mathf.Max(1, numberToSpawn);

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 spawnPos = player.position + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = player.position.y; // Keep on same ground level

            Instantiate(poacherPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }
    }
}