using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Factory Objects", menuName = "Factory/Basic Factory Object")]
public class FactoryObjectSO : ScriptableObject {
    
    public GameObject prefab;
    public Sprite sprite;
    public string objectName;
}
