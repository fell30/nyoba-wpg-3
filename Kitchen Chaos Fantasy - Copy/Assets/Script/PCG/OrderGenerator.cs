using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator : MonoBehaviour
{
    [Header("Konfigurasi level (isi sesuai scene)")]
    [SerializeField] private LevelConfigSO levelConfig;
    [SerializeField] private List<PotionRecipeSO> recipes = new List<PotionRecipeSO>();

    [Header("Generator")]
    [SerializeField] private int maxAttempts = 100;
    [Tooltip("0 = acak setiap kali. Isi angka lain agar hasil bisa diulang persis.")]
    [SerializeField] private int seed = 0;

    [Header("Opsional")]
    [SerializeField] private OrderLogger logger;

    private System.Random rng;
    private DependencyValidator validator;
    private int generateCounter = 0;

    public LevelConfigSO Config { get { return levelConfig; } }
    public int LastAttempts { get; private set; }

    public DependencyValidator Validator
    {
        get
        {
            if (validator == null) validator = new DependencyValidator(recipes);
            return validator;
        }
    }

    // Satu instance Random dipakai bersama agar hasil tidak kembar pada pemanggilan beruntun.
    private System.Random Rng
    {
        get
        {
            if (rng == null) rng = (seed != 0) ? new System.Random(seed) : new System.Random();
            return rng;
        }
    }

    public void ResetState()
    {
        rng = null;
        validator = null;
        generateCounter = 0;
        LastAttempts = 0;
    }

    // Mengembalikan daftar order valid, atau null jika gagal (OrderSystem akan fallback).</summary>
    public List<KitchenObjectSO> Generate(string source = "game")
    {
        LastAttempts = 0;

        if (levelConfig == null || levelConfig.PotionCount == 0 || levelConfig.orderCount <= 0)
        {
            Debug.LogError("[PCG] LevelConfig belum diisi dengan benar.");
            return null;
        }

        generateCounter++;
        string lastReason = "";
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            List<KitchenObjectSO> candidate = BuildCandidate();
            DependencyCheckResult result = Validator.ValidateCandidate(candidate, levelConfig);
            LastAttempts = attempt;
            lastReason = result.Reason;

            if (logger != null)
                logger.LogCandidate(levelConfig, generateCounter, attempt, result, candidate, source);

            if (result.IsValid) return candidate;
        }

        Debug.LogError("[PCG] Gagal menghasilkan order valid setelah " + maxAttempts + " percobaan. "
            + "Masalah: " + Validator.DescribeSetupProblems(levelConfig)
            + " | Penolakan terakhir: " + lastReason);
        return null;
    }

    private List<KitchenObjectSO> BuildCandidate()
    {
        List<KitchenObjectSO> candidate = new List<KitchenObjectSO>(levelConfig.orderCount);
        for (int i = 0; i < levelConfig.orderCount; i++)
        {
            int index = Rng.Next(levelConfig.availablePotions.Count);
            candidate.Add(levelConfig.availablePotions[index]);
        }
        return candidate;
    }
}