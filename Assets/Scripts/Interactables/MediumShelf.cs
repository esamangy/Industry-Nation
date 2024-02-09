using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelf : MonoBehaviour, IFactoryObjectParent {

    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    [SerializeField] private Transform topOfShelf;
    
    private FactoryObject factoryObject;
    public void Interact(PlayerController player){
        if(factoryObject == null){
            GameObject factoryObjectTransform = Instantiate(FactoryObjectSO.prefab, topOfShelf);
            factoryObjectTransform.GetComponent<FactoryObject>().SetFactoryObjectParent(this);
        } else {
            factoryObject.SetFactoryObjectParent(player);
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
