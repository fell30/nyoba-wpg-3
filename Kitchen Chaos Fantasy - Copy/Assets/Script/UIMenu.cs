using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private CameraFollowWithIntro cameraFollow;

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
    }

    public void OnPlayClicked()  // <-- Diubah jadi public
    {
        if (cameraFollow != null)
        {
            cameraFollow.StartIntroTransition();
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
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
