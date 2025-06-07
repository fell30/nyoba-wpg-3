using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMenu : MonoBehaviour
{
    public Button[] menuButtons;
    public GameObject TRANSISIOUT;

    private int currentIndex = 0;



    [Header("Zoom Out Settings")]
    public CameraZoomOut cameraZoom;
    public float zoomOutTargetFOV = 100f;
    public float zoomOutDuration = 2f;


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
        StartCoroutine(ZoomOutThenLoadScene(nameScene));
    }

    private IEnumerator ZoomOutThenLoadScene(string nameScene)
    {
        // Aktifkan transisi (jika ada animasi/layar hitam)
        if (TRANSISIOUT != null)
            TRANSISIOUT.SetActive(true);

        // Jalankan efek zoom kamera
        if (cameraZoom != null)
            yield return StartCoroutine(cameraZoom.ZoomOutCoroutine(zoomOutTargetFOV, zoomOutDuration));
        else
            yield return new WaitForSeconds(zoomOutDuration);

        // Pindah ke scene berikutnya
        SceneManager.LoadScene(nameScene);
    }

    public void EnableMenu(GameObject menu) => menu.SetActive(true);
    public void DisableMenu(GameObject menu) => menu.SetActive(false);
    public void PauseGame() => Time.timeScale = 0f;
    public void ResumeGame() => Time.timeScale = 1f;

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

    public void QuitGame() => Application.Quit();
}
