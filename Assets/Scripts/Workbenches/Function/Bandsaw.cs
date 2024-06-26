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
    private int progressMultiplier = 0;
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
                StopCoroutine(progressCoroutine);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }
        }
    }
    
    private IEnumerator InteractAlertnateHold(){
        BandsawRecipeSO bandsawRecipeSO = GetBandsawRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());
        while(bandsawProgress < bandsawRecipeSO.bandsawProgressMax){
            bandsawProgress += Time.deltaTime * progressMultiplier;
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
    
    public override void InteractAlternate(PlayerController player) {
        if(HasFactoryObject() && HasRecipeWithInput(GetFactoryObject().GetFactoryObjectSO())){
            player.GetGameInput().OnInteractAlternateActionStopped += GameInput_OnInteractAlternateActionStopped;
            player.OnSelectedShelfChanged += PlayerController_OnSelectedShelfChanged;
            if(progressMultiplier == 0){
                progressCoroutine = StartCoroutine(InteractAlertnateHold());
            }
            progressMultiplier ++;
        }
    }
    
    private void GameInput_OnInteractAlternateActionStopped(object sender, EventArgs e) {
        if(progressMultiplier == 1){
            StopCoroutine(progressCoroutine);
        }
        GameInput gameInput = sender as GameInput;
        gameInput.OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
        gameInput.GetComponent<PlayerController>().OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
        progressMultiplier --;
    }
    
    private void PlayerController_OnSelectedShelfChanged(object sender, PlayerController.OnSelectedShelfChangedEventArgs e) {
        if(e.selectedBench != this){
            if(progressMultiplier == 1){
                StopCoroutine(progressCoroutine);
            }
            PlayerController playerController = sender as PlayerController;
            playerController.OnSelectedShelfChanged -= PlayerController_OnSelectedShelfChanged;
            playerController.GetComponent<GameInput>().OnInteractAlternateActionStopped -= GameInput_OnInteractAlternateActionStopped;
            progressMultiplier --;
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
