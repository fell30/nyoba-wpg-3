
using UnityEngine;

public class PotionCreationState : MonoBehaviour
{
    public Player player;
    public TutorialController tutorialController;

    public GameObject WaterBucketHighlight;
    public GameObject leafTreeHighlight;
    public GameObject mortarHighlight;
    public GameObject barrelHighlight;
    public GameObject cauldronHighlight;
    public GameObject stirringHighlight;

    [SerializeField] private CauldronCounter cauldronCounter;

    private enum PotionState
    {
        Start,
        TakeWaterBucket,
        AddWaterBucketToCauldron,
        TakeLeafLife,
        UseMortar,
        AddCauldron,
        TakeMushroom,
        MixPotion,
        ServePotion,
        Complete
    }

    private PotionState currentState = PotionState.Start;



    void Start()
    {
        currentState = PotionState.TakeWaterBucket;
        WaterBucketHighlight.SetActive(true);

    }

    void Update()
    {
        switch (currentState)
        {

            case PotionState.TakeWaterBucket:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "Watter_Bucket")
                {
                    WaterBucketHighlight.SetActive(false);
                    cauldronHighlight.SetActive(true);
                    currentState = PotionState.AddWaterBucketToCauldron;
                }

                break;

            case PotionState.AddWaterBucketToCauldron:
                Debug.Log("Checking if water is added in PotionCreationState...");
                if (cauldronCounter.IsWaterAdded())
                {
                    Debug.Log("Water has been added to the cauldron.");
                    leafTreeHighlight.SetActive(true);
                    cauldronHighlight.SetActive(false);
                    currentState = PotionState.TakeLeafLife; // Ubah state setelah air ditambahkan
                }
                else
                {
                    Debug.Log("Water not yet added.");
                }
                break;

            case PotionState.TakeLeafLife:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "LeafLife")
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
