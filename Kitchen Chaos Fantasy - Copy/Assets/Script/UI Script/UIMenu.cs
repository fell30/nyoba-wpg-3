using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    public GameObject OptionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Start()
    {

    }

    // EnableDisable Menu
    public void EnableMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void DisableMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    // Pause Game, Resume Game, Restart game, Back to Main Menu
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BacktoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        OptionsMenu.SetActive(true);
    }

    public void BackOptions()
    {
        OptionsMenu.SetActive(false);
    }
}
