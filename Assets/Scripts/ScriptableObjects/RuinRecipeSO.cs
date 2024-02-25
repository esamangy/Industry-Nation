using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ruin Recipe", menuName = "Factory/Ruin Recipe", order = 3)]
public class RuinRecipeSO : ScriptableObject {
    public FactoryObjectSO input;
    public FactoryObjectSO output;
    public float RuinTimerMax;
}
