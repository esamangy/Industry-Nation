using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : BaseWorkbench, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private WorkbenchRecipeSO[] workbenchRecipeSOArray;
    private float workbenchProgress;
    private GameInput gameInput;
    private Coroutine progressCoroutine;
    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    workbenchProgress = 0;
                    WorkbenchRecipeSO anvilRecipeSO = GetWorkbenchRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = workbenchProgress / anvilRecipeSO.workbenchProgressMax
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

    private IEnumerator InteractAlertnateHold(){
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());
        while(workbenchProgress < workbenchRecipeSO.workbenchProgressMax){
            workbenchProgress += Time.deltaTime;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = workbenchProgress / workbenchRecipeSO.workbenchProgressMax
                });
            yield return null;
        }

        FactoryObjectSO outputFactoryObjectSO = GetOutputForInput(GetFactoryObject().GetFactoryObjectSO());

        GetFactoryObject().DestroySelf();
        
        FactoryObject.SpawnFactoryObject(outputFactoryObjectSO, this);
        workbenchProgress = 0;
    }

    public override void InteractAlternate(PlayerController player) {
        if(HasFactoryObject() && HasRecipeWithInput(GetFactoryObject().GetFactoryObjectSO())){
            player.GetGameInput().OnInteractAlternateActionStopped += GameInput_OnInteractAlternateActionStopped;
            player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
            progressCoroutine = StartCoroutine(InteractAlertnateHold());
        }
    }

    private void GameInput_OnInteractAlternateActionStopped(object sender, EventArgs e) {
        StopCoroutine(progressCoroutine);
    }

    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedBench != this){
            StopCoroutine(progressCoroutine);
        }
    }

    private bool HasRecipeWithInput(FactoryObjectSO inputFactoryObjectSO) {
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(inputFactoryObjectSO);
        return workbenchRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        WorkbenchRecipeSO workbenchRecipeSO = GetWorkbenchRecipeSOWithInput(inputFactoryObjectSO);
        if(workbenchRecipeSO != null){
            return workbenchRecipeSO.output;
        } else {
            return null;
        }
    }

    private WorkbenchRecipeSO GetWorkbenchRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (WorkbenchRecipeSO workbenchRecipeSO in workbenchRecipeSOArray) {
            if(workbenchRecipeSO.input == inputFactoryObjectSO){
                return workbenchRecipeSO;
            }
        }
        return null;
    }
}
