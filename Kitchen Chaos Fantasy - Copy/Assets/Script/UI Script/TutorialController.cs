using System.Collections;

using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public OrderSystem orderSystem; public Player player;
    public GameObject tutorialUI;
    public GameObject tutorialUI2;
    public GameObject tutorialUI3;
    public GameObject tutorialUI4;
    public GameObject tutorialUI5;
    public GameObject tutorialUI6;
    public GameObject serveTutorial;
    [SerializeField] PotionCreationState potionCreationState;
    [SerializeField] private GameObject[] TimerPanel;
    [SerializeField] private CountdownTimer countdownTimer;

    private bool isPotionServed = false; // Status apakah potion sudah disajikan

    void Start()
    {
        // Pastikan player sudah terhubung sebelum mulai tutorial
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            Debug.Log("Player found in the scene.");

        }

        // Memastikan player dan OrderSystem sudah terhubung dengan benar
        if (player != null && orderSystem != null)
        {
            StartCoroutine(StartTutorial());
        }
        else
        {
            Debug.LogError("Player atau OrderSystem tidak diassign di Inspector!");
        }
    }

    private IEnumerator StartTutorial()
    {
        // Tampilkan UI tutorial langkah demi langkah
        tutorialUI.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(2f);
        tutorialUI.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(2f);

        tutorialUI2.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(2f);
        tutorialUI2.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(2f);

        tutorialUI3.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(2f);
        tutorialUI3.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(2f);

        tutorialUI4.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(2f);
        tutorialUI4.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(5f);


        tutorialUI5.SetActive(true);
        player.enabled = true;
        yield return new WaitForSeconds(3f);
        tutorialUI5.SetActive(false);
        // potionCreationState.StartPotionCreationProcess();
        yield return new WaitUntil(() => isPotionServed == true);
        player.enabled = true;
        tutorialUI6.SetActive(true);
        yield return new WaitForSeconds(3f);
        tutorialUI6.SetActive(false);

        //Timer
        yield return new WaitForSeconds(1f);
        TimerPanel[0].SetActive(true);
        yield return new WaitForSeconds(2);
        TimerPanel[1].SetActive(true);
        TimerPanel[0].SetActive(false);
        yield return new WaitForSeconds(2);
        TimerPanel[2].SetActive(true);
        TimerPanel[1].SetActive(false);
        yield return new WaitForSeconds(2);
        TimerPanel[2].SetActive(false);
        //End Timer
        yield return new WaitForSeconds(0.5f);
        EndTutorial();
    }

    //Finish make potion
    public void OnPotionServed()
    {
        Debug.Log("Healing Potion has been served!");
        isPotionServed = true; // Set status menjadi true
        potionCreationState.stirringHighlight.SetActive(false);

    }

    //Finish Tutorial
    public void EndTutorial()
    {
        Debug.Log("Tutorial ended. Starting Order System...");
        orderSystem.StartOrderSystem();



        if (serveTutorial != null)
        {
            serveTutorial.SetActive(false);
            Debug.Log("ServeTutorial GameObject destroyed.");
        }
        countdownTimer.StartTimer();
    }
}
