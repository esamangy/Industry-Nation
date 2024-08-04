using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
    [Serializable]
    public struct OrderInfo{
        public LoadingDock dock;
        public OrderSO orderSO;
    }
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderCompleted;
    public event EventHandler<DeliveryEventArgs> OnOrderSuccess;
    public event EventHandler<DeliveryEventArgs> OnOrderFailed;
    public static DeliveryManager Instance{get; private set;}
    [SerializeField] private OrderListSO orderListSO;
    [SerializeField] private OrderInfo[] orderInfos;
    private int waitingOrdersMax = 5;
    private int successfulOrdersAmount;
    public class DeliveryEventArgs : EventArgs {
        public LoadingDock loadingDock;
    }
    private void Awake() {
        Instance = this;
        LoadingDock[] docks = GameObject.FindObjectsOfType<LoadingDock>();
        orderInfos = new OrderInfo[docks.Length];
        for(int i = 0; i < orderInfos.Length; i ++){
            orderInfos[i].dock = docks[i];
            orderInfos[i].orderSO = null;
        }
    }

    public void DeliverOrder(LoadingDock dockDeliveredTo, BoxFactoryObject boxFactoryObject) {
        for(int i = 0; i < orderInfos.Length; i ++){
            OrderSO waitingOrderSO = orderInfos[i].orderSO;
            if(waitingOrderSO == null){
                continue;
            }
            if(dockDeliveredTo != orderInfos[i].dock){
                continue;
            }
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

                orderInfos[i].orderSO = null;

                OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                OnOrderSuccess?.Invoke(this, new DeliveryEventArgs{
                    loadingDock = dockDeliveredTo,
                });

                return;
            }
        }
        //player deliered an incorrect order
        OnOrderFailed?.Invoke(this, new DeliveryEventArgs{
            loadingDock = dockDeliveredTo,
        });
    }

    public OrderInfo[] GetWaitingOrderInfos(){
        return orderInfos;
    }

    public int GetSuccessfulOrdersAmount(){
        return successfulOrdersAmount;
    }

    public OrderListSO GetOrderListSO(){
        return orderListSO;
    }

    public int GetWaitingOrdersMax(){
        return waitingOrdersMax;
    }

    public bool IsWaitingListFull(){
        foreach (OrderInfo orderInfo in orderInfos) {
            if(orderInfo.orderSO == null){
                return false;
            }
        }
        return true;
    }

    public void AddOrderToWaitList(){
        if(IsWaitingListFull()){
            return;
        }

        OrderSO waitingOrderSO = orderListSO.orderSOList[UnityEngine.Random.Range(0, orderListSO.orderSOList.Count)];

        List<OrderInfo> valid = new List<OrderInfo>();
        foreach (OrderInfo orderInfo in orderInfos) {
            if(orderInfo.orderSO == null){
                valid.Add(orderInfo);
            }
        }

        OrderInfo chosen = valid[UnityEngine.Random.Range(0, valid.Count)];
        chosen.orderSO = waitingOrderSO;

        for(int i = 0; i < orderInfos.Length; i ++) {
            if(orderInfos[i].dock == chosen.dock){
                OrderInfo temp = new OrderInfo {
                    dock = orderInfos[i].dock,
                    orderSO = waitingOrderSO
                };
                orderInfos[i] = temp;
            }
        }

        OnOrderSpawned?.Invoke(this, EventArgs.Empty);
    }
}
