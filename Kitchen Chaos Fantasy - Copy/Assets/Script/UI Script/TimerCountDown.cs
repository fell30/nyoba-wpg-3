using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 60;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    [SerializeField] private GameObject GameOverPanel;

    private bool isTimerSoundPlaying = false;

    void Start()
    {

    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);

                // Periksa apakah waktu tersisa kurang dari atau sama dengan 8 detik
                if (timeRemaining <= 8f && !isTimerSoundPlaying)
                {

                    AudioEventSystem.PlayAudio("TimerSound");
                    isTimerSoundPlaying = true;
                }
            }
            else
            {
                Debug.Log("Time has run out!");
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

    void UpdateTimerDisplay(float currentTime)
    {
        currentTime = Mathf.Clamp(currentTime, 0, Mathf.Infinity);
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnd()
    {
        Debug.Log("Timer finished! Execute any end of timer logic here.");
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;


        if (isTimerSoundPlaying)
        {
            AudioEventSystem.StopAudio("TimerSound");
            isTimerSoundPlaying = false;
        }
    }
}
