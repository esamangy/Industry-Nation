using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : BaseWorkbench{

    [SerializeField] private AnvilRecipeSO[] AnvilRecipeSOArray;
    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                }
                
            }
        } else {
            if(!player.HasFactoryObject()){
                GetFactoryObject().SetFactoryObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(PlayerController player) {
        if(HasFactoryObject() && HasRecipeWithInput(GetFactoryObject().GetFactoryObjectSO())){
            FactoryObjectSO outputFactoryObjectSO = GetOutputForInput(GetFactoryObject().GetFactoryObjectSO());

            GetFactoryObject().DestroySelf();
            
            FactoryObject.SpawnFactoryObject(outputFactoryObjectSO, this);
        }
    }
    private bool HasRecipeWithInput(FactoryObjectSO inputFactoryObjectSO) {
        foreach (AnvilRecipeSO anvilRecipeSO in AnvilRecipeSOArray) {
            if(anvilRecipeSO.input == inputFactoryObjectSO){
                return true;
            }
        }
        return false;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (AnvilRecipeSO anvilRecipeSO in AnvilRecipeSOArray) {
            if(anvilRecipeSO.input == inputFactoryObjectSO){
                return anvilRecipeSO.output;
            }
        }
        return null;
    }
}
