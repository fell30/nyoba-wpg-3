using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMenu : MonoBehaviour
{

    public Button[] menuButtons;
    public GameObject TRANSISIOUT;
    private int currentIndex = 0;


    void Start()
    {
        EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
            EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }
    public void PlayGame(string nameScene)
    {
        StartCoroutine(TransitionOut(nameScene));
    }
    private IEnumerator TransitionOut(string nameScene)
    {
        TRANSISIOUT.SetActive(true);
        yield return new WaitForSeconds(0.83f);
        SceneManager.LoadScene(nameScene);
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


}
