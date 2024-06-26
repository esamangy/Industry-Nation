using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : BaseUI {
    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        
        Show();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsCountdownToStartActive()){
            Hide();
        }
    }
}
