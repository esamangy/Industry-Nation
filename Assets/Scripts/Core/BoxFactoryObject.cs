using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFactoryObject : FactoryObject {
    private const string CLOSE = "Close";
    private const string OPEN = "Open";
    [SerializeField] private List<FactoryObjectSO> validFactoryObjectSO;
    private Animator animator;
    private List<FactoryObjectSO> factoryObjectSOList;
    private enum State{
        Open,
        Close
    }
    public event EventHandler<OnItemAddedEventArgs> OnItemAdded;
    public class OnItemAddedEventArgs : EventArgs {
        public FactoryObjectSO factoryObjectSO;
    }
    private State currentState;
    private void Awake() {
        factoryObjectSOList = new List<FactoryObjectSO>();
        animator = GetComponent<Animator>();
        currentState = State.Close;
    }
    public bool TryAddItem(FactoryObjectSO factoryObjectSO) {
        if(!validFactoryObjectSO.Contains(factoryObjectSO)){
            //not a valid item
            return false;
        }
        factoryObjectSOList.Add(factoryObjectSO);
        OnItemAdded?.Invoke(this, new OnItemAddedEventArgs{
            factoryObjectSO = factoryObjectSO
        });
        return true;
    }

    public List<FactoryObjectSO> GetFactoryObjectSOList(){
        return factoryObjectSOList;
    }

    public void TryOpen(){
        if(currentState == State.Open){
            return;
        }
        currentState = State.Open;
        animator.SetTrigger(OPEN);
    }

    public void TryClose(){
        if(currentState == State.Close){
            return;
        }
        currentState = State.Close;
        animator.SetTrigger(CLOSE);
    }

}
