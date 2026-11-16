using System.Collections.Generic;

public struct DependencyCheckResult
{
    public bool IsValid;
    public string Reason;

    public static DependencyCheckResult Ok()
    {
        return new DependencyCheckResult { IsValid = true, Reason = "OK" };
    }

    public static DependencyCheckResult Fail(string reason)
    {
        return new DependencyCheckResult { IsValid = false, Reason = reason };
    }
}

public class DependencyValidator
{
    private readonly Dictionary<KitchenObjectSO, PotionRecipeSO> recipeByPotion =
        new Dictionary<KitchenObjectSO, PotionRecipeSO>();

    public DependencyValidator(IEnumerable<PotionRecipeSO> recipes)
    {
        if (recipes == null) return;
        foreach (PotionRecipeSO recipe in recipes)
        {
            if (recipe != null && recipe.potion != null)
            {
                recipeByPotion[recipe.potion] = recipe;
            }
        }
    }

    public DependencyCheckResult ValidateRecipe(PotionRecipeSO recipe)
    {
        if (recipe == null || recipe.steps == null || recipe.steps.Count == 0)
            return DependencyCheckResult.Fail("resep kosong");

        if (recipe.steps[0].category != IngredientCategory.Water)
            return DependencyCheckResult.Fail("R1: air bukan langkah pertama");

        for (int i = 0; i < recipe.steps.Count; i++)
        {
            RecipeStepData step = recipe.steps[i];
            if (step.category == IngredientCategory.Plant && !step.requiresProcessing)
                return DependencyCheckResult.Fail("R2: tanaman tidak diolah");
            if (step.category == IngredientCategory.Mushroom && step.requiresProcessing)
                return DependencyCheckResult.Fail("R3: jamur tidak boleh diolah");
        }
        return DependencyCheckResult.Ok();
    }

    // Ringkasan masalah setup (resep hilang/salah) untuk pesan error di Console.</summary>
    public string DescribeSetupProblems(LevelConfigSO config)
    {
        if (config == null) return "LevelConfig kosong";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < config.availablePotions.Count; i++)
        {
            KitchenObjectSO potion = config.availablePotions[i];
            if (potion == null)
            {
                sb.Append("[Available Potions elemen " + i + " kosong] ");
                continue;
            }

            PotionRecipeSO recipe;
            if (!recipeByPotion.TryGetValue(potion, out recipe))
            {
                sb.Append("[" + potion.objectName + ": tidak ada Potion Recipe yang kolom Potion-nya menunjuk ke aset ini] ");
                continue;
            }

            DependencyCheckResult r = ValidateRecipe(recipe);
            if (!r.IsValid) sb.Append("[" + potion.objectName + ": " + r.Reason + "] ");
        }
        return sb.Length == 0 ? "resep dan config tampak benar" : sb.ToString();
    }

    public DependencyCheckResult ValidateCandidate(List<KitchenObjectSO> candidate, LevelConfigSO config)
    {
        if (candidate == null || config == null)
            return DependencyCheckResult.Fail("kandidat atau config null");

        if (candidate.Count != config.orderCount)
            return DependencyCheckResult.Fail("R5: jumlah order tidak sesuai config");

        for (int i = 0; i < candidate.Count; i++)
        {
            KitchenObjectSO potion = candidate[i];
            if (potion == null)
                return DependencyCheckResult.Fail("R4: order kosong pada indeks " + i);

            if (!config.availablePotions.Contains(potion))
                return DependencyCheckResult.Fail("R4: potion tidak tersedia di level ini (" + potion.objectName + ")");

            PotionRecipeSO recipe;
            if (!recipeByPotion.TryGetValue(potion, out recipe))
                return DependencyCheckResult.Fail("R4: resep tidak ditemukan (" + potion.objectName + ")");

            DependencyCheckResult recipeResult = ValidateRecipe(recipe);
            if (!recipeResult.IsValid)
                return DependencyCheckResult.Fail(potion.objectName + " - " + recipeResult.Reason);
        }

        // R6 hanya bisa dipenuhi jika jumlah order >= jumlah jenis potion
        bool enforceCoverage = config.requireAllPotionTypes && config.orderCount >= config.PotionCount;
        if (enforceCoverage)
        {
            for (int i = 0; i < config.availablePotions.Count; i++)
            {
                if (!candidate.Contains(config.availablePotions[i]))
                    return DependencyCheckResult.Fail("R6: potion tidak muncul (" + config.availablePotions[i].objectName + ")");
            }
        }

        return DependencyCheckResult.Ok();
    }
}