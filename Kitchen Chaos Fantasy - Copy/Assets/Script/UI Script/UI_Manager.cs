using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject[] Koin;
    public TextMeshProUGUI teksSkor;
    public GameObject SuccesCoin;


    private int skor = 0; // Variabel untuk melacak skor

    public void PauseGame()
    {
        Time.timeScale = 0;
        menuPause.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuPause.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        menuPause.SetActive(false);
        SceneManager.LoadScene("Dami");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        menuPause.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Next_Collect()
    {
        StartCoroutine(KoleksiKoin());
    }

    private IEnumerator KoleksiKoin()
    {
        for (int i = 0; i < Koin.Length; i++)
        {
            Koin[i].SetActive(true);

            // Tambah skor dan perbarui teks skor
            skor++;
            teksSkor.text = "Coin: " + skor.ToString();

            yield return new WaitForSeconds(0.7f);
            Koin[i].SetActive(false);
        }
        SuccesCoin.SetActive(true);
    }
}
