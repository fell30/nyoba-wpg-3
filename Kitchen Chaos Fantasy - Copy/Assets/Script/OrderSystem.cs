using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderSystem : MonoBehaviour
{
    public List<KitchenObjectSO> possiblePotions;
    private Queue<KitchenObjectSO> orderQueue = new Queue<KitchenObjectSO>(); // Antrian untuk order
    private KitchenObjectSO currentOrder; // Order yang sedang aktif
    public GameObject orderUIPrefab;
    public Transform orderUIParent;
    public TextMeshProUGUI coinText;

    [SerializeField] private GameObject OrderSucces;

    private int maxOrders = 2;
    private int coins = 0;

    private GameObject currentOrderUI;
    private bool isTutorialCompleted = false;



    void Start()
    {

        // Jangan panggil GenerateOrders atau ShowNextOrder di sini.
        UpdateCoinUI(); // Update UI koin di awal
    }


    public void StartOrderSystem()
    {
        isTutorialCompleted = true;
        GenerateOrders(maxOrders);
        ShowNextOrder(); // Tampilkan order pertama dari antrian
    }

    void GenerateOrders(int count)
    {
        for (int i = 0; i < count; i++)
        {
            KitchenObjectSO newOrder = possiblePotions[Random.Range(0, possiblePotions.Count)];
            orderQueue.Enqueue(newOrder); // Masukkan order ke dalam antrian
        }
    }

    // Menampilkan order berikutnya dari antrian
    void ShowNextOrder()
    {
        if (isTutorialCompleted && orderQueue.Count > 0)
        {
            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI); // Hapus UI order sebelumnya jika ada
            }

            currentOrder = orderQueue.Dequeue(); // Ambil order dari antrian
            currentOrderUI = CreateOrderUI(currentOrder); // Buat UI untuk order tersebut
        }
        else if (!isTutorialCompleted)
        {
            Debug.Log("Tutorial belum selesai, sistem order tidak aktif.");
        }
        else
        {
            currentOrder = null; // Tidak ada order lagi
            Debug.Log("All orders completed!");
            OrderSucces.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("Level1");
                Debug.Log("Game Restarted");
            }
        }
    }

    // Metode untuk membuat UI order dan mengembalikan referensinya
    GameObject CreateOrderUI(KitchenObjectSO order)
    {
        if (orderUIPrefab == null)
        {
            Debug.LogError("Order UI Prefab is null! Make sure to assign it in the Inspector.");
            return null;
        }

        if (orderUIParent == null)
        {
            Debug.LogError("Order UI Parent is null! Make sure to assign it in the Inspector.");
            return null;
        }

        GameObject orderUI = Instantiate(orderUIPrefab, orderUIParent);
        Debug.Log("Order UI instantiated successfully.");

        TextMeshProUGUI orderText = orderUI.GetComponentInChildren<TextMeshProUGUI>();
        if (orderText != null)
        {
            orderText.text = order.name; // Tampilkan nama order
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is missing in the Order UI Prefab!");
        }

        return orderUI; // Kembalikan referensi ke UI yang baru dibuat
    }

    public bool CheckOrder(KitchenObjectSO potion)
    {
        return currentOrder != null && currentOrder == potion;
    }

    // Selesaikan order saat ini dan tampilkan order berikutnya
    public void CompleteOrder(KitchenObjectSO potion)
    {
        if (currentOrder != null && currentOrder == potion)
        {
            Debug.Log("Order completed: " + currentOrder.name);
            currentOrder = null; // Hapus order saat ini
            AddCoins(3); // Tambahkan koin

            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI);
                currentOrderUI = null; // Reset referensi UI order
            }

            ShowNextOrder(); // Tampilkan order berikutnya dari antrian
        }
        else
        {
            Debug.Log("Potion is not the current order.");
        }
    }


    private void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Coins added: " + amount + ". Total Coins: " + coins);
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coins.ToString();
        }
        else
        {
            Debug.LogError("Coin Text UI is not assigned!");
        }
    }

    // Panggil ini saat pemain berhasil menyajikan potion


}
