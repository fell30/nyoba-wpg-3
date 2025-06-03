using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldSystem : MonoBehaviour
{
    public int currentGold { get; private set; }
    public Gold_UI goldUI;
    public void AddGold(int amount)
    {
        currentGold += amount;


        if (goldUI != null)
        {
            goldUI.UpdateGoldText(currentGold);
            Debug.Log("[GoldSystem] Gold UI updated. Current Gold: " + currentGold);

        }

    }
    public int GetcurrentGold()
    {
        return currentGold;
    }
}
