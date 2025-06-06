using UnityEngine;

public class ServingCounter : BaseCounter
{
    private OrderSystem orderSystem;
    private GoldSystem goldSystem;

    private void Start()
    {
        orderSystem = FindObjectOfType<OrderSystem>();
        goldSystem = FindAnyObjectByType<GoldSystem>();
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


            if (orderSystem.CheckOrder(playerKitchenObjectSO))
            {
                if (goldSystem != null)
                {
                    int reward = playerKitchenObjectSO.GoldReward;
                    goldSystem.AddGold(reward);
                    Debug.Log("Order served successfully!");
                }
                GetComponent<WellAudio>()?.PlayTaruhSound();
                playerObject.DestroySelf();
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
