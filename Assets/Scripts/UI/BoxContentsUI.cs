using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxContentsUI : MonoBehaviour {

    [SerializeField] private BoxFactoryObject boxFactoryObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }
    private void Start() {
        boxFactoryObject.OnItemAdded += BoxFactoryObject_OnItemAdded;
    }
    private void BoxFactoryObject_OnItemAdded(object sender, BoxFactoryObject.OnItemAddedEventArgs e) {
        UpdateVisual();
    }
    private void UpdateVisual(){
        foreach (Transform child in transform) {
            if(child != iconTemplate){
                Destroy(child.gameObject);
            }
        }
        foreach (FactoryObjectSO factoryObjectSO in boxFactoryObject.GetFactoryObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<BoxIconsSingleUI>().SetFactoryObjectSO(factoryObjectSO);
        }
    }
}
