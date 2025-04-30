using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public event Action OnAllOrdersCompleted;
    public List<KitchenObjectSO> possibleOrders;
    private Queue<KitchenObjectSO> orderQueue = new Queue<KitchenObjectSO>();
    private KitchenObjectSO currentOrder;
    [SerializeField] private CountdownTimer countdownTimer;

    public GameObject orderUIPrefab;
    public Transform orderUIParent;

    [SerializeField] private GameObject OrderSuccess;
    private GameObject currentOrderUI;

    private int maxOrders = 2;
    private bool isTutorialCompleted = false;

    public void StartOrderSystem()
    {
        isTutorialCompleted = true;
        GenerateOrders(maxOrders);
        ShowNextOrder();
    }

    public void AddNewOrder()
    {
        KitchenObjectSO newOrder = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Count)];
        orderQueue.Enqueue(newOrder);
        Debug.Log("New Order: " + newOrder.objectName);
    }

    private void GenerateOrders(int count)
    {
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
            ShowNextOrder();
        }
    }
}
