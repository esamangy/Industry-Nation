using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : BaseUI {
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    private Action onCloseButtonAction;
    private void Awake() {
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });
    }

    private void Start() {
        GameManager.Instance.OnGamePausedStatus += GameManager_OnGamePausedStatus;
        Hide();
    }
    private void OnDestroy() {
        GameManager.Instance.OnGamePausedStatus -= GameManager_OnGamePausedStatus;
    }

    public override void Show(){
        base.Show();
        soundEffectsButton.Select();
    }
    public void Show(Action onCloseButtonAction){
        this.onCloseButtonAction = onCloseButtonAction;
        Show();
    }

    private void GameManager_OnGamePausedStatus(object sender, GameManager.PauseStatusEventArgs e) {
        if(!e.isPaused){
            Hide();
        }
    }

    private void UpdateVisual(){
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }
}
