using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxIconsSingleUI : MonoBehaviour {

    [SerializeField] private Image image;
    public void SetFactoryObjectSO(FactoryObjectSO factoryObjectSO){
        image.sprite = factoryObjectSO.sprite;
    }
}
