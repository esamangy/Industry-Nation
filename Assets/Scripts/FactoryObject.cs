using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObject : MonoBehaviour {
    
    [SerializeField] private FactoryObjectSO factoryObjectSO;
    public FactoryObjectSO GetFactoryObjectSO(){
        return factoryObjectSO;
    }
}
