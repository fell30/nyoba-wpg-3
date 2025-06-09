using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject transisiOut;
    public Button ResumeButton;
    [SerializeField] private SfxMortar buttonSound;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void RestartGame()
    {
        StartCoroutine(Restart());
        if (buttonSound != null)
        {
            buttonSound.PlayTaruhSound();
        }
    }
    private IEnumerator Restart()
    {
        if (transisiOut != null)
        {
            transisiOut.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            FindAnyObjectByType<BGMManager>().StopBGM();
            if (SceneManager.GetActiveScene().name == "Level-2")
            {
                FindAnyObjectByType<bgm_Level_Selection>().StopBGM();
            }


            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Game Restarted");

        }
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        buttonSound.PlayTaruhSound();


        EventSystem.current.SetSelectedGameObject(null);

        if (ResumeButton != null)
        {
            Animator anim = ResumeButton.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Update(0f);
                anim.Play("Normal", 0);
            }
        }
    }
    public void BacktoMainMenu()
    {
        StartCoroutine(Backmenu());
        if (buttonSound != null)
        {
            buttonSound.PlayTaruhSound();
        }
    }
    private IEnumerator Backmenu()
    {
        if (transisiOut != null)
        {
            transisiOut.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            FindAnyObjectByType<BGMManager>().StopBGM();
            if (SceneManager.GetActiveScene().name == "Level-2")
            {
                FindAnyObjectByType<bgm_Level_Selection>().StopBGM();
            }
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level_Selection");

        }
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (buttonSound != null)
        {
            buttonSound.PlayTaruhSound();
        }
        Debug.Log("Game Resumed");
    }

    public void QuitGame()
    {
        Application.Quit();
        buttonSound.PlayTaruhSound();
    }
}
