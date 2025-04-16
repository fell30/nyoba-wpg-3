using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnGrabContainerCounter : ContainerCounter 
{
    public override void Interact(Player player)
    {
        base.Interact(player);

        if (!player.HasKitchenObject())
        {
            return;
        }

        Destroy(gameObject);
    }
}
