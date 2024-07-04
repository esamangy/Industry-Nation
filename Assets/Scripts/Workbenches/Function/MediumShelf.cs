using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumShelf : BaseWorkbench {
    [SerializeField] private FactoryObjectSO FactoryObjectSO;
    public override void Interact(PlayerController player){
        if(!HasFactoryObject()){
            //No factory object here
            if(player.HasFactoryObject()){
                //player is carrying something
                player.GetFactoryObject().SetFactoryObjectParent(this);
            }
        } else {
            //there is a factory object here
            if(player.HasFactoryObject()){
                //player is carrying something
                if(player.GetFactoryObject().TryGetBox(out BoxFactoryObject boxFactoryObject)){
                    //player is holding a box
                    if(boxFactoryObject.TryAddItem(GetFactoryObject().GetFactoryObjectSO())){
                        GetFactoryObject().DestroySelf();
                    }
                } else {
                    //player is not holding box but something else
                    if(GetFactoryObject().TryGetBox(out boxFactoryObject)){
                        //shelf is holding a box
                        if(boxFactoryObject.TryAddItem(player.GetFactoryObject().GetFactoryObjectSO())) {
                            player.GetFactoryObject().DestroySelf();
                        }
                    }
                }
            } else {
                //player is not carrying anything
                GetFactoryObject().SetFactoryObjectParent(player);
                FireOnObjectTakenFromHere();
            }
        }
    }
}
