using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDock : BaseWorkbench {
    public override void Interact(PlayerController player) {
        if(player.HasFactoryObject()){
            if(player.GetFactoryObject().TryGetBox(out BoxFactoryObject boxFactoryObject)){
                //only accepts boxes
                DeliveryManager.Instance.DeliverOrder(boxFactoryObject);
                player.GetFactoryObject().DestroySelf();
            }
        }
    }
}
