using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anvil : BaseWorkbench, IHasProgress{
    public static event EventHandler OnAnyHammer;
    public event EventHandler OnHammer;
    new public static void ResetStaticData(){
        OnAnyHammer = null;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private AnvilRecipeSO[] AnvilRecipeSOArray;
    private int anvilProgress;
    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    anvilProgress = 0;
                    AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = (float)anvilProgress / anvilRecipeSO.anvilProgressMax
                    });
                }
                
            }
        } else {
            //there is a factory object here
            if(player.HasFactoryObject()){
                //the player is carrying something
                if(player.GetFactoryObject().TryGetBox(out BoxFactoryObject boxFactoryObject)){
                    //player is holding a box
                    if(boxFactoryObject.TryAddItem(GetFactoryObject().GetFactoryObjectSO())){
                        GetFactoryObject().DestroySelf();
                    }
                }
            } else {
                //the player is not carrying something
                GetFactoryObject().SetFactoryObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(PlayerController player) {
        if(HasFactoryObject() && HasRecipeWithInput(GetFactoryObject().GetFactoryObjectSO())){
            anvilProgress ++;

            OnAnyHammer?.Invoke(this, EventArgs.Empty);
            OnHammer?.Invoke(this, EventArgs.Empty);

            AnvilRecipeSO anvilRecipeSO = GetAnvilRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
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
