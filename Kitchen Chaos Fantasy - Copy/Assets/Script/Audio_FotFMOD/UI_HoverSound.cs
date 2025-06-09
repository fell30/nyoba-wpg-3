using UnityEngine;
using UnityEngine.EventSystems;

public class UI_HoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private SfxMortar hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            hoverSound.PlayMortarSound(); // atau kamu bisa buat fungsi baru: PlayHoverSound()
        }
    }
}
