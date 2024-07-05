using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInput : MonoBehaviour{
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputActionMap ui;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractAlternateActionStopped;
    public event EventHandler OnPausedAction;
    private void Awake() {
        inputAsset = GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
        ui = inputAsset.FindActionMap("UI");

        player.Enable();
        ui.Enable();
        
        player.FindAction("Interact").performed += Interact_performed;
        player.FindAction("InteractAlternate").started += InteractAlternate_Started;
        player.FindAction("InteractAlternate").canceled += InteractAlternate_Canceled;
        player.FindAction("Pause").performed += Pause_Performed;
    }

    private void OnDestroy() {
        player.FindAction("Interact").performed -= Interact_performed;
        player.FindAction("InteractAlternate").started -= InteractAlternate_Started;
        player.FindAction("InteractAlternate").canceled -= InteractAlternate_Canceled;
        player.FindAction("Pause").performed -= Pause_Performed;

        ui.Enable();
        player.Disable();
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
        Vector2 inputVector = player.FindAction("Move").ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public InputActionAsset GetInputActionsAsset(){
        return inputAsset;
    }
}
