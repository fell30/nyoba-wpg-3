using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCounter : BaseCounter
{
    // Event yang akan dipanggil saat player mengambil object
    public event EventHandler OnGrabbedKitchenObject;

    // ScriptableObject untuk KitchenObject
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // Jumlah maksimal object yang bisa diambil dari tree
    [SerializeField] private int maxKitchenObjectCount = 5;

    // Jumlah object yang tersedia
    private int currentKitchenObjectCount;

    private void Awake()
    {
        // Inisialisasi jumlah object yang tersedia dengan nilai maksimum
        currentKitchenObjectCount = maxKitchenObjectCount;
    }

    public override void Interact(Player player)
    {
        // Jika player tidak memiliki KitchenObject dan tree memiliki object yang tersedia
        if (!player.HasKitchenObject() && currentKitchenObjectCount > 0)
        {
            // Instantiate KitchenObject dari prefab
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            // Kurangi jumlah object yang tersedia
            currentKitchenObjectCount--;

            // Panggil event saat object diambil
            OnGrabbedKitchenObject?.Invoke(this, EventArgs.Empty);

            // Debug log untuk mengetahui sisa object
            Debug.Log("KitchenObject diambil. Sisa object: " + currentKitchenObjectCount);
        }
        else if (currentKitchenObjectCount <= 0)
        {
            // Tidak ada object yang tersisa, berikan pesan atau indikasi
            Debug.Log("Object Kitchen habis! Tidak bisa mengambil lagi.");
        }
    }

    // Method untuk menambah jumlah object, bisa dipanggil dari script lain untuk mengisi ulang object
    public void RefillKitchenObjects(int amount)
    {
        currentKitchenObjectCount += amount;

        // Pastikan jumlah tidak melebihi maksimum
        if (currentKitchenObjectCount > maxKitchenObjectCount)
        {
            currentKitchenObjectCount = maxKitchenObjectCount;
        }

        // Debug log untuk mengetahui jumlah object setelah diisi ulang
        Debug.Log("Object Kitchen diisi ulang. Jumlah sekarang: " + currentKitchenObjectCount);
    }

    // Method untuk mendapatkan jumlah object yang tersisa, bisa untuk keperluan UI atau logika lain
    public int GetCurrentKitchenObjectCount()
    {
        return currentKitchenObjectCount;
    }
}
