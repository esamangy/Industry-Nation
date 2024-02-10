using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Anvil Recipe", menuName = "Factory/Anvil Recipe", order = 1)]
public class AnvilRecipeSO : ScriptableObject {
    public FactoryObjectSO input;
    public FactoryObjectSO output;
}
