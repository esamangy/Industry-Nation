using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI orderNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetOrderSO(DeliveryManager.OrderInfo orderInfo){
        orderNameText.text = orderInfo.dock.GetDockName() + " -- " + orderInfo.orderSO.orderName;

        ResetIcons();

        foreach (FactoryObjectSO factoryObjectSO in orderInfo.orderSO.factoryObjectSOList) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = factoryObjectSO.sprite;
        }
    }

    public void SetNoOrderSO(){
        orderNameText.text = "Waiting for order...";

        ResetIcons();
    }

    private void ResetIcons() {
        foreach (Transform child in iconContainer){
            if(child != iconTemplate){
                Destroy(child.gameObject);
            }
        }
    }
}
