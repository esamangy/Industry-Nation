using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactoryObjectParent {
    public Transform GetFactoryObjectFollowTransform();

    public void SetFactoryObject(FactoryObject factoryObject);

    public FactoryObject GetFactoryObject();

    public void ClearFactoryObject();

    public bool HasFactoryObject();
}
