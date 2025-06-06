using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class totalReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalGoldText;
    //[SerializeField] private TextMeshProUGUI totalServedText;
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private TextMeshProUGUI failedOrderCountText;

    [SerializeField] private GameObject statLinePrefab;
    [SerializeField] private GameObject failedLinePrefab;
    [SerializeField] private Transform statContainer;
    [SerializeField] private Transform failedContainer;


    public void ShowPotionStats(Dictionary<KitchenObjectSO, int> serveStats)
    {
        foreach (Transform child in statContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var entry in serveStats)
        {
            KitchenObjectSO potion = entry.Key;
            int count = entry.Value;
            GameObject line = Instantiate(statLinePrefab, statContainer);
            line.GetComponent<PotionStatLineUI>().Setup(potion, count);
        }
    }

    public void ShowGameOver(Dictionary<KitchenObjectSO, int> serveStats)
    {
        foreach (Transform child in failedContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var entry in serveStats)
        {
            KitchenObjectSO potion = entry.Key;
            int count = entry.Value;
            GameObject line = Instantiate(failedLinePrefab, failedContainer);
            line.GetComponent<PotionFailed>().Setup(potion, count);
        }
    }
    public void ShowFailedOrders(Dictionary<KitchenObjectSO, int> failedOrders)
    {
        foreach (Transform child in failedContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in failedOrders)
        {
            KitchenObjectSO potion = entry.Key;
            int count = entry.Value;

            GameObject line = Instantiate(failedLinePrefab, failedContainer);
            line.GetComponent<PotionFailed>().Setup(potion, count);
        }

    }
    public void ShowFailedOrderCount(List<KitchenObjectSO> failedOrders)
    {
        int failedCount = failedOrders.Count;
        failedOrderCountText.text = failedCount.ToString();
    }
    public void ShowTimeRemaining(float timeLeft)
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timeRemainingText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void ShowFinalGold(int totalGold)
    {
        finalGoldText.text = "Total Gold: " + totalGold;
        gameObject.SetActive(true);
    }
    // public void ShowTotalServed(Dictionary<KitchenObjectSO, int> serveStats)
    // {
    //     int totalServed = 0;

    //     foreach (var entry in serveStats)
    //     {
    //         totalServed += entry.Value;
    //     }

    //     totalServedText.text = totalServed.ToString();
    // }
}
