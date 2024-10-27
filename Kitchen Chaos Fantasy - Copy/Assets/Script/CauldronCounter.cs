using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CauldronCounter : BaseCounter
{
    [SerializeField] private RecipePotionSO[] potionRecipes; // Daftar resep potion
    [SerializeField] private Slider cookingProgressSlider; // Slider untuk menampilkan progress pengolahan
    [SerializeField] private Transform counterTopPoint1; // Titik pertama untuk meletakkan bahan pertama
    [SerializeField] private Transform counterTopPoint2; // Titik kedua untuk meletakkan bahan kedua
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private Light fireLight;
    [SerializeField] private ParticleSystem blubParticle;
    [SerializeField] private Light blubLight;

    private KitchenObject ingredient1; // Referensi ke objek bahan pertama yang dimasukkan
    private KitchenObject ingredient2; // Referensi ke objek bahan kedua yang dimasukkan
    private KitchenObject finishedPotion; // Referensi ke objek Potion yang sudah selesai dibuat
    private bool isCookingInProgress = false; // Flag untuk menandakan apakah proses pengolahan sedang berlangsung

    private void Start()
    {
        if (cookingProgressSlider != null)
        {
            cookingProgressSlider.gameObject.SetActive(false);
            cookingProgressSlider.value = 0;
            cookingProgressSlider.maxValue = 1;
        }

        if (fireParticle != null)
        {
            fireParticle.Stop();
        }

        if (fireLight != null)
        {
            fireLight.enabled = false;
        }
        if (blubParticle != null)
        {
            blubParticle.Stop();
        }
        if (blubLight != null)
        {
            blubLight.enabled = false;
        }
    }

    // Metode Interaksi Utama
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();

            if (ingredient1 == null)
            {
                playerObject.SetKitchenObjectParent(this);
                ingredient1 = playerObject;
                ingredient1.transform.position = counterTopPoint1.position; // Set posisi bahan pertama
                AudioEventSystem.PlayAudio("DropIngredient");
            }
            else if (ingredient2 == null)
            {
                playerObject.SetKitchenObjectParent(this);
                ingredient2 = playerObject;
                ingredient2.transform.position = counterTopPoint2.position; // Set posisi bahan kedua
                AudioEventSystem.PlayAudio("DropIngredient");
            }
        }
        else if (!player.HasKitchenObject())
        {
            if (ingredient1 != null)
            {
                ingredient1.SetKitchenObjectParent(player); // Berikan bahan pertama ke Player
                ingredient1 = null; // Hapus referensi bahan pertama dari Cauldron

            }
            else if (ingredient2 != null)
            {
                ingredient2.SetKitchenObjectParent(player); // Berikan bahan kedua ke Player
                ingredient2 = null; // Hapus referensi bahan kedua dari Cauldron
            }
            else if (finishedPotion != null) // Jika ada Potion yang sudah jadi
            {
                finishedPotion.SetKitchenObjectParent(player); // Berikan Potion ke Player
                finishedPotion = null; // Hapus referensi Potion setelah diambil oleh Player
            }
        }
    }

    // Metode Interaksi Alternatif
    public override void InteractAlternate(Player player)
    {
        if (HasBothIngredients() && !isCookingInProgress)
        {
            RecipePotionSO matchedRecipe = GetMatchingRecipe();

            if (matchedRecipe != null)
            {
                StartCoroutine(CookPotion(player, matchedRecipe)); // Mulai proses memasak jika kedua bahan sesuai resep
                AudioEventSystem.PlayAudio("Cooking");
            }
            else
            {
                Debug.Log("No matching recipe found for these ingredients.");
            }
        }
    }

    // Coroutine untuk proses pembuatan Potion
    private IEnumerator CookPotion(Player player, RecipePotionSO recipe)
    {
        isCookingInProgress = true; // Tandai bahwa proses pengolahan sedang berlangsung
        cookingProgressSlider.gameObject.SetActive(true); // Tampilkan slider progress
        cookingProgressSlider.value = 0; // Set nilai awal slider ke 0

        if (fireParticle != null)
        {
            fireParticle.Play(); // Nyalakan partikel api
        }

        if (fireLight != null)
        {
            fireLight.enabled = true; // Nyalakan cahaya
        }
        if (blubParticle != null)
        {
            blubParticle.Play();
        }
        if (blubLight != null)
        {
            blubLight.enabled = true;
        }

        float cookDuration = 3f; // Durasi pemasakan
        float elapsedTime = 0f;

        while (elapsedTime < cookDuration)
        {
            elapsedTime += Time.deltaTime;
            cookingProgressSlider.value = elapsedTime / cookDuration; // Update nilai slider berdasarkan waktu yang telah berlalu
            yield return null; // Tunggu frame berikutnya
        }

        // Hancurkan bahan-bahan setelah selesai memasak
        ingredient1.DestroySelf();
        ingredient2.DestroySelf();
        ingredient1 = null;
        ingredient2 = null;
        AudioEventSystem.StopAudio("Cooking");

        // Buat Potion sesuai resep dan simpan di Cauldron
        Transform potionTransform = Instantiate(recipe.potionResult.prefab);
        potionTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        finishedPotion = potionTransform.GetComponent<KitchenObject>(); // Simpan referensi ke Potion

        cookingProgressSlider.gameObject.SetActive(false); // Sembunyikan slider progress setelah selesai
        isCookingInProgress = false; // Reset status pengolahan

        if (fireParticle != null)
        {
            fireParticle.Stop(); // Matikan partikel api setelah memasak selesai
        }

        if (fireLight != null)
        {
            fireLight.enabled = false; // Matikan cahaya setelah memasak selesai
        }
        if (blubParticle != null)
        {
            blubParticle.Stop();
        }
        if (blubLight != null)
        {
            blubLight.enabled = false;

        }
    }

    // Cek apakah kedua bahan sudah lengkap di Cauldron
    private bool HasBothIngredients()
    {
        return ingredient1 != null && ingredient2 != null;
    }

    // Cari resep yang sesuai dengan bahan-bahan yang dimasukkan
    private RecipePotionSO GetMatchingRecipe()
    {
        foreach (RecipePotionSO recipe in potionRecipes)
        {
            if (recipe.ingredients.Length == 2)
            {
                // Periksa apakah bahan 1 dan bahan 2 cocok dengan resep
                if ((recipe.ingredients[0] == ingredient1.GetKitchenObjectSO() && recipe.ingredients[1] == ingredient2.GetKitchenObjectSO()) ||
                    (recipe.ingredients[1] == ingredient1.GetKitchenObjectSO() && recipe.ingredients[0] == ingredient2.GetKitchenObjectSO()))
                {
                    return recipe;
                }
            }
        }
        return null;
    }
    public KitchenObject GetIngredient1()
    {
        return ingredient1;
    }
}
