using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CauldronCounter : BaseCounter
{
    [SerializeField] private RecipePotionSO[] _potionRecipes; // Daftar resep potion
    [SerializeField] private Slider _cookingProgressSlider; // Slider untuk menampilkan progress pengolahan
    [SerializeField] private Transform _counterTopPoint1; // Titik pertama untuk meletakkan bahan pertama
    [SerializeField] private Transform _counterTopPoint2; // Titik kedua untuk meletakkan bahan kedua
    [SerializeField] private ParticleSystem _fireParticle;
    [SerializeField] private Light _fireLight;
    [SerializeField] private ParticleSystem _blubParticle;
    [SerializeField] private Light _blubLight;
    [SerializeField] private KitchenObjectSO _waterBucketSO; // Referensi ke Water
    [SerializeField] private GameObject _Water; // GameObject visual Water

    private KitchenObject _ingredient1; // Referensi ke objek bahan pertama yang dimasukkan (non-water)
    private KitchenObject _ingredient2; // Referensi ke objek bahan kedua yang dimasukkan (non-water)
    private KitchenObject _finishedPotion; // Referensi ke objek Potion yang sudah selesai dibuat
    private bool _isCookingInProgress = false; // Flag untuk menandakan apakah proses pengolahan sedang berlangsung
    private bool _isWaterAdded = false; // Flag untuk menandakan apakah water sudah dimasukkan

    // Metode Getter untuk Flag _isWaterAdded
    public bool IsWaterAdded()
    {
        return _isWaterAdded;
    }

    private void Start()
    {
        InitializeUI();
        InitializeEffects();
        _Water.SetActive(false); // Sembunyikan visual Water pada awalnya
    }

    private void InitializeUI()
    {
        if (_cookingProgressSlider != null)
        {
            _cookingProgressSlider.gameObject.SetActive(false);
            _cookingProgressSlider.value = 0;
            _cookingProgressSlider.maxValue = 1;
        }
    }

    private void InitializeEffects()
    {
        if (_fireParticle != null)
        {
            _fireParticle.Stop();
        }

        if (_fireLight != null)
        {
            _fireLight.enabled = false;
        }
        if (_blubParticle != null)
        {
            _blubParticle.Stop();
        }
        if (_blubLight != null)
        {
            _blubLight.enabled = false;
        }
    }

    // Metode Interaksi Utama
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();

            if (!_isWaterAdded)
            {
                // Cauldron requires water first
                if (playerObject.GetKitchenObjectSO() == _waterBucketSO)
                {
                    // Destroy the WaterBucket in player's hand
                    KitchenObject.Destroy(playerObject.gameObject);
                    _isWaterAdded = true;

                    // Activate Water GameObject in cauldron
                    _Water.SetActive(true);
                    AudioEventSystem.PlayAudio("DropIngredient");

                    // Start blubParticle effects
                    StartBlubParticle();
                }
                else
                {
                    Debug.Log("Cauldron requires water as the first ingredient.");
                    // Tambahkan feedback visual atau audio di sini untuk memberi tahu pemain
                    // Contoh: UIManager.Instance.ShowMessage("First ingredient must be Water!");
                    // AudioEventSystem.PlayAudio("ErrorSound");
                }
            }
            else
            {
                // Water already added, allow adding ingredients
                if (playerObject.GetKitchenObjectSO() != _waterBucketSO)
                {
                    if (_ingredient1 == null)
                    {
                        playerObject.SetKitchenObjectParent(this); // Pindahkan bahan pertama ke cauldron
                        _ingredient1 = playerObject;
                        _ingredient1.transform.position = _counterTopPoint1.position; // Set posisi bahan pertama
                        AudioEventSystem.PlayAudio("DropIngredient");
                    }
                    else if (_ingredient2 == null)
                    {
                        playerObject.SetKitchenObjectParent(this); // Pindahkan bahan kedua ke cauldron
                        _ingredient2 = playerObject;
                        _ingredient2.transform.position = _counterTopPoint2.position; // Set posisi bahan kedua
                        AudioEventSystem.PlayAudio("DropIngredient");
                    }
                    else
                    {
                        Debug.Log("Cauldron is full.");
                        // Tambahkan feedback visual atau audio di sini untuk memberi tahu pemain
                        // Contoh: UIManager.Instance.ShowMessage("Cauldron is full!");
                        // AudioEventSystem.PlayAudio("ErrorSound");
                    }
                }
                else
                {
                    Debug.Log("Water has already been added. You cannot add another water.");
                    // Tambahkan feedback visual atau audio di sini untuk memberi tahu pemain
                    // Contoh: UIManager.Instance.ShowMessage("Water has already been added!");
                    // AudioEventSystem.PlayAudio("ErrorSound");
                }
            }
        }
        else
        {
            // Player tidak memegang objek, coba ambil bahan atau potion dari cauldron
            if (_finishedPotion != null)
            {
                _finishedPotion.SetKitchenObjectParent(player); // Berikan Potion ke Player
                _finishedPotion = null; // Hapus referensi Potion setelah diambil oleh Player
            }
            else
            {
                if (_ingredient1 != null)
                {
                    _ingredient1.SetKitchenObjectParent(player); // Berikan bahan pertama ke Player
                    _ingredient1 = null; // Hapus referensi bahan pertama dari Cauldron

                    // Matikan blubParticle jika tidak ada bahan lagi
                    StopBlubParticleIfNoIngredients();
                }
                else if (_ingredient2 != null)
                {
                    _ingredient2.SetKitchenObjectParent(player); // Berikan bahan kedua ke Player
                    _ingredient2 = null; // Hapus referensi bahan kedua dari Cauldron

                    // Matikan blubParticle jika tidak ada bahan lagi
                    StopBlubParticleIfNoIngredients();
                }
            }
        }
    }

    // Metode Interaksi Alternatif
    public override void InteractAlternate(Player player)
    {
        if (HasBothIngredients() && !_isCookingInProgress)
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
                // Tambahkan feedback visual atau audio di sini untuk memberi tahu pemain
                // Contoh: UIManager.Instance.ShowMessage("No matching recipe found!");
                // AudioEventSystem.PlayAudio("ErrorSound");
            }
        }
        else
        {
            Debug.Log("Cannot start cooking. Ensure you have both water and another ingredient.");
            // Tambahkan feedback visual atau audio di sini untuk memberi tahu pemain
            // Contoh: UIManager.Instance.ShowMessage("Ensure you have both water and another ingredient.");
            // AudioEventSystem.PlayAudio("ErrorSound");
        }
    }

    // Coroutine untuk proses pembuatan Potion
    private IEnumerator CookPotion(Player player, RecipePotionSO recipe)
    {
        _isCookingInProgress = true; // Tandai bahwa proses pengolahan sedang berlangsung
        _cookingProgressSlider.gameObject.SetActive(true); // Tampilkan slider progress
        _cookingProgressSlider.value = 0; // Set nilai awal slider ke 0

        PlayCookingEffects();

        float cookDuration = 3f; // Durasi pemasakan
        float elapsedTime = 0f;

        while (elapsedTime < cookDuration)
        {
            elapsedTime += Time.deltaTime;
            _cookingProgressSlider.value = elapsedTime / cookDuration; // Update nilai slider berdasarkan waktu yang telah berlalu
            yield return null; // Tunggu frame berikutnya
        }

        // Hancurkan bahan-bahan setelah selesai memasak
        _ingredient1?.DestroySelf();
        _ingredient2?.DestroySelf();
        _ingredient1 = null;
        _ingredient2 = null;
        AudioEventSystem.StopAudio("Cooking");

        // Buat Potion sesuai resep dan simpan di Cauldron
        Transform potionTransform = Instantiate(recipe.potionResult.prefab);
        potionTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        _finishedPotion = potionTransform.GetComponent<KitchenObject>(); // Simpan referensi ke Potion

        _cookingProgressSlider.gameObject.SetActive(false); // Sembunyikan slider progress setelah selesai
        _isCookingInProgress = false; // Reset status pengolahan

        StopCookingEffects();
    }

    private void PlayCookingEffects()
    {
        if (_fireParticle != null)
        {
            _fireParticle.Play(); // Nyalakan partikel api
        }

        if (_fireLight != null)
        {
            _fireLight.enabled = true; // Nyalakan cahaya
        }
        if (_blubParticle != null)
        {
            _blubParticle.Play();
        }
        if (_blubLight != null)
        {
            _blubLight.enabled = true;
        }
    }

    private void StopCookingEffects()
    {
        if (_fireParticle != null)
        {
            _fireParticle.Stop(); // Matikan partikel api setelah memasak selesai
        }

        if (_fireLight != null)
        {
            _fireLight.enabled = false; // Matikan cahaya setelah memasak selesai
        }
        if (_blubParticle != null)
        {
            _blubParticle.Stop();
        }
        if (_blubLight != null)
        {
            _blubLight.enabled = false;
        }
    }

    // Cek apakah kedua bahan sudah lengkap di Cauldron
    private bool HasBothIngredients()
    {
        return _ingredient1 != null && _ingredient2 != null;
    }

    // Cari resep yang sesuai dengan bahan-bahan yang dimasukkan
    private RecipePotionSO GetMatchingRecipe()
    {
        foreach (RecipePotionSO recipe in _potionRecipes)
        {
            if (recipe.ingredients.Length == 2)
            {
                // Periksa apakah bahan 1 dan bahan 2 cocok dengan resep
                if ((recipe.ingredients[0] == _ingredient1.GetKitchenObjectSO() && recipe.ingredients[1] == _ingredient2.GetKitchenObjectSO()) ||
                    (recipe.ingredients[1] == _ingredient1.GetKitchenObjectSO() && recipe.ingredients[0] == _ingredient2.GetKitchenObjectSO()))
                {
                    return recipe;
                }
            }
        }
        return null;
    }

    public KitchenObject GetIngredient1()
    {
        return _ingredient1;
    }

    // Metode untuk memulai blubParticle
    public void StartBlubParticle()
    {
        if (_blubParticle != null && !_blubParticle.isPlaying)
        {
            _blubParticle.Play();
        }

        if (_blubLight != null && !_blubLight.enabled)
        {
            _blubLight.enabled = true;
        }
    }

    // Metode untuk mematikan blubParticle jika tidak ada bahan
    private void StopBlubParticleIfNoIngredients()
    {
        if (_ingredient1 == null && _ingredient2 == null)
        {
            if (_blubParticle != null && _blubParticle.isPlaying)
            {
                _blubParticle.Stop();
            }

            if (_blubLight != null && _blubLight.enabled)
            {
                _blubLight.enabled = false;
            }

            _Water.SetActive(false); // Nonaktifkan visual Water
            _isWaterAdded = false; // Reset flag water
        }
    }
}
