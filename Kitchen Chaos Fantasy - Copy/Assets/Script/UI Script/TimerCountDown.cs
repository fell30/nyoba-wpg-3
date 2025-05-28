using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 60;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    [SerializeField] private GameObject GameOverPanel;


    private bool isTimerSoundPlaying = false;
    private bool isUrgentMusicPlaying = false;
    private Vector3 originalPosition; // Simpan posisi asli teks

    void Start()
    {
        originalPosition = timeText.transform.localPosition; // Simpan posisi awal teks
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {


                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);

                // 🔥 Musik berubah saat timer tersisa ≤ 10 detik
                if (timeRemaining <= 10f && !isUrgentMusicPlaying)
                {

                    isUrgentMusicPlaying = true;
                }

                // 🔥 Timer Sound saat tersisa ≤ 8 detik
                if (timeRemaining <= 30f && !isTimerSoundPlaying)
                {

                    isTimerSoundPlaying = true;
                    GetComponent<SFX_Timer>().PlayTimer();
                }
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                TimerEnd();
            }
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }
    public void StopTimer()
    {
        timerIsRunning = false;
        FindObjectOfType<BGMManager>().StartBGM();
    }

    void UpdateTimerDisplay(float currentTime)
    {
        currentTime = Mathf.Clamp(currentTime, 0, Mathf.Infinity);
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // 🔥 Warna teks berubah saat tersisa ≤ 10 detik
        if (currentTime <= 10)
        {
            timeText.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 2, 1));
        }
        else
        {
            timeText.color = Color.white;
        }

        // 🔥 Efek Getar jika tersisa ≤ 5 detik
        if (currentTime <= 30)
        {
            timeText.transform.DOKill(); // Hentikan animasi sebelumnya agar tidak bertumpuk
            timeText.transform.DOShakeScale(0.3f, 0.07f, 10, 90, false).SetLoops(-1, LoopType.Restart);

        }
        else
        {
            timeText.transform.DOKill();
            timeText.transform.localPosition = originalPosition; // Reset posisi ke awal
        }
    }

    void TimerEnd()
    {
        Debug.Log("Timer finished! Execute any end of timer logic here.");
        GameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pause game saat waktu habis
        FindAnyObjectByType<BGMManager>().StopBGM();


        // Hentikan suara timer
        if (isTimerSoundPlaying)
        {
            AudioEventSystem.StopAudio("TimerSound");
            isTimerSoundPlaying = false;
        }

        // Hentikan efek getar
        timeText.transform.DOKill();
        timeText.transform.localPosition = originalPosition;
    }
}
