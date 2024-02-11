using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObject : MonoBehaviour {
    
    [SerializeField] private FactoryObjectSO factoryObjectSO;

    private IFactoryObjectParent factoryObjectParent;
    public FactoryObjectSO GetFactoryObjectSO(){
        return factoryObjectSO;
    }

    public void SetFactoryObjectParent(IFactoryObjectParent factoryObjectParent){
        if(this.factoryObjectParent != null){
            this.factoryObjectParent.ClearFactoryObject();
        }

        this.factoryObjectParent = factoryObjectParent;

        if(factoryObjectParent.HasFactoryObject()){
            throw new Exception("Counter already has a FactoryObject!");
        }
        factoryObjectParent.SetFactoryObject(this);

        transform.parent = factoryObjectParent.GetFactoryObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IFactoryObjectParent GetFactoryObjectParent(){
        return factoryObjectParent;
    }

    public void DestroySelf(){
        factoryObjectParent.ClearFactoryObject();
        
        Destroy(gameObject);
    }

    public static FactoryObject SpawnFactoryObject(FactoryObjectSO factoryObjectSO, IFactoryObjectParent factoryObjectParent) {
        GameObject factoryObjectTransform = Instantiate(factoryObjectSO.prefab);
        FactoryObject factoryObject = factoryObjectTransform.GetComponent<FactoryObject>();
        factoryObject.SetFactoryObjectParent(factoryObjectParent);

        return factoryObject;
    }
}
