using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] VisualGameObjectArray;
    private void Start()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player.Instance is null!");
            return;
        }
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        // Debug.Log("Selected counter changed: " + (e.selectedCounter != null ? e.selectedCounter.name : "null"));

        if (e.selectedCounter == baseCounter)
        {
            ShowSelectedCounterVisual();
        }
        else
        {
            HideSelectedCounterVisual();
        }
    }


    private void ShowSelectedCounterVisual()
    {
        foreach (GameObject visual in VisualGameObjectArray)
        {
            visual.SetActive(true);
        }

    }
    private void HideSelectedCounterVisual()
    {
        foreach (GameObject visual in VisualGameObjectArray)
        {
            visual.SetActive(false);
        }
    }
}
