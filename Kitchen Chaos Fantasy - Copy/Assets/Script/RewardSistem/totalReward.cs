using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class totalReward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalGoldText;
    [SerializeField] private GameObject statLinePrefab;
    [SerializeField] private Transform statContainer;

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
    public void ShowFinalGold(int totalGold)
    {
        finalGoldText.text = "Total Gold: " + totalGold;
        gameObject.SetActive(true);
    }
}
