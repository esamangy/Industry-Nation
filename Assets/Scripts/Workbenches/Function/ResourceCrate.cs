using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCrate : BaseWorkbench, IFactoryObjectParent {

    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private FactoryObjectSO factoryObjectSO;
    public override void Interact(PlayerController player){
        if(!player.HasFactoryObject()){
            FactoryObject.SpawnFactoryObject(factoryObjectSO, player);
            
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty); 
        }  
    }

    public FactoryObjectSO GetFactoryObjectSO(){
        return factoryObjectSO;
    }
}
