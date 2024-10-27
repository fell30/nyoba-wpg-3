using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour, IkitchenObjectParent
{

    public static Player Instance { get; private set; }



    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    public bool IsInteractAlternatePressed()
    {
        return gameInput.IsInteractAlternatePressed();
    }

    public float speed = 5f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float PlayerRadius = .5f;
    [SerializeField] private float PlayerHeight = 2f;
    [SerializeField] private LayerMask layerMaskCounter;
    [SerializeField] private Transform KitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 LastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private bool wasWalking = false;


    private void Awake()
    {
        if (Instance != null)
        {
            UnityEngine.Debug.LogError("There is more than one player instance in the scene");

        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteract += GameInput_OnInteract;
        gameInput.OnInteractAlternate += GameInput_OnInteractAlternate;
    }

    private void GameInput_OnInteractAlternate(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }
    private void Update()
    {
        HandleMoveMent();
        HandleInteraction();
        HandleFootstepSound();

    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleFootstepSound()
    {
        if (isWalking && !wasWalking)
        {
            // Pemain mulai berjalan
            AudioEventSystem.PlayAudio("Footstep");
        }
        else if (!isWalking && wasWalking)
        {
            // Pemain berhenti
            AudioEventSystem.StopAudio("Footstep");
        }

        wasWalking = isWalking; // Update status wasWalking
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
                MoveDir = MoveDirX; // Jika bisa bergerak di x, gunakan arah ini
            }
            else
            {
                Vector3 MoveDirZ = new Vector3(0f, 0f, MoveDir.z).normalized;
                CanMove = MoveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PlayerHeight, PlayerRadius, MoveDirZ, MoveDistance);

                if (CanMove)
                {
                    MoveDir = MoveDirZ; // Jika bisa bergerak di z, gunakan arah ini
                }

            }

        }

        // Setelah pengecekan, jika CanMove true, player bergerak
        if (CanMove)
        {
            transform.position += MoveDir * MoveDistance; // Gerakkan player
        }

        float rotationSpeed = 15f;
        isWalking = MoveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, MoveDir, Time.deltaTime * rotationSpeed);


    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    //Metode untuk mendapatkan Posisi Player
    public Vector3 Getposition()
    {
        return transform.position;
    }
    // Tambahan metode untuk memeriksa apakah pemain telah bergerak sejak posisi tertentu
    public bool HasMovedSince(Vector3 startPosition)
    {
        return Vector3.Distance(startPosition, transform.position) > 0.01f; // Threshold pergerakan
    }

    //IkitcheObjectParent
    public Transform GetKitchenObjectFollowTransform()
    {
        return KitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }



}
