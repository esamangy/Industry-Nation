using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelf : MonoBehaviour {

    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    [SerializeField] private Transform topOfShelf;
    
    private FactoryObject factoryObject;

    //testing
    [SerializeField] private MediumShelf secondMediumShelf;
    [SerializeField] private bool testing;

    private void Update() {
        if(testing && Input.GetKeyDown(KeyCode.T)){
            if(factoryObject != null){
                factoryObject.SetMediumShelf(secondMediumShelf);
            }
        }
    }
    public void Interact(){
        if(factoryObject == null){
            GameObject factoryObjectTransform = Instantiate(FactoryObjectSO.prefab, topOfShelf);
            factoryObjectTransform.GetComponent<FactoryObject>().SetMediumShelf(this);
        } else {

        }
        
    }

    public Transform GetFactoryObjectFollowTransform(){
        return topOfShelf;
    }

    public void SetFactoryObject(FactoryObject factoryObject){
        this.factoryObject = factoryObject;
    }

    public FactoryObject GetFactoryObject(){
        return factoryObject;
    }

    public void ClearFactoryObject(){
        factoryObject = null;
    }

    public bool HasFactoryObject(){
        return factoryObject != null;
    }
}
