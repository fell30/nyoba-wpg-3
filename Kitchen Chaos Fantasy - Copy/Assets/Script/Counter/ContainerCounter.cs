using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnGrabbedKitchenObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {

        // Jika kitchenObject belum ada, instantiate baru
        if (!player.HasKitchenObject())
        {
            // Jika kitchenObject belum ada, instantiate baru
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnGrabbedKitchenObject?.Invoke(this, EventArgs.Empty);
            GetComponent<WellAudio>()?.PlayAmbilSound();

        }


    }
}
