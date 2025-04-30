using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DestroyOnGrabContainerCounter : ContainerCounter
{
    [SerializeField] private EventReference ambilRamuanSFX;
    public override void Interact(Player player)
    {
        base.Interact(player);
        if (!player.HasKitchenObject())
        {
            return;

        }
        SFXCounter.PlaySFX(ambilRamuanSFX, player.transform.position);

        Destroy(gameObject);

    }
}
