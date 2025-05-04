using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using System.Runtime.InteropServices;
using UnityEngine;


public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;





    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                GetComponent<WellAudio>()?.PlayAmbilSound();
            }
            else
            {

            }
        }
        else
        {
            if (player.HasKitchenObject())
            {

            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                GetComponent<WellAudio>()?.PlayTaruhSound();
            }
        }

    }




}
