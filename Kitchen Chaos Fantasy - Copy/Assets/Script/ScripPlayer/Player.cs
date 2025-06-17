using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IkitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public bool IsInteractAlternatePressed() => gameInput.IsInteractAlternatePressed();
    public bool IsInteractPressed() => gameInput.IsInteractPressed();

    public float speed = 5f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float PlayerRadius = .5f;
    [SerializeField] private float PlayerHeight = 2f;
    [SerializeField] private LayerMask layerMaskCounter;
    [SerializeField] private Transform KitchenObjectHoldPoint;
    [SerializeField] private ParticleSystem footstepParticleSystem;

    // Dash Variables
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;

    private bool isWalking;
    private bool isCooking;
    private Vector3 LastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private bool wasWalking = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance in the scene");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteract += GameInput_OnInteract;
        gameInput.OnInteractAlternate += GameInput_OnInteractAlternate;
    }

    private void Update()
    {
        HandleDash();
        if (!isDashing)
        {
            HandleMoveMent();
        }
        HandleInteraction();
        HandleFootstepSound();
    }

    private void GameInput_OnInteractAlternate(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteract(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    public bool IsWalking() => isWalking;
    public bool IsCooking() => isCooking;
    public void SetIsCooking(bool cooking) => isCooking = cooking;

    private void HandleFootstepSound()
    {
        if (isWalking && !wasWalking)
        {
            AudioEventSystem.PlayAudio("Footstep");
            footstepParticleSystem?.Play();
        }
        else if (!isWalking && wasWalking)
        {
            AudioEventSystem.StopAudio("Footstep");
            footstepParticleSystem?.Stop();
        }

        wasWalking = isWalking;
    }

    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovmentInputNormalized();
        Vector3 MoveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (MoveDir != Vector3.zero)
        {
            LastInteractDir = MoveDir;
        }

        float InteractDistance = 2f;

        if (Physics.Raycast(transform.position, LastInteractDir, out RaycastHit raycasthit, InteractDistance, layerMaskCounter))
        {
            if (raycasthit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMoveMent()
    {
        Vector2 inputVector = gameInput.GetMovmentInputNormalized();
        Vector3 MoveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float MoveDistance = speed * Time.deltaTime;

        bool CanMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDir, MoveDistance);

        if (!CanMove)
        {
            Vector3 MoveDirX = new Vector3(inputVector.x, 0f, 0f).normalized;
            CanMove = MoveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirX, MoveDistance);

            if (CanMove)
            {
                MoveDir = MoveDirX;
            }
            else
            {
                Vector3 MoveDirZ = new Vector3(0f, 0f, MoveDir.z).normalized;
                CanMove = MoveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirZ, MoveDistance);

                if (CanMove)
                {
                    MoveDir = MoveDirZ;
                }
            }
        }

        if (CanMove)
        {
            transform.position += MoveDir * MoveDistance;
        }

        float rotationSpeed = 15f;
        isWalking = MoveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, MoveDir, Time.deltaTime * rotationSpeed);
    }

    private void HandleDash()
    {
        if (isDashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
            return;
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f)
        {
            Vector2 inputVector = gameInput.GetMovmentInputNormalized();
            Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y);

            if (inputDir != Vector3.zero)
            {
                dashDirection = inputDir.normalized;
                isDashing = true;
                dashTimer = dashDuration;

                // Optional: play audio/particle
                AudioEventSystem.PlayAudio("Dash");
            }
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Vector3 Getposition() => transform.position;

    public bool HasMovedSince(Vector3 startPosition)
    {
        return Vector3.Distance(startPosition, transform.position) > 0.01f;
    }

    // KitchenObjectParent interface implementation
    public Transform GetKitchenObjectFollowTransform() => KitchenObjectHoldPoint;
    public void SetKitchenObject(KitchenObject kitchenObject) => this.kitchenObject = kitchenObject;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void ClearKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
}
