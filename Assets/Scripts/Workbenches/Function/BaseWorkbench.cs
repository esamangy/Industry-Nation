using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkbench : MonoBehaviour, IFactoryObjectParent {
    public static event EventHandler OnAnyObjectPlacedHere;
    public static void ResetStaticData(){
        OnAnyObjectPlacedHere = null;
    }
    [SerializeField] private Transform ItemHoldPoint;
    private FactoryObject factoryObject;

    public virtual void Interact(PlayerController player){
        Debug.LogError("BaseWorkbench.Interact();");
    }
    public virtual void InteractAlternate(PlayerController player){
        //Debug.LogError("BaseWorkbench.Interact();");
    }

    public Transform GetFactoryObjectFollowTransform(){
        return ItemHoldPoint;
    }

    public void SetFactoryObject(FactoryObject factoryObject){
        this.factoryObject = factoryObject;
        if(factoryObject != null){
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
        if(factoryObject.TryGetBox(out BoxFactoryObject boxFactoryObject)){
            boxFactoryObject.TryClose();
        }
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
