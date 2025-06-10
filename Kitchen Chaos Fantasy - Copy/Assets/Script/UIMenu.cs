using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Diagnostics.CodeAnalysis;

public class UIMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private CameraFollowWithIntro cameraFollow;
    [SerializeField] private StudioEventEmitter bgmMainMenu; // Tambahkan ini untuk BGM
    [SerializeField] private PlayerLevelSelection playerLevelSelection; // Tambahkan ini untuk PlayerLevelSelection
    [SerializeField] private SfxMortar buttonSound;
    [SerializeField] private MenuNavigation menuNavigation;
    public GameObject  menuplayqu;
    


    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        else
            Debug.LogWarning("Play Button belum di-assign di Inspector!");

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
        else
            Debug.LogWarning("Quit Button belum di-assign di Inspector!");

        if (SceneManager.GetActiveScene().name == "Level_Selection")
        {
            if (bgmMainMenu != null)
            {
                bgmMainMenu.Play();
            }
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            EnableMenu();
            Debug.Log("askaksaksa");
        }
    }

    public void OnPlayClicked()  // <-- Diubah jadi public
    {
        if (cameraFollow != null)
        {
            cameraFollow.StartIntroTransition();
            bgmMainMenu?.Stop(); // Hentikan BGM jika ada
            playerLevelSelection?.playBGMLevelSelection(); // Memanggil playBGMLevelSelection jika ada
            playerLevelSelection.enabled = true;
            if (buttonSound != null)
            {
                buttonSound.PlayTaruhSound(); // Memanggil PlayTaruhSound dari SfxMortar
            }

        }
        else
        {
            Debug.LogWarning("CameraFollowWithIntro belum di-assign di Inspector!");
        }

        gameObject.SetActive(false);
    }

    public void OnQuitClicked()  // <-- Diubah jadi public
    {
        Debug.Log("Quit Game");
        if (buttonSound != null)
        {
            buttonSound.PlayTaruhSound(); // Memanggil PlayTaruhSound dari SfxMortar
        }
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void DisableMenu(GameObject menu)
    {
        menu.SetActive(false);
        menuNavigation.enabled = false;
    }
    public void EnableMenu()
    {
        menuplayqu.SetActive(true);
        menuNavigation.enabled = true;
    }
    public void EnableMenuuuuuuu(GameObject gameObject)
    {
      gameObject.SetActive(true);
    }
}
