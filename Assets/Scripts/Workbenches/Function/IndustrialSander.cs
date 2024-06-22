using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialSander : BaseWorkbench, IHasProgress {

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State{
        Idle,
        Sanding,
        Sanded,
        Ruined,
    }
    [SerializeField] private SandingRecipeSO[] sandingRecipeSOArray;
    [SerializeField] private RuinRecipeSO[] ruinRecipeSOArray;
    private State currentState;
    private float sandingTimer;
    private float ruinTimer;
    private SandingRecipeSO sandingRecipeSO;
    private RuinRecipeSO ruinRecipeSO;
    private void Start() {
        currentState = State.Idle;
    }
    private void Update() {
        if(HasFactoryObject()){
            switch(currentState){
                case State.Idle:
                    break;
                case State.Sanding:
                    sandingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = sandingTimer / sandingRecipeSO.sandingTimerMax
                    });
                    if(sandingTimer > sandingRecipeSO.sandingTimerMax){
                        // Sanded
                        GetFactoryObject().DestroySelf();

                        FactoryObject.SpawnFactoryObject(sandingRecipeSO.output, this);
                        ruinRecipeSO = GetRuinRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());
                        ChangeState(State.Sanded);
                        ruinTimer = 0f;
                    }
                    break;
                case State.Sanded:
                    ruinTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = ruinTimer / ruinRecipeSO.RuinTimerMax
                    });
                    if(ruinTimer > sandingRecipeSO.sandingTimerMax){
                        // ruined
                        GetFactoryObject().DestroySelf();

                        FactoryObject.SpawnFactoryObject(ruinRecipeSO.output, this);

                        ChangeState(State.Ruined);
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                            progressNormalized = 0f
                        });
                        }
                    break;
                case State.Ruined:
                    break;
            }
        }
    }
    public override void Interact(PlayerController player) {
        if(!HasFactoryObject()){
            if(player.HasFactoryObject()){
                if(HasRecipeWithInput(player.GetFactoryObject().GetFactoryObjectSO())){
                    player.GetFactoryObject().SetFactoryObjectParent(this);
                    
                    sandingRecipeSO = GetSandingRecipeSOWithInput(GetFactoryObject().GetFactoryObjectSO());

                    ChangeState(State.Sanding);
                    sandingTimer = 0f;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs{
                        progressNormalized = sandingTimer / sandingRecipeSO.sandingTimerMax
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
        currentState = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs{
            state = newState
        });
    }

    private bool HasRecipeWithInput(FactoryObjectSO inputFactoryObjectSO) {
        SandingRecipeSO sandingRecipeSO = GetSandingRecipeSOWithInput(inputFactoryObjectSO);
        return sandingRecipeSO != null;
    }

    private FactoryObjectSO GetOutputForInput(FactoryObjectSO inputFactoryObjectSO){
        SandingRecipeSO sandingRecipeSO = GetSandingRecipeSOWithInput(inputFactoryObjectSO);
        if(sandingRecipeSO != null){
            return sandingRecipeSO.output;
        } else {
            return null;
        }
    }

    private SandingRecipeSO GetSandingRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach (SandingRecipeSO sandingRecipeSO in sandingRecipeSOArray) {
            if(sandingRecipeSO.input == inputFactoryObjectSO){
                return sandingRecipeSO;
            }
        }
        return null;
    }

    private RuinRecipeSO GetRuinRecipeSOWithInput(FactoryObjectSO inputFactoryObjectSO){
        foreach(RuinRecipeSO ruinRecipeSO in ruinRecipeSOArray) {
            if(ruinRecipeSO.input == inputFactoryObjectSO){
                return ruinRecipeSO;
            }
        }
        return null;
    }

    public bool IsSanded(){
        return currentState == State.Sanded;
    }
}
