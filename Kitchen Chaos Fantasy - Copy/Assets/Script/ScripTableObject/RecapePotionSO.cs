using UnityEngine;

[CreateAssetMenu(fileName = "NewPotionRecipe", menuName = "RecipePotionSO")]
public class RecipePotionSO : ScriptableObject
{
    public string potionName; // Nama potion
    public KitchenObjectSO[] ingredients; // Daftar bahan yang diperlukan
    public KitchenObjectSO potionResult; // Hasil potion
}
