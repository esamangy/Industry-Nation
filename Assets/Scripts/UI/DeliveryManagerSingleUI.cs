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
    public void SetOrderSO(OrderSO orderSO){
        orderNameText.text = orderSO.orderName;

        foreach (Transform child in iconContainer){
            if(child == iconTemplate){
                continue;
            } else {
                Destroy(child.gameObject);
            }
        }

        foreach (FactoryObjectSO factoryObjectSO in orderSO.factoryObjectSOList) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = factoryObjectSO.sprite;
        }
    }
}
