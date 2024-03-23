using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Workbench Recipe", menuName = "Factory/Workbench Recipe", order = 4)]
public class WorkbenchRecipeSO : ScriptableObject {
    public List<FactoryObjectSO> input;
    public List<FactoryObjectSO> output;
    public float workbenchProgressMax;
}
