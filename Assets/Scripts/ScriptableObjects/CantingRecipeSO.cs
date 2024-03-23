using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Canting Recipe", menuName = "Factory/Canting Recipe", order = 6)]
public class CantingRecipeSO : ScriptableObject {
    public FactoryObjectSO input;
    public FactoryObjectSO output;
    public float cantingTimerMax;
}
