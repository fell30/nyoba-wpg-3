using UnityEngine;

public class GridIndicator : MonoBehaviour
{
    public GameObject gridPrefab; // Prefab untuk indikator grid
    public Transform player; // Transform pemain
    public float gridSize = 1.0f; // Ukuran grid
    public Material validMaterial; // Material untuk posisi yang valid (hijau)
    public Material invalidMaterial; // Material untuk posisi tidak valid (merah)
    public LayerMask plantableLayer;

    private GameObject gridIndicator;
    private Renderer gridRenderer;
    private TreeGridSpawner treeGridSpawner; // Referensi ke skrip TreeGridSpawner

    void Start()
    {
        // Membuat instance indikator grid dari prefab
        gridIndicator = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity);
        gridIndicator.SetActive(false); // Nonaktifkan pada awal
        gridRenderer = gridIndicator.GetComponent<Renderer>();
        treeGridSpawner = FindObjectOfType<TreeGridSpawner>(); // Temukan TreeGridSpawner
    }

    void Update()
    {
        // Menemukan posisi grid terdekat
        Vector3 nearestGridPoint = GetNearestGridPosition();

        // Aktifkan indikator dan pindahkan ke posisi grid terdekat
        gridIndicator.SetActive(true);
        gridIndicator.transform.position = nearestGridPoint;

        // Cek apakah posisi valid dan belum ada pohon
        if (IsValidPlacement(nearestGridPoint) && !treeGridSpawner.plantedTrees.Contains(nearestGridPoint))
        {
            gridRenderer.material = validMaterial; // Jika valid, gunakan material hijau
        }
        else
        {
            gridRenderer.material = invalidMaterial; // Jika tidak valid atau sudah ada pohon, gunakan material merah
        }
    }

    // Fungsi untuk mendapatkan posisi grid terdekat
    public Vector3 GetNearestGridPosition()
    {
        Vector3 nearestGridPoint = new Vector3(
            Mathf.Round(player.position.x / gridSize) * gridSize,
            Mathf.Round(player.position.y / gridSize) * gridSize,
            Mathf.Round(player.position.z / gridSize) * gridSize
        );
        return nearestGridPoint;
    }

    // Fungsi untuk cek apakah posisi valid
    public bool IsValidPlacement(Vector3 position)
    {
        RaycastHit hit;
        // Raycast ke bawah untuk memeriksa apakah area bisa ditanami
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, 2f, plantableLayer))
        {
            return true; // Posisi valid jika mengenai area yang diizinkan
        }
        return false; // Posisi tidak valid jika tidak mengenai layer yang diizinkan
    }
}
