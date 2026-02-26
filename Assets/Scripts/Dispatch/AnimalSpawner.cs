using UnityEngine;
using System.Collections;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject animalPrefab;        // Prefab of the animal
    public int numberToSpawn = 5;          // Total animals to spawn
    public float spawnDuration = 10f;      // Time over which to spawn them
    public float spawnRadius = 20f;        // Distance around player to spawn

    [Header("References")]
    public Transform player;               // Player to spawn around

    void Start()
    {
        if (player != null && animalPrefab != null)
            StartCoroutine(SpawnAnimals());
    }

    private IEnumerator SpawnAnimals()
    {
        float interval = spawnDuration / Mathf.Max(1, numberToSpawn);

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 spawnPos = player.position + Random.insideUnitSphere * spawnRadius;
            spawnPos.y = player.position.y; // Keep on same ground level

            Instantiate(animalPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(interval);
        }
    }
}