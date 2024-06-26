using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInput : MonoBehaviour{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public enum Binding{
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
        Gamepad_Interact,
        Gamepad_Interact_Alt,
        Gamepad_Pause,
    }
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputActionMap ui;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnInteractAlternateActionStopped;
    public event EventHandler OnPausedAction;
    public event EventHandler OnBindingRebound;
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
    // public string GetBindingText(Binding binding){
    //     switch(binding){
    //         default:
    //         case Binding.Move_Up:
    //             return playerInput.Player.Move.bindings[1].ToDisplayString();
    //         case Binding.Move_Down:
    //             return playerInput.Player.Move.bindings[2].ToDisplayString();
    //         case Binding.Move_Left:
    //             return playerInput.Player.Move.bindings[3].ToDisplayString();
    //         case Binding.Move_Right:
    //             return playerInput.Player.Move.bindings[4].ToDisplayString();
    //         case Binding.Interact:
    //             return playerInput.Player.Interact.bindings[0].ToDisplayString();
    //         case Binding.Interact_Alt:
    //             return playerInput.Player.InteractAlternate.bindings[0].ToDisplayString();
    //         case Binding.Pause:
    //             return playerInput.Player.Pause.bindings[0].ToDisplayString();
    //         case Binding.Gamepad_Interact:
    //             return playerInput.Player.Interact.bindings[2].ToDisplayString();
    //         case Binding.Gamepad_Interact_Alt:
    //             return playerInput.Player.InteractAlternate.bindings[2].ToDisplayString();
    //         case Binding.Gamepad_Pause:
    //             return playerInput.Player.Pause.bindings[1].ToDisplayString();
    //     }
    // }

    // public void RebindBinding(Binding binding, Action onActionRebound){
    //     playerInput.Player.Disable();
    //     InputAction inputAction;
    //     int bindingIndex;
    //     switch(binding){
    //         default:
    //         case Binding.Move_Up:
    //             inputAction = playerInput.Player.Move;
    //             bindingIndex = 1;
    //             break;                
    //         case Binding.Move_Down:
    //             inputAction = playerInput.Player.Move;
    //             bindingIndex = 2;
    //             break; 
    //         case Binding.Move_Left:
    //             inputAction = playerInput.Player.Move;
    //             bindingIndex = 3;
    //             break; 
    //         case Binding.Move_Right:
    //             inputAction = playerInput.Player.Move;
    //             bindingIndex = 4;
    //             break; 
    //         case Binding.Interact:
    //             inputAction = playerInput.Player.Interact;
    //             bindingIndex = 0;
    //             break; 
    //         case Binding.Interact_Alt:
    //             inputAction = playerInput.Player.InteractAlternate;
    //             bindingIndex = 0;
    //             break; 
    //         case Binding.Pause:
    //             inputAction = playerInput.Player.Pause;
    //             bindingIndex = 0;
    //             break; 
    //         case Binding.Gamepad_Interact:
    //             inputAction = playerInput.Player.Interact;
    //             bindingIndex = 2;
    //             break; 
    //         case Binding.Gamepad_Interact_Alt:
    //             inputAction = playerInput.Player.InteractAlternate;
    //             bindingIndex = 2;
    //             break; 
    //         case Binding.Gamepad_Pause:
    //             inputAction = playerInput.Player.Pause;
    //             bindingIndex = 1;
    //             break; 
    //     }

    //     inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
    //         callback.Dispose();
    //         playerInput.Player.Enable();
    //         onActionRebound();

    //         PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInput.SaveBindingOverridesAsJson());
    //         PlayerPrefs.Save();

    //         OnBindingRebound?.Invoke(this, EventArgs.Empty);
    //     }).Start();
    // }

}
