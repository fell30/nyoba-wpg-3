using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class TutorialDialog : MonoBehaviour
{
    [System.Serializable]
    public class DialogBox
    {
        public GameObject dialogBox;
        public TextMeshProUGUI tutorialText;
        public CanvasGroup canvasGroup; // Untuk efek fade
        public RectTransform rectTransform; // Untuk efek slide
        [HideInInspector] public Vector2 initialPosition; // Simpan posisi awal dialog
    }

    public DialogBox mainDialog;
    public DialogBox npcDialog;
    public DialogBox hintDialog;

    public float typingSpeed = 0.01f; // Kecepatan efek ketikan
    public float fadeDuration = 0.3f;
    public float scaleDuration = 0.3f;
    public float slideDuration = 0.3f;
    public float slideDistance = 50f; // Jarak geser saat muncul

    private Coroutine typingCoroutine;

    private void Awake()
    {
        DOTween.SetTweensCapacity(1000, 100);

        // Simpan posisi awal sebelum animasi
        mainDialog.initialPosition = mainDialog.rectTransform.anchoredPosition;
        npcDialog.initialPosition = npcDialog.rectTransform.anchoredPosition;
        hintDialog.initialPosition = hintDialog.rectTransform.anchoredPosition;
    }

    public void ShowMessage(string message, string type)
    {
        DialogBox selectedDialog = GetDialogBox(type);
        if (selectedDialog == null) return;

        selectedDialog.dialogBox.SetActive(true);

        // Hentikan animasi sebelumnya untuk mencegah efek tumpukan
        selectedDialog.canvasGroup.DOKill(true);
        selectedDialog.dialogBox.transform.DOKill(true);
        selectedDialog.rectTransform.DOKill(true);

        // Reset ke posisi awal sebelum animasi
        selectedDialog.rectTransform.anchoredPosition = selectedDialog.initialPosition - new Vector2(0, slideDistance);
        selectedDialog.canvasGroup.alpha = 0;
        selectedDialog.dialogBox.transform.localScale = Vector3.one * 0.8f;

        // Animasi masuk
        Sequence seq = DOTween.Sequence();
        seq.Append(selectedDialog.canvasGroup.DOFade(1, fadeDuration));
        seq.Join(selectedDialog.dialogBox.transform.DOScale(1.3f, scaleDuration).SetEase(Ease.OutBack));
        seq.Join(selectedDialog.rectTransform.DOAnchorPosY(selectedDialog.initialPosition.y, slideDuration).SetEase(Ease.OutQuad));
        seq.AppendCallback(() =>
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(selectedDialog, message));
        });
    }

    private IEnumerator TypeText(DialogBox dialog, string message)
    {
        dialog.tutorialText.text = ""; // Reset teks

        foreach (char letter in message.ToCharArray())
        {
            dialog.tutorialText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    public void HideMessage(string type)
    {
        DialogBox selectedDialog = GetDialogBox(type);
        if (selectedDialog == null) return;

        // Pastikan efek ketikan berhenti
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Animasi keluar: fade + scale + slide
        selectedDialog.canvasGroup.DOFade(0, fadeDuration);
        selectedDialog.dialogBox.transform.DOScale(0.8f, scaleDuration).SetEase(Ease.InBack);
        selectedDialog.rectTransform.DOAnchorPosY(selectedDialog.initialPosition.y - slideDistance, slideDuration).SetEase(Ease.InQuad)
            .OnComplete(() => selectedDialog.dialogBox.SetActive(false));
    }

    public void HideMessageDelayed(string type, float delay)
    {
        StopCoroutine(HideMessageCoroutine(type, delay)); // Hentikan jika ada yang berjalan
        StartCoroutine(HideMessageCoroutine(type, delay));
    }

    private IEnumerator HideMessageCoroutine(string type, float delay)
    {
        yield return new WaitForSeconds(delay);
        HideMessage(type);
    }
    public bool IsMessageActive(string type)
    {
        DialogBox selectedDialog = GetDialogBox(type);
        return selectedDialog != null && selectedDialog.dialogBox.activeSelf;
    }

    private DialogBox GetDialogBox(string type)
    {
        switch (type)
        {
            case "main":
                return mainDialog;
            case "npc":
                return npcDialog;
            case "hint":
                return hintDialog;
            default:
                return null;
        }
    }
}