using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWorkbench : MonoBehaviour, IFactoryObjectParent {
    public static event EventHandler OnAnyObjectPlacedHere;
    public event EventHandler OnObjectTakenFromHere;
    public event EventHandler<PlayerEventArgs> OnInteract;
    public event EventHandler<PlayerEventArgs> OnInteractAlt;
    public class PlayerEventArgs : EventArgs {
        public PlayerController player;
    }
    public static void ResetStaticData(){
        OnAnyObjectPlacedHere = null;
    }
    [SerializeField] private Transform ItemHoldPoint;
    private FactoryObject factoryObject;

    public virtual void Interact(PlayerController player){
        OnInteract?.Invoke(this, new PlayerEventArgs{
            player = player
        });
    }
    public virtual void InteractAlternate(PlayerController player){
        OnInteractAlt?.Invoke(this, new PlayerEventArgs{
            player = player
        });
    }

    public Transform GetFactoryObjectFollowTransform(){
        return ItemHoldPoint;
    }

    public virtual void SetFactoryObject(FactoryObject factoryObject){
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

    protected void FireOnObjectTakenFromHere(){
        OnObjectTakenFromHere?.Invoke(this, EventArgs.Empty);
    }
}
