using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("Object to Spawn")]
    public GameObject[] prefabToSpawn;

    [Header("Spawn Points (max 1 each)")]
    public Transform[] spawnPoints;

    [Header("Respawn Settings")]
    public float respawnCooldown = 5f;
    public int maxActiveObjects = 3;
    private bool[] isRespawning;


    private GameObject[] activeObjectsPerPoint;
    private bool firstSpawnDone = false;

    private void Start()
    {
        activeObjectsPerPoint = new GameObject[spawnPoints.Length];
        isRespawning = new bool[spawnPoints.Length];

        StartCoroutine(InitialSpawn());
    }

    private IEnumerator InitialSpawn()
    {
        // Spawn awal tanpa cooldown
        for (int i = 0; i < spawnPoints.Length && i < maxActiveObjects; i++)
        {
            SpawnAtPoint(i);
        }

        firstSpawnDone = true;
        yield return null;
    }

    private void Update()
    {
        if (!firstSpawnDone) return;

        for (int i = 0; i < spawnPoints.Length && GetActiveCount() < maxActiveObjects; i++)
        {
            if (activeObjectsPerPoint[i] == null && !isRespawning[i])
            {
                StartCoroutine(SpawnAfterDelay(i, respawnCooldown));
            }
        }


    }

    private IEnumerator SpawnAfterDelay(int index, float delay)
    {
        isRespawning[index] = true;
        yield return new WaitForSeconds(delay);

        if (activeObjectsPerPoint[index] == null && GetActiveCount() < maxActiveObjects)
        {
            SpawnAtPoint(index);
        }

        isRespawning[index] = false;
    }


    private void SpawnAtPoint(int index)
    {
        if (index >= prefabToSpawn.Length || index >= spawnPoints.Length)
        {
            Debug.LogWarning("Index out of bounds for spawn or prefab array.");
            return;
        }

        GameObject selectedPrefab = prefabToSpawn[index];

        GameObject obj = Instantiate(selectedPrefab, spawnPoints[index].position, spawnPoints[index].rotation);
        activeObjectsPerPoint[index] = obj;

        RespawnTracker tracker = obj.GetComponent<RespawnTracker>();
        if (tracker != null)
        {
            tracker.manager = this;
            tracker.spawnPointIndex = index;
        }
    }


    public void NotifyDestroyed(int index)
    {
        activeObjectsPerPoint[index] = null;
    }

    private int GetActiveCount()
    {
        int count = 0;
        foreach (var obj in activeObjectsPerPoint)
        {
            if (obj != null) count++;
        }
        return count;
    }
}
