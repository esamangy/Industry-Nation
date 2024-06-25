using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : BaseUI {
    [SerializeField] private TextMeshProUGUI keyboardMoveUpText;
    [SerializeField] private TextMeshProUGUI keyboardMoveDownText;
    [SerializeField] private TextMeshProUGUI keyboardMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyboardMoveRightText;
    [SerializeField] private TextMeshProUGUI keyboardInteractText;
    [SerializeField] private TextMeshProUGUI keyboardInteractAltText;
    [SerializeField] private TextMeshProUGUI keyboardPauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    private void Start() {
        //GameInput.Instance.OnBindingRebound += GameInput_OnBindingRebound;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        UpdateVisual();
        
        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsCountdownToStartActive()){
            Hide();
        }
    }

    private void GameInput_OnBindingRebound(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual(){
        // keyboardMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        // keyboardMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        // keyboardMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        // keyboardMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        // keyboardInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        // keyboardPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alt);
        // keyboardPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        // gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        // gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact_Alt);
        // gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }
}
