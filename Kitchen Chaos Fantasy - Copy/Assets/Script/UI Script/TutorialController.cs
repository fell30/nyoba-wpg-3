using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    public OrderSystem orderSystem;
    public Player player;
    public GameObject[] TutorialInteract;
    public GameObject serveTutorial;
    public GameObject ServeMain;
    [SerializeField] PotionCreationState potionCreationState;
    // [SerializeField] private GameObject[] TimerPanel;
    [SerializeField] private GameObject[] perkenalanUIMas;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private TutorialDialog tutorialDialog;
    [SerializeField] private GameObject TimerDanGoldUI;
    private bool isPotionServed = false;
    private enum TutorialInputState
    {
        Start,
        WASD,
        PressP,
        PressO,
    }

    private TutorialInputState currenState = TutorialInputState.Start;

    private void Update()
    {

        switch (currenState)
        {
            case TutorialInputState.WASD:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    currenState = TutorialInputState.PressP;
                    TutorialInteract[0].SetActive(false);
                    TutorialInteract[1].SetActive(true);

                    if (SceneManager.GetActiveScene().name == "Level-tutorial")
                    {
                        player.enabled = false;

                    }


                }
                break;
            case TutorialInputState.PressP:
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.O))

                {
                    currenState = TutorialInputState.PressO;

                    TutorialInteract[1].SetActive(false);
                    StartCoroutine(StartTutorial());




                }
                break;
            case TutorialInputState.PressO:

                currenState = TutorialInputState.Start;

                StartCoroutine(StartTutorial());

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
        player.enabled = false;

        tutorialDialog.ShowMessage("Since we need money to pay for the academy, selling potions is the most efficient way to get it.", "main");
        yield return StartCoroutine(WaitForKeyPressOrDelay(6f));



        tutorialDialog.ShowMessage("There should be ingredients around here that can be used for make potions.", "main");

        yield return StartCoroutine(WaitForKeyPressOrDelay(6f));


        tutorialDialog.ShowMessage(" Let's start making potions and selling them! ", "main");

        yield return StartCoroutine(WaitForKeyPressOrDelay(6f));


        currenState = TutorialInputState.WASD;
        tutorialDialog.HideMessage("main");
        TutorialInteract[0].SetActive(true);
        // tutorialDialog.ShowMessage("Press WASD to Move Around", "hint");
        player.enabled = true;

    }

    private IEnumerator WaitForKeyPressOrDelay(float delayTime)
    {
        float timeElapsed = 0f;

        while (timeElapsed < delayTime)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {

                yield break;
            }
            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator StartTutorial()
    {
        player.enabled = true;
        TutorialInteract[2].SetActive(false);
        tutorialDialog.HideMessage("hint");

        potionCreationState.StartPotionCreationProcess();
        yield return new WaitUntil(() => isPotionServed == true);
        player.enabled = true;

        //Timer
        // yield return new WaitForSeconds(1f);
        // TimerPanel[0].SetActive(true);
        // yield return new WaitForSeconds(2);
        // TimerPanel[1].SetActive(true);
        // TimerPanel[0].SetActive(false);
        // yield return new WaitForSeconds(2);
        // TimerPanel[2].SetActive(true);
        // TimerPanel[1].SetActive(false);
        // yield return new WaitForSeconds(2);
        // TimerPanel[2].SetActive(false);

        //End Timer
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(perkenalanUI());
    }

    //Finish make potion
    public void OnPotionServed()
    {
        Debug.Log("Healing Potion has been served!");
        isPotionServed = true;
        potionCreationState.stirringHighlight.SetActive(false);

    }


    private IEnumerator perkenalanUI()
    {
        TimerDanGoldUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        orderSystem.StartOrderSystem();
        perkenalanUIMas[0].SetActive(true);
        TimerDanGoldUI.SetActive(true);
        // Resep
        tutorialDialog.ShowMessage("Alright, first of all look at the order menu on the top left", "main");
        yield return new WaitForSeconds(4.5f);
        tutorialDialog.ShowMessage("The leftmost part is the potion that must be made", "main");
        yield return new WaitForSeconds(4.5f);
        tutorialDialog.ShowMessage("And to the right is a list of ingredients and steps to follow.", "main");

        yield return new WaitForSeconds(4.5f);
        perkenalanUIMas[0].SetActive(false);
        // Coin
        tutorialDialog.ShowMessage("Next, on the bottom left, there are coins that have been collected from selling potions.", "main");
        perkenalanUIMas[1].SetActive(true);
        yield return new WaitForSeconds(4.5f);
        perkenalanUIMas[1].SetActive(false);
        // Timer
        tutorialDialog.ShowMessage("Lastly, at the bottom right there is a timer, the timer is the time limit set to complete the order.", "main");
        perkenalanUIMas[2].SetActive(true);
        yield return new WaitForSeconds(4.5f);
        tutorialDialog.HideMessage("main");
        perkenalanUIMas[2].SetActive(false);
        EndTutorial();
    }

    //Finish Tutorial
    public void EndTutorial()
    {



        FindObjectOfType<BGMManager>().StartBGM();




        if (serveTutorial != null)
        {
            serveTutorial.SetActive(false);
            ServeMain.SetActive(true);

        }
        countdownTimer.StartTimer();
    }
}
