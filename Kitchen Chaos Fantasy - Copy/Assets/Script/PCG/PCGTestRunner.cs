using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PCGTestRunner : MonoBehaviour
{
    [SerializeField] private OrderGenerator generator;
    [SerializeField] private int runs = 30;

    private int checksPassed;
    private int checksTotal;

    [ContextMenu("Run Batch Test")]
    public void RunBatch()
    {
        if (generator == null || generator.Config == null)
        {
            Debug.LogError("[PCG Test] Generator atau LevelConfig belum diisi.");
            return;
        }

        generator.ResetState();
        HashSet<string> unique = new HashSet<string>();
        int totalCandidates = 0;
        int successes = 0;

        for (int i = 0; i < runs; i++)
        {
            List<KitchenObjectSO> orders = generator.Generate("batch");
            totalCandidates += generator.LastAttempts;
            if (orders == null) continue;

            successes++;
            unique.Add(string.Join("|", orders.ConvertAll(o => o.objectName).ToArray()));
        }

        LevelConfigSO c = generator.Config;
        Debug.Log("[PCG Test] Level " + c.levelIndex
            + " | generate sukses " + successes + "/" + runs
            + " | kombinasi unik " + unique.Count
            + " | total kandidat " + totalCandidates + " (ditolak " + (totalCandidates - successes) + ")"
            + " | O=" + c.orderCount + " P=" + c.PotionCount
            + " T=" + c.TimePerOrder.ToString("F1", CultureInfo.InvariantCulture)
            + " | D=" + c.DifficultyScore().ToString("F3", CultureInfo.InvariantCulture));
    }

    [ContextMenu("Run Negative Test")]
    public void RunNegative()
    {
        if (generator == null || generator.Config == null)
        {
            Debug.LogError("[PCG Test] Generator atau LevelConfig belum diisi.");
            return;
        }

        generator.ResetState();
        DependencyValidator v = generator.Validator;
        LevelConfigSO c = generator.Config;
        checksPassed = 0;
        checksTotal = 0;

        // Kontrol positif: resep benar harus lolos
        PotionRecipeSO good = MakeRecipe(Step(IngredientCategory.Water, false), Step(IngredientCategory.Plant, true), Step(IngredientCategory.Mushroom, false));
        Check("kontrol: resep benar lolos", v.ValidateRecipe(good).IsValid);

        // R1, R2, R3
        PotionRecipeSO r1 = MakeRecipe(Step(IngredientCategory.Plant, true), Step(IngredientCategory.Water, false), Step(IngredientCategory.Mushroom, false));
        Check("R1 ditolak: air bukan langkah pertama", !v.ValidateRecipe(r1).IsValid);

        PotionRecipeSO r2 = MakeRecipe(Step(IngredientCategory.Water, false), Step(IngredientCategory.Plant, false), Step(IngredientCategory.Mushroom, false));
        Check("R2 ditolak: tanaman tidak diolah", !v.ValidateRecipe(r2).IsValid);

        PotionRecipeSO r3 = MakeRecipe(Step(IngredientCategory.Water, false), Step(IngredientCategory.Plant, true), Step(IngredientCategory.Mushroom, true));
        Check("R3 ditolak: jamur diolah", !v.ValidateRecipe(r3).IsValid);

        // R4: potion di luar level
        KitchenObjectSO dummy = ScriptableObject.CreateInstance<KitchenObjectSO>();
        dummy.objectName = "DummyPotion";
        List<KitchenObjectSO> outside = new List<KitchenObjectSO>();
        for (int i = 0; i < c.orderCount; i++) outside.Add(dummy);
        Check("R4 ditolak: potion di luar level", !v.ValidateCandidate(outside, c).IsValid);

        // R5: jumlah order salah
        List<KitchenObjectSO> wrongCount = new List<KitchenObjectSO>();
        for (int i = 0; i < c.orderCount + 1; i++) wrongCount.Add(c.availablePotions[0]);
        Check("R5 ditolak: jumlah order salah", !v.ValidateCandidate(wrongCount, c).IsValid);

        // R6: semua order sama (hanya diuji jika aturan aktif dan memungkinkan)
        if (c.requireAllPotionTypes && c.PotionCount > 1 && c.orderCount >= c.PotionCount)
        {
            List<KitchenObjectSO> sameOnly = new List<KitchenObjectSO>();
            for (int i = 0; i < c.orderCount; i++) sameOnly.Add(c.availablePotions[0]);
            Check("R6 ditolak: ada potion yang tidak muncul", !v.ValidateCandidate(sameOnly, c).IsValid);
        }

        DestroyImmediate(good);
        DestroyImmediate(r1);
        DestroyImmediate(r2);
        DestroyImmediate(r3);
        DestroyImmediate(dummy);

        Debug.Log("[PCG Test] Uji negatif: " + checksPassed + "/" + checksTotal + " sesuai harapan");
    }

    private void Check(string name, bool passed)
    {
        checksTotal++;
        if (passed) checksPassed++;
        Debug.Log("[PCG Test] " + (passed ? "OK    " : "GAGAL ") + name);
    }

    private static RecipeStepData Step(IngredientCategory category, bool requiresProcessing)
    {
        RecipeStepData s = new RecipeStepData();
        s.category = category;
        s.requiresProcessing = requiresProcessing;
        return s;
    }

    private static PotionRecipeSO MakeRecipe(params RecipeStepData[] steps)
    {
        PotionRecipeSO r = ScriptableObject.CreateInstance<PotionRecipeSO>();
        r.steps = new List<RecipeStepData>(steps);
        return r;
    }
}