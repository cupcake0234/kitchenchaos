using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event System.Action OnTakeKitchenObject;

    public override void InteractOfButton_E(Player player)
    {
        if (!HasKitchenObject() && !player.HasKitchenObject())
        {
            // 柜台没有物品且玩家手中没有物品时，生成一个物品并交给玩家
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnTakeKitchenObject?.Invoke();
        }
    }
}
