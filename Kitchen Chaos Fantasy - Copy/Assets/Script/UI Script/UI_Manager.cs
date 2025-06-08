using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public GameObject PauseMenu;


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
    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
