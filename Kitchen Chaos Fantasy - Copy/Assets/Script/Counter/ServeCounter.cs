using UnityEngine;

public class ServingCounter : BaseCounter {
    private OrderSystem orderSystem;
    private ServeSFX serveSFX;

    private void Start()
    {
        orderSystem = FindObjectOfType<OrderSystem>();
        serveSFX = GetComponent<ServeSFX>();
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
            if (orderSystem.CheckOrder(playerKitchenObjectSO)) {
                Debug.Log("Order served successfully!");
                serveSFX?.PlayAmbilSound();
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
