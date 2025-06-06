using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionFailed : MonoBehaviour
{
    // [SerializeField] private Image icon;
    // [SerializeField] private TextMeshProUGUI nameText;
    // [SerializeField] private TextMeshProUGUI countText;


    // public void Setup(KitchenObjectSO so, int count)
    // {
    //     icon.sprite = so.IconObject;
    //     nameText.text = so.objectName;
    //     countText.text = "x" + count;
    // }
    [SerializeField] private Image potionImage;
    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI earnedGoldPotionText;
    public void Setup(KitchenObjectSO potionSO, int count)
    {
        potionImage.sprite = potionSO.IconObject;
        potionNameText.text = potionSO.objectName;
        countText.text = "x" + count;

        int totalGold = potionSO.GoldReward * count;
        earnedGoldPotionText.text = totalGold.ToString();
    }

}
