using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] Items; // Array untuk item (buah) yang akan di-spawn
    public Transform[] spawnPoints; // Array untuk titik spawn buah
    public int maxFruits = 5; // Maksimal jumlah buah
    public float respawnDelay = 5.0f; // Waktu respawn item setelah kosong
    public float spawnDelay = 0.2f; // Jeda waktu antar spawn item

    private List<GameObject> spawnedFruits = new List<GameObject>(); // List untuk menyimpan buah yang di-spawn

    void Start()
    {
        // Spawn item pertama kali di awal game
        StartCoroutine(SpawnAllFruits(maxFruits));
    }

    void Update()
    {
        // Hapus buah yang sudah diambil atau hancur
        spawnedFruits.RemoveAll(fruit => fruit == null);

        // Cek jika semua item habis, lakukan respawn setelah 5 detik
        if (spawnedFruits.Count == 0)
        {
            StartCoroutine(RespawnFruits());
        }
    }

    // Coroutine untuk respawn item setelah 5 detik
    private IEnumerator RespawnFruits()
    {
        yield return new WaitForSeconds(respawnDelay); // Tunggu 5 detik
        StartCoroutine(SpawnAllFruits(maxFruits)); // Spawn 5 item baru
    }

    // Coroutine untuk spawn semua buah sekaligus
    private IEnumerator SpawnAllFruits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            RandomSpawn(); // Spawn item satu per satu
            yield return new WaitForSeconds(spawnDelay); // Tunggu sedikit sebelum spawn item berikutnya
        }
    }

    // Method untuk spawn item secara acak di titik spawn
    private void RandomSpawn()
    {
        int randomItemIndex = Random.Range(0, Items.Length);
        int randomPointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomPointIndex];

        // Spawn item di titik yang dipilih secara acak
        GameObject spawnedFruit = Instantiate(Items[randomItemIndex], spawnPoint.position, Quaternion.identity);
        spawnedFruits.Add(spawnedFruit); // Tambahkan ke list
    }
}
