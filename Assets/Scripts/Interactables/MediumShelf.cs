using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelf : BaseWorkbench {
    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    public override void Interact(PlayerController player){
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                player.GetFactoryObject().SetFactoryObjectParent(this);
            }
        } else {
            if(!player.HasFactoryObject()){
                GetFactoryObject().SetFactoryObjectParent(player);
            }
        }
    }


}
