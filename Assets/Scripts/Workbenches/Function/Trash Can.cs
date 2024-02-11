using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : BaseWorkbench {


    public override void Interact(PlayerController player) {
        if(player.HasFactoryObject()){
            player.GetFactoryObject().DestroySelf();
        }
    }
}
