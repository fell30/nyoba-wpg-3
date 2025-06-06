using System.Collections;
using FMODUnity;
using UnityEngine;

public class DestroyOnGrabContainerCounter : ContainerCounter
{
    [SerializeField] private EventReference ambilRamuanSFX;
    [SerializeField] private GameObject DestroyEfek;



    public override void Interact(Player player)
    {


        if (!player.HasKitchenObject())
        {
            base.Interact(player);
            SFXCounter.PlaySFX(ambilRamuanSFX, player.transform.position);


            if (DestroyEfek != null)
            {
                GameObject efekInstance = Instantiate(DestroyEfek, transform.position, Quaternion.identity);
                Destroy(efekInstance, 0.5f);
                Destroy(gameObject);

            }



        }

    }
}
