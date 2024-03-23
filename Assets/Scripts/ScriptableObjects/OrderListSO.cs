using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Order List", menuName = "Order/Order List", order = 0)]
public class OrderListSO : ScriptableObject {

    public List<OrderSO> orderSOList;
}
