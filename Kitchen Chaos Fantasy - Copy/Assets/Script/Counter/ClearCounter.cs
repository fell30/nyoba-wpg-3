using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using UnityEngine;


public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private CounterSFXPlayer sfxPlayer;

    private void Awake() {
        sfxPlayer = GetComponentInChildren<CounterSFXPlayer>();
    }



    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);

                sfxPlayer?.PlayTaruhSound();
            } else {

            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);

                // Mainkan suara ambil
                sfxPlayer?.PlayAmbilSound();
            }
        }
    }

}
