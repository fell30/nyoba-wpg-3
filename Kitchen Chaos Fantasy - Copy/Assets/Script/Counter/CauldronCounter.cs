using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CauldronCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private RecipePotionSO[] _potionRecipes;
    [SerializeField] private Transform _counterTopPoint1;
    [SerializeField] private Transform _counterTopPoint2;
    [SerializeField] private ParticleSystem _fireParticle;
    [SerializeField] private ParticleSystem MagicParticle;
    [SerializeField] private Light _fireLight;
    [SerializeField] private ParticleSystem _blubParticle;
    [SerializeField] private Light _blubLight;
    [SerializeField] private KitchenObjectSO _waterBucketSO;
    [SerializeField] private GameObject _Water;
    [SerializeField] private Player _player;

    private KitchenObject _ingredient1;
    private KitchenObject _ingredient2;
    private KitchenObject _finishedPotion;
    private bool _isCookingInProgress = false;
    private bool _isWaterAdded = false;

    public bool IsWaterAdded() => _isWaterAdded;

    private void Start()
    {
        InitializeEffects();
        _Water.SetActive(false);


    }

    private void InitializeEffects()
    {
        _fireParticle?.Stop();
        MagicParticle?.Stop();
        _fireLight.enabled = false;
        _blubParticle?.Stop();
        _blubLight.enabled = false;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject playerObject = player.GetKitchenObject();

            if (!_isWaterAdded && playerObject.GetKitchenObjectSO() == _waterBucketSO)
            {
                KitchenObject.Destroy(playerObject.gameObject);
                _isWaterAdded = true;
                _Water.SetActive(true);
                AudioEventSystem.PlayAudio("DropIngredient");
                GetComponent<WellAudio>()?.PlayAmbilSound();
            }
            else if (_isWaterAdded && playerObject.GetKitchenObjectSO() != _waterBucketSO)
            {
                if (_ingredient1 == null)
                {
                    playerObject.SetKitchenObjectParent(this);
                    _ingredient1 = playerObject;
                    _ingredient1.transform.position = _counterTopPoint1.position;
                    AudioEventSystem.PlayAudio("DropIngredient");
                    GetComponent<WellAudio>()?.PlayAmbilSound();
                }
                else if (_ingredient2 == null)
                {
                    playerObject.SetKitchenObjectParent(this);
                    _ingredient2 = playerObject;
                    _ingredient2.transform.position = _counterTopPoint2.position;
                    AudioEventSystem.PlayAudio("DropIngredient");
                    GetComponent<WellAudio>()?.PlayAmbilSound();
                }
            }
        }
        else
        {
            if (_finishedPotion != null)
            {
                _finishedPotion.SetKitchenObjectParent(player);
                _finishedPotion = null;
                ResetCauldron();
            }
            else if (_ingredient2 != null)
            {
                _ingredient2.SetKitchenObjectParent(player);
                _ingredient2 = null;
                StopBlubParticleIfNoIngredients();
            }
            else if (_ingredient1 != null)
            {
                _ingredient1.SetKitchenObjectParent(player);
                _ingredient1 = null;
                StopBlubParticleIfNoIngredients();
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasBothIngredients() && !_isCookingInProgress)
        {
            RecipePotionSO matchedRecipe = GetMatchingRecipe();
            if (matchedRecipe != null)
            {
                StartCoroutine(CookPotion(matchedRecipe));
                AudioEventSystem.PlayAudio("Cooking");
                GetComponent<WellAudio>()?.MasakPotion();
            }
            else
            {
                Debug.Log("No matching recipe found for these ingredients.");
            }
        }
    }

    private IEnumerator CookPotion(RecipePotionSO recipe)
    {
        _isCookingInProgress = true;
        PlayCookingEffects();
        Player.Instance.SetIsCooking(true);
        _player.enabled = false;


        float cookDuration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < cookDuration)
        {
            elapsedTime += Time.deltaTime;
            float progressNormalized = elapsedTime / cookDuration;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = progressNormalized });
            yield return null;
        }

        _ingredient1?.DestroySelf();
        _ingredient2?.DestroySelf();
        _ingredient1 = null;
        _ingredient2 = null;
        AudioEventSystem.StopAudio("Cooking");
        GetComponent<WellAudio>()?.StopMasakPotion();


        Transform potionTransform = Instantiate(recipe.potionResult.prefab);
        potionTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        _finishedPotion = potionTransform.GetComponent<KitchenObject>();

        _isCookingInProgress = false;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0 });
        StopCookingEffects();
        Player.Instance.SetIsCooking(false);
        _player.enabled = true;
    }

    private void ResetCauldron()
    {
        _Water.SetActive(false);
        _isWaterAdded = false;
    }

    private void PlayCookingEffects()
    {
        _fireParticle?.Play();
        _fireLight.enabled = true;
        _blubParticle?.Play();
        _blubLight.enabled = true;
        StartCoroutine(MagicDelay());
    }
    private IEnumerator MagicDelay()
    {
        yield return new WaitForSeconds(1.8f);
        MagicParticle?.Play();
    }

    private void StopCookingEffects()
    {
        _fireParticle?.Stop();
        _fireLight.enabled = false;
        _blubParticle?.Stop();
        _blubLight.enabled = false;
        MagicParticle?.Stop();
    }

    private void StopBlubParticleIfNoIngredients()
    {
        if (_ingredient1 == null && _ingredient2 == null)
        {
            _blubParticle?.Stop();
            _blubLight.enabled = false;
            ResetCauldron();
        }
    }

    private bool HasBothIngredients() => _ingredient1 != null && _ingredient2 != null;

    private RecipePotionSO GetMatchingRecipe()
    {
        foreach (RecipePotionSO recipe in _potionRecipes)
        {
            if (recipe.ingredients.Length == 2 &&
                ((recipe.ingredients[0] == _ingredient1.GetKitchenObjectSO() && recipe.ingredients[1] == _ingredient2.GetKitchenObjectSO()) ||
                (recipe.ingredients[1] == _ingredient1.GetKitchenObjectSO() && recipe.ingredients[0] == _ingredient2.GetKitchenObjectSO())))
            {
                return recipe;
            }
        }
        return null;
    }
    public KitchenObject GetIngredient1()
    {
        return _ingredient1;
    }

    public KitchenObject GetIngredient2()
    {
        return _ingredient2;
    }

}
