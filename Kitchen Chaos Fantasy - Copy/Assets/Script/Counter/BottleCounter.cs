using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCounter : BaseCounter
{
    public event EventHandler OnBottleSpawned;
    [SerializeField] private KitchenObjectSO bottleKitchenObjectSO;
    private float spawnBottleTimer;
    private float spawnBottleTimerMax;
    private int bottleSpawnAmount;
    private int bottleSpawnAmountMax = 4;

    private void Update()
    {
        spawnBottleTimer += Time.deltaTime;
        if (spawnBottleTimer > spawnBottleTimerMax)
        {
            spawnBottleTimer = 0f;
            if (bottleSpawnAmount < bottleSpawnAmountMax)
            {

                bottleSpawnAmount++;
                OnBottleSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
