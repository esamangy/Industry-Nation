using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingDock : BaseWorkbench {
    [SerializeField] private string dockName = "Dock 1";
    public override void Interact(PlayerController player) {
        if(player.HasFactoryObject()){
            if(player.GetFactoryObject().TryGetBox(out BoxFactoryObject boxFactoryObject)){
                //only accepts boxes
                DeliveryManager.Instance.DeliverOrder(this, boxFactoryObject);
                player.GetFactoryObject().DestroySelf();
            }
        }
    }

    public string GetDockName() {
        return dockName;
    }
}
