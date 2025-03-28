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
        // Cek apakah player memiliki kitchen object
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();
            KitchenObjectSO playerKitchenObjectSO = playerObject.GetKitchenObjectSO();

            Debug.Log("Player is trying to serve: " + playerKitchenObjectSO.name);

            // Cek apakah objek yang dipegang sesuai dengan order yang ada
            if (orderSystem.CheckOrder(playerKitchenObjectSO))
            {
                Debug.Log("Order served successfully!");
                playerObject.DestroySelf(); // Hancurkan objek yang disajikan
                orderSystem.CompleteOrder(playerKitchenObjectSO);
            }
            else
            {
                Debug.Log("This item is not ordered.");
            }
        }
        else
        {
            Debug.Log("Player is not holding anything.");
        }
    }
}
