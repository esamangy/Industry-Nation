using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{
    //Events
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractAlternateActionStopped;
    private PlayerInputActions playerInput;
    private void Awake() {
        playerInput = new PlayerInputActions();
        playerInput.Player.Enable();

        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.InteractAlternate.started += InteractAlternate_Started;
        playerInput.Player.InteractAlternate.canceled += InteractAlternate_Canceled;
    }

    private void InteractAlternate_Canceled(InputAction.CallbackContext context) {
        OnInteractAlternateActionStopped?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_Started(InputAction.CallbackContext context) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext context) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized(){
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

}
