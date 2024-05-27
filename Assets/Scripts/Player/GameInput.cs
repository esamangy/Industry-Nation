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
    public event EventHandler OnPausedAction;
    private PlayerInputActions playerInput;
    public static GameInput Instance{get; private set;}
    private void Awake() {
        Instance = this;
        playerInput = new PlayerInputActions();
        playerInput.Player.Enable();

        playerInput.Player.Interact.performed += Interact_performed;
        playerInput.Player.InteractAlternate.started += InteractAlternate_Started;
        playerInput.Player.InteractAlternate.canceled += InteractAlternate_Canceled;
        playerInput.Player.Pause.performed += Pause_Performed;
    }

    private void OnDestroy() {
        playerInput.Player.Interact.performed -= Interact_performed;
        playerInput.Player.InteractAlternate.started -= InteractAlternate_Started;
        playerInput.Player.InteractAlternate.canceled -= InteractAlternate_Canceled;
        playerInput.Player.Pause.performed -= Pause_Performed;

        playerInput.Dispose();
    }

    private void Pause_Performed(InputAction.CallbackContext context) {
        OnPausedAction?.Invoke(this, EventArgs.Empty);
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
