using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;


public class PotionCreationState : MonoBehaviour
{
    public Player player;
    public TutorialController tutorialController;
    public TutorialDialog tutorialDialog;

    public GameObject WaterBucketHighlight;
    public GameObject leafTreeHighlight;
    public GameObject mortarHighlight;
    public GameObject barrelHighlight;
    public GameObject cauldronHighlight;
    public GameObject stirringHighlight;


    //COBA
    // [SerializeField] private GameObject OrderLightPotion_Tutor;
    //[SerializeField] private GameObject OrderHealingPotion_Tutor;

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



    public void StartPotionCreationProcess()
    {
        currentState = PotionState.TakeWaterBucket;
        WaterBucketHighlight.SetActive(true);
        tutorialDialog.ShowMessage("Quick!, Get some water from the well", "main");
        // OrderLightPotion_Tutor.SetActive(true);

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
                    //tutorialDialog.HideMessageDelayed("main", 4f);
                    tutorialDialog.ShowMessage("Good! Next, fill the furnace with water.", "npc");
                }

                break;

            case PotionState.AddWaterBucketToCauldron:
                // Debug.Log("Checking if water is added in PotionCreationState...");
                if (cauldronCounter.IsWaterAdded())
                {

                    leafTreeHighlight.SetActive(true);
                    cauldronHighlight.SetActive(false);
                    currentState = PotionState.TakeLeafLife; // Ubah state setelah air ditambahkan
                                                             // tutorialDialog.HideMessageDelayed("npc", 7f);
                    tutorialDialog.ShowMessage("The water is ready! Now take the Torch Flower in the flowerpot.", "npc");
                }
                else
                {
                    // Debug.Log("Water not yet added.");
                }
                break;

            case PotionState.TakeLeafLife:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "LeafLife")
                {
                    leafTreeHighlight.SetActive(false);
                    mortarHighlight.SetActive(true); // Tampilkan highlight untuk Mortar
                    currentState = PotionState.UseMortar;
                    // tutorialDialog.HideMessageDelayed("main", 7f);
                    tutorialDialog.ShowMessage("Puree the Torch Flower using a mortar and pestle.", "npc");
                }
                break;

            case PotionState.UseMortar:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "LeafLifePowder")
                {
                    mortarHighlight.SetActive(false);
                    cauldronHighlight.SetActive(true); // Tampilkan highlight untuk Cauldron
                    currentState = PotionState.AddCauldron;
                    // tutorialDialog.HideMessageDelayed("npc", 7f);
                    tutorialDialog.ShowMessage("Put Twisting Mushroom into the furnace.", "npc");
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
                        //Debug.Log("State changed to: " + currentState.ToString());
                        // tutorialDialog.HideMessageDelayed("main", 7f);
                        tutorialDialog.ShowMessage("Great! Now take the Torch Flower and put it into the furnace.", "npc");
                    }
                }
                break;


            case PotionState.TakeMushroom:
                if (player.HasKitchenObject() && player.GetKitchenObject().GetKitchenObjectSO().name == "TwistingMushroom")
                {
                    cauldronHighlight.SetActive(true); // Tampilkan highlight untuk Cauldron
                    barrelHighlight.SetActive(false);

                    currentState = PotionState.MixPotion;
                    // tutorialDialog.HideMessageDelayed("npc", 7f);
                    tutorialDialog.ShowMessage("Stir all ingredients until well combined.", "main");
                }
                break;

            case PotionState.MixPotion:
                // Cek apakah pemain mengaduk potion di Cauldron
                if (player.HasKitchenObject())
                {
                    //Debug.Log("Object held by player: " + player.GetKitchenObject().GetKitchenObjectSO().name); // Cek nama objek

                    if (player.GetKitchenObject().GetKitchenObjectSO().name == "HealingPotion")
                    {
                        cauldronHighlight.SetActive(false);
                        stirringHighlight.SetActive(true); // Tampilkan highlight untuk mengaduk potion
                        currentState = PotionState.ServePotion; // Pindah ke state ServePotion
                        Debug.Log("State changed to ServePotion");
                        //  tutorialDialog.HideMessageDelayed("main", 7f);
                        tutorialDialog.ShowMessage("Take the Light Potion and serve it!", "main");
                    }
                }
                break;


            case PotionState.ServePotion:
                if (player.GetKitchenObject())
                {
                    Debug.Log("Serve Potion");
                    currentState = PotionState.Complete;
                    //tutorialDialog.HideMessageDelayed("npc", 4f);
                    tutorialDialog.ShowMessage("Bring the order to the table, and get to work!", "main");
                    tutorialDialog.HideMessageDelayed("main", 4f);
                    //OrderLightPotion_Tutor.SetActive(false);
                    //OrderHealingPotion_Tutor.SetActive(true);
                    player.enabled = true;

                }
                break;

            case PotionState.Complete:
                // Proses pembuatan potion sudah selesai



                break;
        }
    }


}
