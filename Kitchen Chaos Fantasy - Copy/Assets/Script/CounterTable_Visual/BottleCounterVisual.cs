using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCounterVisual : MonoBehaviour
{
    [SerializeField] private BottleCounter bottleCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform bottleVisualPrefarb;

    private void Start()
    {
        bottleCounter.OnBottleSpawned += BottleCounter_OnBottleSpawned;
    }
    private void BottleCounter_OnBottleSpawned(object sender, System.EventArgs e)
    {
        Transform bottleVisualTransform = Instantiate(bottleVisualPrefarb, counterTopPoint);

    }
}
