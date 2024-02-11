using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : BaseWorkbench{

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnHammer;
    public class OnProgressChangedEventArgs : EventArgs{
        public float progressNormalized;
    }
    [SerializeField] private AnvilRecipeSO[] AnvilRecipeSOArray;
    private int anvilProgress;
    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    anvilProgress = 0;
                    AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                        progressNormalized = (float)anvilProgress / anvilRecipeSO.anvilProgressMax
                    });
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
            anvilProgress ++;

            OnHammer?.Invoke(this, EventArgs.Empty);

            AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                        progressNormalized = (float)anvilProgress / anvilRecipeSO.anvilProgressMax
                    });

            if(anvilProgress >= anvilRecipeSO.anvilProgressMax){
                FactoryObjectSO outputFactoryObjectSO = GetOutputForInput(GetFactoryObject().GetFactoryObjectSO());

                GetFactoryObject().DestroySelf();
                
                FactoryObject.SpawnFactoryObject(outputFactoryObjectSO, this);
                anvilProgress = 0;
            }
        }
    }
    private bool HasRecipeWithInput(FactoryObjectSO inputFactoryObjectSO) {
        AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(inputFactoryObjectSO);
        return anvilRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(inputFactoryObjectSO);
        if(anvilRecipeSO != null){
            return anvilRecipeSO.output;
        } else {
            return null;
        }
    }

    private AnvilRecipeSO GetAnvilRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (AnvilRecipeSO anvilRecipeSO in AnvilRecipeSOArray) {
            if(anvilRecipeSO.input == inputFactoryObjectSO){
                return anvilRecipeSO;
            }
        }
        return null;
    }
}
