using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public class OrderSystem : MonoBehaviour
{
    public event Action OnAllOrdersCompleted;
    public List<KitchenObjectSO> possibleOrders;
    private Queue<KitchenObjectSO> orderQueue = new Queue<KitchenObjectSO>();
    private Dictionary<KitchenObjectSO, int> serveCounts = new Dictionary<KitchenObjectSO, int>();
    private List<KitchenObjectSO> activeOrders = new List<KitchenObjectSO>();
    private KitchenObjectSO currentOrder;
    [SerializeField] private CountdownTimer countdownTimer;

    public GameObject orderUIPrefab;
    [SerializeField] private GameObject clearUI;
    public Transform orderUIParent;

    [SerializeField] private GameObject[] TimerPanel;

    [SerializeField] private GameObject OrderSuccess;
    [SerializeField] private totalReward totalReward;
    [SerializeField] private GoldSystem goldSystem;

    private GameObject currentOrderUI;

    public int maxOrders;
    private bool isTutorialCompleted = false;

    private void Start()
    {
        StartCoroutine(HideCursorNextFrame());
        if (SceneManager.GetActiveScene().name != "Level-Tutorial")
        {
            StartCoroutine(TimerPanelAnimation());

        }
    }
    IEnumerator HideCursorNextFrame()
    {
        yield return null;  // tunggu 1 frame agar Unity fokus ke game window
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public Dictionary<KitchenObjectSO, int> GetServeStats()
    {
        return serveCounts;
    }
    public List<KitchenObjectSO> GetFailedOrders()
    {
        return new List<KitchenObjectSO>(activeOrders);
    }
    public Dictionary<KitchenObjectSO, int> GetFailedOrderStats()
    {
        Dictionary<KitchenObjectSO, int> failedStats = new Dictionary<KitchenObjectSO, int>();

        foreach (KitchenObjectSO order in activeOrders)
        {
            if (failedStats.ContainsKey(order))
            {
                failedStats[order]++;
            }
            else
            {
                failedStats[order] = 1;
            }
        }

        return failedStats;
    }

    public void StartOrderSystem()
    {
        isTutorialCompleted = true;
        GenerateOrders(maxOrders);
        ShowNextOrder();
    }


    public void AddNewOrder()
    {
        KitchenObjectSO newOrder = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Count)];
        activeOrders.Add(newOrder);
        orderQueue.Enqueue(newOrder);
        Debug.Log("New Order: " + newOrder.objectName);
        Debug.Log("Total Orders: " + orderQueue.Count);
    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int j = rand.Next(i, n);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void GenerateOrders(int count)
    {
        Shuffle(possibleOrders);
        for (int i = 0; i < count; i++)
        {
            AddNewOrder();
        }
    }


    private void ShowNextOrder()
    {
        if (isTutorialCompleted && orderQueue.Count > 0)
        {
            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI);
            }

            currentOrder = orderQueue.Dequeue();
            currentOrderUI = Instantiate(orderUIPrefab, orderUIParent);

            OrderUI orderUIComponent = currentOrderUI.GetComponent<OrderUI>();
            if (orderUIComponent != null)
            {
                orderUIComponent.SetOrder(currentOrder);
            }
        }
        else
        {
            currentOrder = null;
            OrderSuccess.SetActive(true);
            OnAllOrdersCompleted?.Invoke();
            countdownTimer.StopTimer();
            int totalGold = goldSystem.GetcurrentGold();
            totalReward.ShowFinalGold(totalGold);
            totalReward.ShowPotionStats(GetServeStats());
            float remaining = countdownTimer.GetTimeRemaining();
            totalReward.ShowTimeRemaining(remaining);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            clearUI.SetActive(false);

        }
    }

    public bool CheckOrder(KitchenObjectSO item)
    {
        return currentOrder != null && currentOrder == item;
    }

    public void CompleteOrder(KitchenObjectSO item)
    {
        if (CheckOrder(item))
        {
            currentOrder = null;
            if (currentOrderUI != null)
            {
                Destroy(currentOrderUI);
                currentOrderUI = null;
            }
            if (serveCounts.ContainsKey(item))
            {
                serveCounts[item]++;
            }
            else
            {
                serveCounts[item] = 1;
            }
            ShowNextOrder();
            activeOrders.Remove(item);
        }
    }

    //Animation TimerPanel
    private IEnumerator TimerPanelAnimation()
    {
        yield return new WaitForSeconds(1f);
        TimerPanel[0].SetActive(true);
        yield return new WaitForSeconds(2);
        TimerPanel[1].SetActive(true);
        TimerPanel[0].SetActive(false);
        yield return new WaitForSeconds(2);
        TimerPanel[2].SetActive(true);
        TimerPanel[1].SetActive(false);
        yield return new WaitForSeconds(2);
        TimerPanel[2].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        StartOrderSystem();
        countdownTimer.StartTimer();

    }
}
