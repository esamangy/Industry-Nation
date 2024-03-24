using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform orderTemplate;

    private void Awake() {
        orderTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManager_OnOrderSpawned;
        DeliveryManager.Instance.OnOrderCompleted += DeliveryManager_OnOrderCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual(){
        foreach (Transform child in container){
            if(child == orderTemplate){
                continue;
            } else {
                Destroy(child.gameObject);
            }
        }

        foreach(OrderSO orderSO in DeliveryManager.Instance.GetWaitingOrderSOList()){
            Transform orderTransform = Instantiate(orderTemplate, container);
            orderTransform.gameObject.SetActive(true);
            orderTransform.GetComponent<DeliveryManagerSingleUI>().SetOrderSO(orderSO);
        }
    }
}
