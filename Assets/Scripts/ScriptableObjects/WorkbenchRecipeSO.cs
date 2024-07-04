using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Workbench Recipe", menuName = "Factory/Workbench Recipe", order = 4)]
public class WorkbenchRecipeSO : ScriptableObject {
    public FactoryObjectSO[] inputs;
    public FactoryObjectSO output;
    public float workbenchProgressMax;
}
