using System;
using UnityEngine;
using UnityEngine.UI;

public class WellCounter : BaseCounter
{
    public event EventHandler OnWaterCollected;

    [SerializeField] private KitchenObjectSO _waterBucketSO;
    [SerializeField] private Slider _holdSlider;
    [SerializeField] private GameInput _gameInput;

    private const float _holdTime = 2f;
    private float _holdProgress = 0f;
    private bool _isHolding = false;
    private Player _interactingPlayer;
    private Vector3 _playerStartPosition;

    private void Start()
    {
        InitializeSlider();
    }

    public void Initialize(GameInput gameInput)
    {
        _gameInput = gameInput ?? throw new ArgumentNullException(nameof(gameInput));
    }

    public override void Interact(Player player)
    {
        if (!_isHolding && !player.HasKitchenObject())
        {
            StartHoldProcess(player);
        }
    }

    private void InitializeSlider()
    {
        _holdSlider.gameObject.SetActive(false);
        _holdSlider.value = 0;

    }

    private void StartHoldProcess(Player player)
    {
        _isHolding = true;
        _holdProgress = 0f;
        _interactingPlayer = player;
        _playerStartPosition = player.Getposition();

        _holdSlider.gameObject.SetActive(true);
        _holdSlider.value = 0f;
        AudioEventSystem.PlayAudio("Well");
    }

    private void Update()
    {
        if (_isHolding)
        {
            HandleHoldProgress();
        }
    }

    private void HandleHoldProgress()
    {
        if (_gameInput.IsInteractPressed() && _interactingPlayer != null && !_interactingPlayer.HasMovedSince(_playerStartPosition))
        {
            _holdProgress += Time.deltaTime / _holdTime;
            _holdSlider.value = _holdProgress;

            if (_holdProgress >= 1f)
            {
                CompleteHoldProcess();
            }
        }
        else
        {
            CancelHoldProcess();
        }
    }

    private void CompleteHoldProcess()
    {
        Transform waterBucketTransform = Instantiate(_waterBucketSO.prefab);
        waterBucketTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(_interactingPlayer);
        OnWaterCollected?.Invoke(this, EventArgs.Empty);

        ResetHoldProcess();
    }

    private void CancelHoldProcess()
    {
        ResetHoldProcess();
    }

    private void ResetHoldProcess()
    {
        _holdSlider.gameObject.SetActive(false);
        _holdSlider.value = 0f;
        _isHolding = false;
        _holdProgress = 0f;
        _interactingPlayer = null;
        AudioEventSystem.StopAudio("Well");
    }
}
