using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // Tambahkan ini!
using FMODUnity;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons;

    private int currentIndex = 0;

    private void Start()
    {
        if (menuButtons.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
            PlayBounceAnimation(menuButtons[0].transform); // Bounce awal
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Length;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    private void UpdateSelection()
    {
        GameObject selected = menuButtons[currentIndex].gameObject;
        EventSystem.current.SetSelectedGameObject(selected);

        PlayBounceAnimation(selected.transform); // ⬅️ Tambahkan animasi di sini
    }

    private void PlayBounceAnimation(Transform target)
    {
        target.localScale = Vector3.one * 0.13f; // Reset ke skala aslinya
        target.DOScale(1.0f, 0.15f)
              .SetEase(Ease.OutBack)
              .OnComplete(() =>
              {
                  target.DOScale(1.10f, 0.15f).SetEase(Ease.InOutQuad);
              });


    }
}