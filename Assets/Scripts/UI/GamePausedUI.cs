using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : BaseUI {
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private OptionsUI optionsMenu;
    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.ResumeGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            Hide();
            optionsMenu.Show(Show);
        });
    }

    private void Start() {
        GameManager.Instance.OnGamePausedStatus += GameManager_OnGamePausedStatus;

        Hide();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePausedStatus -= GameManager_OnGamePausedStatus;
    }

    private void GameManager_OnGamePausedStatus(object sender, GameManager.PauseStatusEventArgs e) {
        if(e.isPaused){
            Show();
        } else {
            Hide();
        }
    }

    public override void Show(){
        base.Show();
        resumeButton.Select();
    }
}
