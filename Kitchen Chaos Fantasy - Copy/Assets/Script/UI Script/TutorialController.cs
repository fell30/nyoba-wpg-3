using System.Collections;
//using System.Diagnostics;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class TutorialController : MonoBehaviour
{
    public OrderSystem orderSystem;
    public Player player;
    public GameObject tutorialUI;
    public GameObject tutorialUI2;
    public GameObject tutorialUI3;
    public GameObject tutorialUI4;
    public GameObject tutorialUI5;
    public GameObject tutorialUI6;
    public GameObject serveTutorial;
    public GameObject ServeMain;
    [SerializeField] PotionCreationState potionCreationState;
    [SerializeField] private GameObject[] TimerPanel;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private TutorialDialog tutorialDialog;

    private bool isPotionServed = false;

    private enum TutorialInputState
    {
        WASD,
        PressP,
        PressO,
    }

    private TutorialInputState currenState = TutorialInputState.WASD;

    private void Update()
    {
        switch (currenState)
        {
            case TutorialInputState.WASD:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    currenState = TutorialInputState.PressP;
                }
                break;
            case TutorialInputState.PressP:
                if (Input.GetKeyDown(KeyCode.P))
                {
                    currenState = TutorialInputState.PressO;
                }
                break;
            case TutorialInputState.PressO:
                if (Input.GetKeyDown(KeyCode.O))
                {
                    currenState = TutorialInputState.WASD;
                }
                break;
        }
    }

    void Start()
    {

        if (player == null)
        {
            player = FindObjectOfType<Player>();
            Debug.Log("Player found in the scene.");

        }


        if (player != null && orderSystem != null)
        {
            StartCoroutine(IntroDialog());
        }
        else
        {
            Debug.LogError("Player atau OrderSystem tidak diassign di Inspector!");
        }
    }
    private IEnumerator IntroDialog()
    {
        tutorialDialog.ShowMessage("Aku membutuhkan uang untuk membayar pendidikanku.", "main");
        yield return new WaitForSeconds(6f);
        tutorialDialog.HideMessageDelayed("main", 8f);
        tutorialDialog.ShowMessage("Karena KIPK sudah dihapus demi efisiensi, potion adalah cara paling efisien untuk mendapatkannya. ", "main");
        yield return new WaitForSeconds(8.5f);
        tutorialDialog.HideMessageDelayed("main", 8f);
        tutorialDialog.ShowMessage("Seharusnya di sekitar sini ada bahan-bahan yang dapat dijadikan untuk potion. Ayo kita mulai untuk membuat potion dan menjualnya!", "main");
        yield return new WaitForSeconds(8.5f);
        tutorialDialog.HideMessageDelayed("main", 3f);

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
        yield return new WaitForSeconds(1f);
        tutorialUI2.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(1f);

        tutorialUI3.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(1f);
        tutorialUI3.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(1f);

        tutorialUI4.SetActive(true);
        player.enabled = false;
        yield return new WaitForSeconds(1f);
        tutorialUI4.SetActive(false);
        player.enabled = true;

        yield return new WaitForSeconds(1f);


        tutorialUI5.SetActive(true);
        player.enabled = true;
        yield return new WaitForSeconds(1f);
        tutorialUI5.SetActive(false);
        potionCreationState.StartPotionCreationProcess();
        yield return new WaitUntil(() => isPotionServed == true);
        player.enabled = true;
        tutorialUI6.SetActive(true);
        yield return new WaitForSeconds(1f);
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
            ServeMain.SetActive(true);
            Debug.Log("ServeTutorial GameObject destroyed.");
        }
        countdownTimer.StartTimer();
    }
}
