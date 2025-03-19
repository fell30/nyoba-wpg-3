using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image recipeImage; // Gambar order

    public void SetOrder(KitchenObjectSO potion)
    {
        if (recipeImage != null && potion.recipeSprite != null)
        {
            recipeImage.sprite = potion.recipeSprite; // Gunakan sprite langsung dari PotionSO
        }
    }

    public void DestroyUI()
    {
        Destroy(gameObject);
    }
}
