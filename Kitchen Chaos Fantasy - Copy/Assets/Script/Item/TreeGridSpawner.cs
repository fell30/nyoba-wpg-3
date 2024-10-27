using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Pastikan menggunakan TextMeshPro

public class TreeGridSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Prefab pohon yang akan ditanam
    public float gridSize = 1f; // Ukuran grid
    public float plantingTime = 1f; // Waktu untuk menanam pohon
    public int maxTrees = 2; // Batas maksimal pohon yang bisa ditanam
    public TextMeshProUGUI messageText; // TextMeshPro untuk pesan kesalahan

    private bool isPlanting = false; // Apakah sedang menanam pohon
    private int treeCount = 0; // Hitungan pohon yang sudah ditanam
                               // Tetap private
    public HashSet<Vector3> plantedTrees = new HashSet<Vector3>();

    // Properti publik hanya untuk akses baca
    public HashSet<Vector3> PlantedTrees
    {
        get { return plantedTrees; }
    }

    public GridIndicator gridIndicator;

    void Start()
    {
        gridIndicator = FindObjectOfType<GridIndicator>(); // Temukan skrip GridIndicator di scene
    }

    void Update()
    {
        // Saat menekan E dan player tidak sedang menanam pohon
        if (Input.GetKeyDown(KeyCode.E) && !isPlanting)
        {
            if (treeCount < maxTrees)
            {
                Vector3 gridPosition = gridIndicator.GetNearestGridPosition(); // Dapatkan posisi grid dari GridIndicator

                // Cek apakah area valid dan belum ada pohon yang tertanam di grid ini
                if (gridIndicator.IsValidPlacement(gridPosition) && !plantedTrees.Contains(gridPosition))
                {
                    StartCoroutine(PlantTree(gridPosition)); // Mulai tanam pohon
                }
                else if (plantedTrees.Contains(gridPosition))
                {
                    ShowMessage("Sudah ada pohon di area ini.");
                }
                else
                {
                    ShowMessage("Area tidak valid untuk menanam pohon.");
                }
            }
            else
            {
                ShowMessage("Anda hanya bisa menanam maksimal " + maxTrees + " pohon.");
            }
        }
    }

    // Coroutine untuk menanam pohon setelah beberapa detik
    IEnumerator PlantTree(Vector3 position)
    {
        isPlanting = true;
        Debug.Log("Menanam pohon, tunggu " + plantingTime + " detik...");
        yield return new WaitForSeconds(plantingTime); // Tunggu selama plantingTime detik
        Instantiate(treePrefab, position, Quaternion.identity); // Tanam pohon
        plantedTrees.Add(position); // Tambahkan posisi grid ke daftar yang sudah tertanam pohon
        treeCount++; // Tambahkan hitungan pohon
        isPlanting = false;
        Debug.Log("Pohon berhasil ditanam! Total pohon: " + treeCount);
    }

    // Fungsi untuk menampilkan pesan error menggunakan TextMeshPro
    void ShowMessage(string message)
    {
        messageText.text = message;
        Invoke("ClearMessage", 2f); // Bersihkan pesan setelah 2 detik
    }

    void ClearMessage()
    {
        messageText.text = "";
    }
}
