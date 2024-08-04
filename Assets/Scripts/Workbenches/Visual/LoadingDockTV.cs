using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingDockTV : MonoBehaviour {
    [SerializeField] private LoadingDock loadingDock;
    [SerializeField] private TextMeshProUGUI loadingDockNameText;
    [SerializeField] private DeliveryManagerSingleUI deliveryManagerSingleUI;

    private void Start() {
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManager_OnOrderSpawned;
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        loadingDockNameText.text = loadingDock.GetDockName();
        UpdateVisual();
    }
    private void DeliveryManager_OnOrderSuccess(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, EventArgs e) {
        UpdateVisual();
    }
#if UNITY_EDITOR
    private void OnValidate() {
        loadingDockNameText.text = loadingDock.GetDockName();
    }
#endif

    private void UpdateVisual(){
        foreach(DeliveryManager.OrderInfo orderInfo in DeliveryManager.Instance.GetWaitingOrderInfos()){
            if(orderInfo.dock == loadingDock && orderInfo.orderSO != null){
                deliveryManagerSingleUI.SetOrderSO(orderInfo);
                return;
            }
        }

        deliveryManagerSingleUI.SetNoOrderSO();
    }
}
