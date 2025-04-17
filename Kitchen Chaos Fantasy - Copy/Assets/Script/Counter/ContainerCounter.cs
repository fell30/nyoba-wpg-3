using FMODUnity;
using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    public event System.EventHandler OnGrabbedKitchenObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private EventReference ambilRamuanSFX;

    public override void Interact(Player player) {

        // Jika kitchenObject belum ada, instantiate baru
        if (!player.HasKitchenObject()) {
            // Jika kitchenObject belum ada, instantiate baru
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnGrabbedKitchenObject?.Invoke(this, EventArgs.Empty);

            SFXCounter.PlaySFX(ambilRamuanSFX,player.transform.position);



        }


    }
}
