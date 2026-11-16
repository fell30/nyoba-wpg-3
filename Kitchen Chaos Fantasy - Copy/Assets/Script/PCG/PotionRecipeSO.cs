using System.Collections.Generic;
using UnityEngine;

public enum IngredientCategory
{
    Water,
    Plant,
    Mushroom
}

[System.Serializable]
public class RecipeStepData
{
    public IngredientCategory category;

    [Tooltip("Centang jika bahan harus diolah di meja pengolahan sebelum masuk cauldron.")]
    public bool requiresProcessing;
}


[CreateAssetMenu(menuName = "Potion Madness/Potion Recipe", fileName = "PotionRecipe")]
public class PotionRecipeSO : ScriptableObject
{
    public KitchenObjectSO potion;
    public List<RecipeStepData> steps = new List<RecipeStepData>();
}