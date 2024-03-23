using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bandsaw Recipe", menuName = "Factory/Bandsaw Recipe", order = 5)]
public class BandsawRecipeSO : ScriptableObject {
    public FactoryObjectSO input;
    public FactoryObjectSO output;
    public float bandsawProgressMax;
}
