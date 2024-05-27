using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderCompleted;
    public event EventHandler OnOrderSuccess;
    public event EventHandler OnOrderFailed;
    public static DeliveryManager Instance{get; private set;}
    [SerializeField] private OrderListSO orderListSO;
    private List<OrderSO> waitingOrderSOList;
    private int waitingOrdersMax = 5;
    private float spawnOrderTimer;
    private float spawnOrderTimerMax = 5f;
    private int successfulOrdersAmount;
    private void Awake() {
        Instance = this;
        waitingOrderSOList = new List<OrderSO>();
    }
    private void Update() {
        spawnOrderTimer -= Time.deltaTime;
        if(spawnOrderTimer <= 0f){
            spawnOrderTimer = spawnOrderTimerMax;

            if(waitingOrderSOList.Count < waitingOrdersMax){
                OrderSO waitingOrderSO = orderListSO.orderSOList[UnityEngine.Random.Range(0, orderListSO.orderSOList.Count)];
                waitingOrderSOList.Add(waitingOrderSO);

                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverOrder(BoxFactoryObject boxFactoryObject) {
        for(int i = 0; i < waitingOrderSOList.Count; i ++){
            OrderSO waitingOrderSO = waitingOrderSOList[i];

            if(waitingOrderSO.factoryObjectSOList.Count != boxFactoryObject.GetFactoryObjectSOList().Count){
                // check if the recipe has the same number of pieces, continue if not
                continue;
            }
            Dictionary<FactoryObjectSO, int> deliveredOrderPieces = new Dictionary<FactoryObjectSO, int>();
            foreach (FactoryObjectSO item in boxFactoryObject.GetFactoryObjectSOList()) {
                if(deliveredOrderPieces.ContainsKey(item)){
                    deliveredOrderPieces[item] ++;
                } else {
                    deliveredOrderPieces.Add(item, 1);
                }
            }
            Dictionary<FactoryObjectSO, int> waitingOrderPieces = new Dictionary<FactoryObjectSO, int>();
            foreach (FactoryObjectSO item in waitingOrderSO.factoryObjectSOList) {
                if(waitingOrderPieces.ContainsKey(item)){
                    waitingOrderPieces[item] ++;
                } else {
                    waitingOrderPieces.Add(item, 1);
                }
            }
            bool deliveredMatchesWaiting = true;
            foreach (KeyValuePair<FactoryObjectSO, int> deliveredPiece in deliveredOrderPieces) {
                if(waitingOrderPieces.ContainsKey(deliveredPiece.Key)){
                    // the delivered object has at least one of the same ingredient being checked
                    if(waitingOrderPieces[deliveredPiece.Key] == deliveredPiece.Value){
                        //the delivered objetc has the same number of the piece being checked
                    } else {
                        //the delivered object has a different number of this piece and there is no point in continuing to check order
                        deliveredMatchesWaiting = false;
                        break;
                    }
                } else {
                    //the delivered object does not have this piece and there is no point in continuing to check order
                    deliveredMatchesWaiting = false;
                    break;
                }
            }
            if(deliveredMatchesWaiting){
                //player deliered a correct order
                successfulOrdersAmount ++;

                waitingOrderSOList.RemoveAt(i);

                OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                OnOrderSuccess?.Invoke(this, EventArgs.Empty);

                return;
            }
        }
        //player deliered an incorrect order
        OnOrderFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<OrderSO> GetWaitingOrderSOList(){
        return waitingOrderSOList;
    }

    public int GetSuccessfulOrdersAmount(){
        return successfulOrdersAmount;
    }
}
