using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCrate : BaseWorkbench, IFactoryObjectParent {

    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    public override void Interact(PlayerController player){
        if(!HasFactoryObject()){
            GameObject factoryObjectTransform = Instantiate(FactoryObjectSO.prefab);
            factoryObjectTransform.GetComponent<FactoryObject>().SetFactoryObjectParent(player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }        
    }
}
