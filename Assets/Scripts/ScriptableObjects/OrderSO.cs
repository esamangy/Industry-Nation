using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order", menuName = "Order/Order", order = 1)]
public class OrderSO : ScriptableObject {
    public List<FactoryObjectSO> factoryObjectSOList;
    public string orderName;
}
