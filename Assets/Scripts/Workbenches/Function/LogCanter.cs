using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCanter : BaseWorkbench, IHasProgress {
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State{
        Idle,
        Canting,
        Canted,
    }
    [SerializeField] private CantingRecipeSO[] cantingRecipeSOArray;
    private State state;
    private float cantingTimer;
    private CantingRecipeSO cantingRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        if(HasFactoryObject()){
            switch(state){
                case State.Idle:
                    break;
                case State.Canting:
                    cantingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = cantingTimer / cantingRecipeSO.cantingTimerMax
                    });
                    if(cantingTimer > cantingRecipeSO.cantingTimerMax){
                        // Sanded
                        GetFactoryObject().DestroySelf();

                        FactoryObject.SpawnFactoryObject(cantingRecipeSO.output, this);;
                        ChangeState(State.Canted);
                    }
                    break;
                case State.Canted:
                    break;
            }
        }
    }

    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    
                    cantingRecipeSO = GetCantingRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    ChangeState(State.Canting);
                    cantingTimer = 0f;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = cantingTimer / cantingRecipeSO.cantingTimerMax
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

                        ChangeState(State.Idle);

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f
                        });
                    }
                }
            } else {
                //the player is not carrying anything
                GetFactoryObject().SetFactoryObjectParent(player);

                ChangeState(State.Idle);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                    progressNormalized = 0f
                });
            }
        }
    }

    private void ChangeState(State newState){
        state = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
            state = newState
        });
    }

    private bool HasRecipeWithInput(FactoryObjectSO inputFactoryObjectSO) {
        CantingRecipeSO cantingRecipeSO = GetCantingRecipeSOWithInput(inputFactoryObjectSO);
        return cantingRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        CantingRecipeSO cantingRecipeSO = GetCantingRecipeSOWithInput(inputFactoryObjectSO);
        if(cantingRecipeSO != null){
            return cantingRecipeSO.output;
        } else {
            return null;
        }
    }

    private CantingRecipeSO GetCantingRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (CantingRecipeSO cantingRecipeSO in cantingRecipeSOArray) {
            if(cantingRecipeSO.input == inputFactoryObjectSO){
                return cantingRecipeSO;
            }
        }
        return null;
    }
}
