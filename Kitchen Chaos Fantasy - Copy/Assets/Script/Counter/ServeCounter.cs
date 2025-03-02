using UnityEngine;

public class ServingCounter : BaseCounter
{
    private OrderSystem orderSystem;

    private void Start()
    {
        orderSystem = FindObjectOfType<OrderSystem>();
    }

    // Metode untuk interaksi utama
    public override void Interact(Player player)
    {
        // Cek apakah player memiliki kitchen object (potion)
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();
            Debug.Log("Player is trying to serve: " + playerObject.GetKitchenObjectSO().name);

            // Cek apakah potion yang dibawa player cocok dengan order
            if (orderSystem.CheckOrder(playerObject.GetKitchenObjectSO()))
            {
                Debug.Log("Order served successfully!");
                playerObject.DestroySelf(); // Hancurkan potion yang telah disajikan
                orderSystem.CompleteOrder(playerObject.GetKitchenObjectSO()); // Tandai order sebagai selesai

                // Panggil metode OnPotionServed() di TutorialController
                FindObjectOfType<TutorialController>().OnPotionServed(); // Pastikan metode ini dipanggil setelah potion disajikan
                AudioEventSystem.PlayAudio("Serve");
            }
            else
            {
                Debug.Log("This potion is not ordered.");
            }
        }
        else
        {
            Debug.Log("Player is not holding any potion to serve.");
        }
    }
}
