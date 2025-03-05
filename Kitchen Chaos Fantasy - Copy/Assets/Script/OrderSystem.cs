using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderSystem : MonoBehaviour
{
    public List<KitchenObjectSO> possiblePotions;
    private Queue<KitchenObjectSO> orderQueue = new Queue<KitchenObjectSO>();
    private KitchenObjectSO currentOrder;
    public GameObject orderUIPrefab;
    public GameObject Timer;
    public Transform orderUIParent;

    [SerializeField] private GameObject OrderSucces;

    private int maxOrders = 2;
    private GameObject currentOrderUI;
    private bool isTutorialCompleted = false;

    public Dictionary<KitchenObjectSO, GameObject> potionRecipeUIs; // UI resep untuk tiap potion
    private GameObject currentRecipeUI; // UI resep yang sedang aktif

    void Start()
    {
        // Jangan panggil GenerateOrders di Start, akan dipanggil setelah tutorial selesai
    }

    public void StartOrderSystem()
    {
        isTutorialCompleted = true;
        GenerateOrders(maxOrders);
        ShowNextOrder();
    }

    void GenerateOrders(int count)
    {
        for (int i = 0; i < count; i++)
        {
            KitchenObjectSO newOrder = possiblePotions[Random.Range(0, possiblePotions.Count)];
            orderQueue.Enqueue(newOrder);
        }
    }

    void ShowNextOrder()
    {
        if (isTutorialCompleted && orderQueue.Count > 0)
        {
            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI);
            }

            currentOrder = orderQueue.Dequeue();
            currentOrderUI = CreateOrderUI(currentOrder);

            // Tampilkan UI Resep yang sesuai dengan potion yang dipesan
            if (potionRecipeUIs.ContainsKey(currentOrder))
            {
                if (currentRecipeUI != null) currentRecipeUI.SetActive(false); // Sembunyikan UI sebelumnya
                currentRecipeUI = potionRecipeUIs[currentOrder]; // Ambil UI resep yang sesuai
                currentRecipeUI.SetActive(true); // Tampilkan UI resep potion
            }
        }
        else if (!isTutorialCompleted)
        {
            Debug.Log("Tutorial belum selesai, sistem order tidak aktif.");
        }
        else
        {
            currentOrder = null;
            Debug.Log("All orders completed!");
            OrderSucces.SetActive(true);
            Timer.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("Level1");
                Debug.Log("Game Restarted");
            }
        }
    }

    GameObject CreateOrderUI(KitchenObjectSO order)
    {
        if (orderUIPrefab == null || orderUIParent == null)
        {
            Debug.LogError("Order UI Prefab atau Parent belum diassign di Inspector.");
            return null;
        }

        GameObject orderUI = Instantiate(orderUIPrefab, orderUIParent);
        TextMeshProUGUI orderText = orderUI.GetComponentInChildren<TextMeshProUGUI>();
        if (orderText != null)
        {
            orderText.text = order.name;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is missing in the Order UI Prefab!");
        }

        return orderUI;
    }

    public bool CheckOrder(KitchenObjectSO potion)
    {
        return currentOrder != null && currentOrder == potion;
    }

    public void CompleteOrder(KitchenObjectSO potion)
    {
        if (currentOrder != null && currentOrder == potion)
        {
            Debug.Log("✅ Order completed: " + currentOrder.name);
            currentOrder = null;

            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI);
                currentOrderUI = null;
            }

            // Sembunyikan UI Resep setelah order selesai
            if (currentRecipeUI != null)
            {
                currentRecipeUI.SetActive(false);
                currentRecipeUI = null;
            }

            ShowNextOrder();
        }
        else
        {
            Debug.Log("❌ Potion is not the current order.");
        }
    }
}

// private void AddCoins(int amount)
// {
//     coins += amount;
//     Debug.Log("Coins added: " + amount + ". Total Coins: " + coins);
//     UpdateCoinUI();
// }

// private void UpdateCoinUI()
// {
//     if (coinText != null)
//     {
//         coinText.text = "Coins: " + coins.ToString();
//     }
//     else
//     {
//         Debug.LogError("Coin Text UI is not assigned!");
//     }
// }

// Panggil ini saat pemain berhasil menyajikan potion



