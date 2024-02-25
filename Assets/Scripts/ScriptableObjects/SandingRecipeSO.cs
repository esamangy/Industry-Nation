using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sanding Recipe", menuName = "Factory/Sanding Recipe", order = 2)]
public class SandingRecipeSO : ScriptableObject {
    public FactoryObjectSO input;
    public FactoryObjectSO output;
    public float sandingTimerMax;
}
