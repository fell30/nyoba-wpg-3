using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion Madness/Level Config", fileName = "LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Parameter Complexity Scaling (Tabel 3.2)")]
    public int levelIndex = 1;
    public List<KitchenObjectSO> availablePotions = new List<KitchenObjectSO>();
    public int orderCount = 2;
    public float totalTimeSeconds = 120f;

    [Header("Aturan komposisi (opsional)")]
    [Tooltip("Jika aktif, setiap potion yang tersedia harus muncul minimal sekali dalam satu sesi.")]
    public bool requireAllPotionTypes = true;

    // Bobot Weighted Scoring (Tabel 3.3)
    public const float WeightOrder = 0.40f;
    public const float WeightPotion = 0.35f;
    public const float WeightTime = 0.25f;

    public int PotionCount
    {
        get { return availablePotions != null ? availablePotions.Count : 0; }
    }

    // Waktu per order = waktu total / jumlah order (60/40/30 detik pada Tabel 3.2).
    public float TimePerOrder
    {
        get { return orderCount > 0 ? totalTimeSeconds / orderCount : 0f; }
    }

    // D = (O x w1) + (P x w2) + (1/T x w3), persis sesuai Subbab 3.2.3.
    public float DifficultyScore()
    {
        float t = TimePerOrder;
        float inverseTime = t > 0f ? 1f / t : 0f;
        return (orderCount * WeightOrder) + (PotionCount * WeightPotion) + (inverseTime * WeightTime);
    }
}