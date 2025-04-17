using System;
using UnityEngine;
using UnityEngine.UI;

public class WellCounter : BaseCounter, IHasProgress {
    public event EventHandler OnWaterCollected;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [SerializeField] private KitchenObjectSO _waterBucketSO;
    [SerializeField] private GameInput _gameInput;

    private const float _holdTime = 2f;
    private float _holdProgress = 0f;
    private bool _isHolding = false;
    private Player _interactingPlayer;
    private Vector3 _playerStartPosition;

    private void Start() {
        ResetHoldProcess();
    }

    public void Initialize(GameInput gameInput) {
        _gameInput = gameInput ?? throw new ArgumentNullException(nameof(gameInput));
    }

    public override void Interact(Player player) {
        if (!_isHolding && !player.HasKitchenObject()) {
            StartHoldProcess(player);
        }
    }

    private void StartHoldProcess(Player player) {
        _isHolding = true;
        _holdProgress = 0f;
        _interactingPlayer = player;
        _playerStartPosition = player.Getposition();

        // SFX awal ambil ramuan
        GetComponent<WellAudio>()?.PlayAmbilSound();


        AudioEventSystem.PlayAudio("Well");
    }

    private void Update() {
        if (_isHolding) {
            HandleHoldProgress();
        }
    }

    private void HandleHoldProgress() {
        if (_gameInput.IsInteractPressed() && _interactingPlayer != null && !_interactingPlayer.HasMovedSince(_playerStartPosition)) {
            _holdProgress += Time.deltaTime / _holdTime;

            // 🔥 Panggil event OnProgressChanged agar UI mengetahui perubahan progress
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = _holdProgress
            });

            if (_holdProgress >= 1f) {
                CompleteHoldProcess();
            }
        } else {
            CancelHoldProcess();
        }
    }

    private void CompleteHoldProcess() {
        Transform waterBucketTransform = Instantiate(_waterBucketSO.prefab);
        waterBucketTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(_interactingPlayer);
        OnWaterCollected?.Invoke(this, EventArgs.Empty);

        // Menambahkan Emitter menaruh ramuan
        GetComponent<WellAudio>()?.PlayTaruhSound();

        ResetHoldProcess();
    }

    private void CancelHoldProcess() {
        ResetHoldProcess();
    }

    private void ResetHoldProcess() {
        _isHolding = false;
        _holdProgress = 0f;
        _interactingPlayer = null;
        AudioEventSystem.StopAudio("Well");

        // 🔥 Beritahu UI bahwa progress reset ke 0
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = 0f
        });
    }
}
