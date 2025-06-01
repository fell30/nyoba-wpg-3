using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionStatLineUI : MonoBehaviour
{
    [SerializeField] private Image potionImage;
    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI earnedGoldPotionText;

    public void Setup(KitchenObjectSO potionSO, int count)
    {
        potionImage.sprite = potionSO.recipeSprite;
        potionNameText.text = potionSO.objectName;
        countText.text = "x" + count;

        int totalGold = potionSO.GoldReward * count;
        earnedGoldPotionText.text = "🪙 " + totalGold;
    }
}
