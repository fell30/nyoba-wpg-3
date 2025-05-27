using System.Collections;
using FMODUnity;
using UnityEngine;

public class DestroyOnGrabContainerCounter : ContainerCounter
{
    [SerializeField] private EventReference ambilRamuanSFX;
    [SerializeField] private GameObject DestroyEfek;
    [SerializeField] private GameObject mushroomObject; // referensi ke visual jamur

    private RespawnableMushroom respawner;

    private void Awake()
    {
        respawner = GetComponent<RespawnableMushroom>();
    }

    public override void Interact(Player player)
    {
        base.Interact(player);

        if (!player.HasKitchenObject())
        {
            return;
        }

        SFXCounter.PlaySFX(ambilRamuanSFX, player.transform.position);

        if (DestroyEfek != null)
        {
            GameObject efekInstance = Instantiate(DestroyEfek, transform.position, Quaternion.identity);
            Destroy(efekInstance, 0.5f);
        }

        // Nonaktifkan visual jamurnya saja
        if (mushroomObject != null)
        {
            mushroomObject.SetActive(false);
        }

        // Panggil respawn
        if (respawner != null)
        {
            respawner.RespawnMushroom();
        }
    }
}
