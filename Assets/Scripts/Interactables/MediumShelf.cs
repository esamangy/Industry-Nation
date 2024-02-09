using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelf : MonoBehaviour {

    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    [SerializeField] private Transform topOfShelf;
    
    public void Interact(){
        Debug.Log("Interact!");
        GameObject pipeTransform = Instantiate(FactoryObjectSO.prefab, topOfShelf);
        pipeTransform.transform.localPosition = Vector3.zero;

        pipeTransform.GetComponent<FactoryObject>().GetFactoryObjectSO();
    }
}
