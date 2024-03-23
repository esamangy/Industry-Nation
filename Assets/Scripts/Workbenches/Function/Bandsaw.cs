using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandsaw : BaseWorkbench, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    [SerializeField] private BandsawRecipeSO[] bandsawRecipeSOArray;
    private float bandsawProgress;
    private GameInput gameInput;
    private Coroutine progressCoroutine;

    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    bandsawProgress = 0;
                    BandsawRecipeSO bandsawRecipeSO = GetBandsawRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = bandsawProgress / bandsawRecipeSO.bandsawProgressMax
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
            player.GetGameInput().OnInteractAlternateActionStopped += GameInput_OnInteractAlternateActionStopped;
            player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
            progressCoroutine = StartCoroutine(InteractAlertnateHold());
        }
    }

    private IEnumerator InteractAlertnateHold(){
        BandsawRecipeSO bandsawRecipeSO = GetBandsawRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());
        while(bandsawProgress < bandsawRecipeSO.bandsawProgressMax){
            bandsawProgress += Time.deltaTime;
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = bandsawProgress / bandsawRecipeSO.bandsawProgressMax
                });
            yield return null;
        }

        FactoryObjectSO outputFactoryObjectSO = GetOutputForInput(GetFactoryObject().GetFactoryObjectSO());

        GetFactoryObject().DestroySelf();
        
        FactoryObject.SpawnFactoryObject(outputFactoryObjectSO, this);
        bandsawProgress = 0;
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
        BandsawRecipeSO bandsawRecipeSO = GetBandsawRecipeSOWithInput(inputFactoryObjectSO);
        return bandsawRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        BandsawRecipeSO bandsawRecipeSO = GetBandsawRecipeSOWithInput(inputFactoryObjectSO);
        if(bandsawRecipeSO != null){
            return bandsawRecipeSO.output;
        } else {
            return null;
        }
    }

    private BandsawRecipeSO GetBandsawRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (BandsawRecipeSO bandsawRecipeSO in bandsawRecipeSOArray) {
            if(bandsawRecipeSO.input == inputFactoryObjectSO){
                return bandsawRecipeSO;
            }
        }
        return null;
    }
}
