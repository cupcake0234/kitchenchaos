using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public event System.Action<DishSO> OnDeliverDish;

    public override void InteractOfButton_E(Player player)
    {
        InteractType interactType = GetInteractType(player);
        if (interactType == InteractType.putDown && player.GetKitchenObject() is PlateKitchenObject)
        {
            OnDeliverDish?.Invoke((player.GetKitchenObject() as PlateKitchenObject).GetDishSO());
            player.GetKitchenObject().DestroySelf();
        }
    }
}