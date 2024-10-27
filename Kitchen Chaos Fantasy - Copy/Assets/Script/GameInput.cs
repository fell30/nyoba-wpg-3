using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GameInput : MonoBehaviour
{

    private PlayerInputAction playerInputActions;
    public event EventHandler OnInteract;
    public event EventHandler OnInteractAlternate;



    private void Awake()
    {

        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;


    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternate?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);


    }
    // Start is called before the first frame update
    public Vector2 GetMovmentInputNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();


        inputVector.Normalize();

        return inputVector;


    }
    public bool IsInteractAlternatePressed()
    {
        return playerInputActions.Player.InteractAlternate.ReadValue<float>() > 0.1f;
    }

}
