using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class BottleCounter : BaseCounter
{
    public event EventHandler OnBottleSpawned;
    public event EventHandler OnBottleRemoved;
    [SerializeField] private KitchenObjectSO bottleKitchenObjectSO;
    private float spawnBottleTimer;
    private float spawnBottleTimerMax = 4f;
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
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (bottleSpawnAmount > 0)
            {
                bottleSpawnAmount--;
                KitchenObject.SpawnKitchenObject(bottleKitchenObjectSO, player);
                OnBottleRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
