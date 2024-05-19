using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : BaseWorkbench {

    public static event EventHandler OnAnyObjectTrashed;
    public override void Interact(PlayerController player) {
        if(player.HasFactoryObject()){
            player.GetFactoryObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
