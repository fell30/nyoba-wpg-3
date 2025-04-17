using System.Collections;
//using System.Diagnostics;
using UnityEngine;
//using UnityEngine.InputSystem.iOS;

public class TutorialController : MonoBehaviour {
    public OrderSystem orderSystem;
    public Player player;
    // public GameObject tutorialUI;
    // public GameObject tutorialUI2;
    // public GameObject tutorialUI3;
    // public GameObject tutorialUI4;
    // public GameObject tutorialUI5;
    // public GameObject tutorialUI6;
    public GameObject serveTutorial;
    public GameObject ServeMain;
    [SerializeField] PotionCreationState potionCreationState;
    [SerializeField] private GameObject[] TimerPanel;
    [SerializeField] private CountdownTimer countdownTimer;
    [SerializeField] private TutorialDialog tutorialDialog;

    private bool isPotionServed = false;

    private enum TutorialInputState {
        Start,
        WASD,
        PressP,
        PressO,
    }

    private TutorialInputState currenState = TutorialInputState.Start;

    private void Update() {
        switch (currenState) {
            case TutorialInputState.WASD:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
                    currenState = TutorialInputState.PressP;
                    player.enabled = false;

                    tutorialDialog.ShowMessage("Tekan P untuk membuka menu potion.", "hint");
                }
                break;
            case TutorialInputState.PressP:
                if (Input.GetKeyDown(KeyCode.P)) {
                    currenState = TutorialInputState.PressO;

                    tutorialDialog.ShowMessage("Press O For Action", "hint");

                }
                break;
            case TutorialInputState.PressO:
                if (Input.GetKeyDown(KeyCode.O)) {
                    currenState = TutorialInputState.Start; // Tambahkan ini agar tidak terus-menerus memulai tutorial
                    StartCoroutine(StartTutorial());
                }
                break;

        }
    }

    void Start() {

        if (player == null) {
            player = FindObjectOfType<Player>();
            Debug.Log("Player found in the scene.");


        }


        if (player != null && orderSystem != null) {
            StartCoroutine(IntroDialog());
        } else {
            Debug.LogError("Player atau OrderSystem tidak diassign di Inspector!");
        }
    }
    private bool isSkipping = false; // Menandai apakah dialog sedang di-skip
    private IEnumerator IntroDialog() {
        player.enabled = false;
        isSkipping = false; // Reset setiap kali dialog dimulai

        // player.enabled = false;
        yield return StartCoroutine(ShowDialogWithSkip("Aku membutuhkan uang untuk membayar pendidikanku.", 6f));
        yield return StartCoroutine(ShowDialogWithSkip("Karena KIPK sudah dihapus demi efisiensi, potion adalah cara paling efisien untuk mendapatkannya.", 8.5f));
        yield return StartCoroutine(ShowDialogWithSkip("Seharusnya di sekitar sini ada bahan-bahan yang dapat dijadikan untuk potion. Ayo kita mulai untuk membuat potion dan menjualnya!", 8.5f));

        currenState = TutorialInputState.WASD;
        tutorialDialog.HideMessage("main");
        tutorialDialog.ShowMessage("Press WASD to Move Around", "hint");
        player.enabled = true;

    }
    // Fungsi untuk menampilkan dialog dengan opsi skip
    private IEnumerator ShowDialogWithSkip(string message, float duration) {
        tutorialDialog.ShowMessage(message, "main");

        float timer = 0f;
        while (timer < duration) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                isSkipping = true;
                break; // Skip langsung ke dialog berikutnya
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!isSkipping) {
            yield return new WaitForSeconds(duration - timer); // Tunggu sisa durasi jika tidak di-skip
        }
    }



    private IEnumerator StartTutorial() {
        player.enabled = true;
        tutorialDialog.HideMessage("hint");
        potionCreationState.StartPotionCreationProcess();
        yield return new WaitUntil(() => isPotionServed == true);
        player.enabled = true;
        //    // tutorialUI6.SetActive(true);
        //     yield return new WaitForSeconds(1f);
        //     tutorialUI6.SetActive(false);

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
    public void OnPotionServed() {
        Debug.Log("Healing Potion has been served!");
        isPotionServed = true; // Set status menjadi true
        potionCreationState.stirringHighlight.SetActive(false);

    }

    //Finish Tutorial
    public void EndTutorial() {
        Debug.Log("Tutorial ended. Starting Order System...");
        orderSystem.StartOrderSystem();

        // Panggil AudioManager untuk memulai BGM
        FindObjectOfType<BGMManager>().StartBGM();

        if (serveTutorial != null) {
            serveTutorial.SetActive(false);
            ServeMain.SetActive(true);
            Debug.Log("ServeTutorial GameObject destroyed.");
        }
        countdownTimer.StartTimer();
    }
}
