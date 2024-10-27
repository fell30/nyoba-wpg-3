
using UnityEngine;

public class PotionCreationState : MonoBehaviour
{
    public Player player;
    public TutorialController tutorialController; // Referensi ke TutorialController

    public GameObject leafTreeHighlight; // Highlight untuk LeafLife Tree
    public GameObject mortarHighlight; // Highlight untuk Mortar and Pestle
    public GameObject barrelHighlight; // Highlight untuk Mushroom barrel
    public GameObject cauldronHighlight; // Highlight untuk Cauldron
    public GameObject stirringHighlight; // Highlight untuk proses mengaduk potion

    [SerializeField] private CauldronCounter cauldronCounter; // Referensi ke CauldronCounter

    private enum PotionState
    {
        Start,
        TakeLeafLife,
        UseMortar,
        AddCauldron,
        TakeMushroom,
        MixPotion,
        ServePotion,
        Complete
    }

    private PotionState currentState = PotionState.Start;



    public void StartPotionCreationProcess()
    {
        currentState = PotionState.TakeLeafLife;
        leafTreeHighlight.SetActive(true); // Tampilkan highlight untuk LeafLife Tree
    }

    void Update()
    {
        switch (currentState)
        {
            case PotionState.TakeLeafLife:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "Tomato")
                {
                    leafTreeHighlight.SetActive(false);
                    mortarHighlight.SetActive(true); // Tampilkan highlight untuk Mortar
                    currentState = PotionState.UseMortar;
                }
                break;

            case PotionState.UseMortar:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "LeafLifePowder")
                {
                    mortarHighlight.SetActive(false);
                    cauldronHighlight.SetActive(true); // Tampilkan highlight untuk Cauldron
                    currentState = PotionState.AddCauldron;
                }
                break;
            case PotionState.AddCauldron:
                // Cek apakah bahan pertama yang dimasukkan adalah LeafLifePowder
                if (cauldronCounter.GetIngredient1() != null)
                {
                    Debug.Log("Ingredient 1: " + cauldronCounter.GetIngredient1().GetKitchenObjectSO().name);
                    if (cauldronCounter.GetIngredient1().GetKitchenObjectSO().name == "LeafLifePowder")
                    {
                        barrelHighlight.SetActive(true);
                        cauldronHighlight.SetActive(false); // Tampilkan highlight untuk Cauldron

                        // Ubah state
                        currentState = PotionState.TakeMushroom;
                        Debug.Log("State changed to: " + currentState.ToString());
                    }
                }
                break;


            case PotionState.TakeMushroom:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "TwistingMushroom")
                {
                    cauldronHighlight.SetActive(true); // Tampilkan highlight untuk Cauldron
                    barrelHighlight.SetActive(false);

                    currentState = PotionState.MixPotion;
                }
                break;

            case PotionState.MixPotion:
                // Cek apakah pemain mengaduk potion di Cauldron
                if (player.HasKitchenObject())
                {
                    Debug.Log("Object held by player: " + player.GetKitchenObject().GetKitchenObjectSO().name); // Cek nama objek

                    if (player.GetKitchenObject().GetKitchenObjectSO().name == "HealingPotion")
                    {
                        cauldronHighlight.SetActive(false);
                        stirringHighlight.SetActive(true); // Tampilkan highlight untuk mengaduk potion
                        currentState = PotionState.ServePotion; // Pindah ke state ServePotion
                        Debug.Log("State changed to ServePotion");
                    }
                }
                break;


            case PotionState.ServePotion:
                if (player.GetKitchenObject())
                {
                    Debug.Log("Serve Potion");
                    currentState = PotionState.Complete; // Tandai proses selesai

                }
                break;

            case PotionState.Complete:
                // Proses pembuatan potion sudah selesai

                break;
        }
    }
}
