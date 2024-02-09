using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryObject : MonoBehaviour {
    
    [SerializeField] private FactoryObjectSO factoryObjectSO;

    private MediumShelf mediumShelf;
    public FactoryObjectSO GetFactoryObjectSO(){
        return factoryObjectSO;
    }

    public void SetMediumShelf(MediumShelf mediumShelf){
        if(this.mediumShelf != null){
            this.mediumShelf.ClearFactoryObject();
        }

        this.mediumShelf = mediumShelf;

        if(mediumShelf.HasFactoryObject()){
            throw new Exception("Counter already has a FactoryObject!");
        }
        mediumShelf.SetFactoryObject(this);

        transform.parent = mediumShelf.GetFactoryObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public MediumShelf GetMediumShelf(){
        return mediumShelf;
    }
}
