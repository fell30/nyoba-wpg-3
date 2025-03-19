using UnityEngine;

public class ServePotionTutorial : BaseCounter
{
    public KitchenObjectSO healingPotionSO; // Referensi ke ScriptableObject HealingPotion
    private TutorialController tutorialController; // Referensi ke TutorialController

    private void Start()
    {
        // Temukan TutorialController di scene
        tutorialController = FindObjectOfType<TutorialController>();

        // Pastikan healingPotionSO sudah dihubungkan di Inspector
        if (healingPotionSO == null)
        {
            Debug.LogError("HealingPotionSO is not assigned! Please assign it in the Inspector.");
        }
    }

    // Metode untuk interaksi utama
    public override void Interact(Player player)
    {
        // Cek apakah player memiliki kitchen object (potion)
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();
            KitchenObjectSO potion = playerObject.GetKitchenObjectSO();

            Debug.Log("Player is trying to serve: " + potion.name);

            // Cek apakah potion yang dibawa player adalah HealingPotion
            if (potion == healingPotionSO)
            {
                Debug.Log("Healing Potion served successfully!");
                playerObject.DestroySelf(); // Hancurkan potion yang telah disajikan

                // Panggil metode OnPotionServed() di TutorialController
                if (tutorialController != null)
                {
                    tutorialController.OnPotionServed();
                    Debug.Log("TutorialController.OnPotionServed() is called.");
                }
                else
                {
                    Debug.LogError("TutorialController not found in the scene.");
                }
            }
            else
            {
                Debug.Log("This is not the correct potion. Only HealingPotion is accepted in the tutorial.");
            }
        }
        else
        {
            Debug.Log("Player is not holding any potion to serve.");
        }
    }
}
