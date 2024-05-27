using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : BaseUI {
    [SerializeField] private TextMeshProUGUI ordersDeliveredText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.IsGameOver()){
            Show();
            ordersDeliveredText.text = DeliveryManager.Instance.GetSuccessfulOrdersAmount().ToString();
        } else {
            Hide();
        }
    }
}
