using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFactoryObject : FactoryObject {
    [SerializeField] private List<FactoryObjectSO> validFactoryObjectSO;

    private List<FactoryObjectSO> factoryObjectSOList;

    private void Awake() {
        factoryObjectSOList = new List<FactoryObjectSO>();
    }
    public bool TryAddItem(FactoryObjectSO factoryObjectSO) {
        if(!validFactoryObjectSO.Contains(factoryObjectSO)){
            //not a valid item
            return false;
        }
        if(factoryObjectSOList.Contains(factoryObjectSO)){
            return false;
        } else {
            factoryObjectSOList.Add(factoryObjectSO);
            return true;
        }
    }
}
