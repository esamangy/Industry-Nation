using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkbench : MonoBehaviour, IFactoryObjectParent {
    [SerializeField] private Transform ItemHoldPoint;
    private FactoryObject factoryObject;

    public virtual void Interact(PlayerController player){
        Debug.LogError("BaseWorkbench.Interact();");
    }

    public Transform GetFactoryObjectFollowTransform(){
        return ItemHoldPoint;
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
