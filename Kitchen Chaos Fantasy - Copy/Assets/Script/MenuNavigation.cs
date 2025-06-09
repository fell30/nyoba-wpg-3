using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using FMODUnity;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons;
    [SerializeField] private SfxMortar buttonSound;

    private int currentIndex = -1; // Awalnya -1, artinya tidak ada yang terseleksi

    private void Start()
    {
        // Jangan set selected di awal, biarkan kosong
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentIndex == -1) currentIndex = 0; // Pilih pertama kali
            else currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;

            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentIndex == -1) currentIndex = 0;
            else currentIndex = (currentIndex + 1) % menuButtons.Length;

            UpdateSelection();
        }

        if (currentIndex != -1 && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    private void UpdateSelection()
    {
        GameObject selected = menuButtons[currentIndex].gameObject;
        EventSystem.current.SetSelectedGameObject(selected);
        PlayBounceAnimation(selected.transform);
    }

    private void PlayBounceAnimation(Transform target)
    {
        if (buttonSound != null)
        {
            buttonSound.PlayMortarSound();
        }

        target.localScale = Vector3.one * 0.13f;
        target.DOScale(1.0f, 0.15f)
              .SetEase(Ease.OutBack)
              .OnComplete(() =>
              {
                  target.DOScale(1.10f, 0.15f).SetEase(Ease.InOutQuad);
              });
    }
}
