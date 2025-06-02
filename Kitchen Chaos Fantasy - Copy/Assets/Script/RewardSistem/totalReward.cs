using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class totalReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalGoldText;
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
    public void ShowFinalGold(int totalGold)
    {
        finalGoldText.text = "Total Gold: " + totalGold;
        gameObject.SetActive(true);
    }
}
