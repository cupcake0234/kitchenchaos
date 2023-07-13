using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void InteractOfButton_E(Player player)
    {
        InteractType interactType = GetInteractType(player);
        switch (interactType)
        {
            case InteractType.doNothing:
                break;
            case InteractType.putDown:
                player.GetKitchenObject().SetKitchenObjectParent(this);
                break;
            case InteractType.pickUp:
                GetKitchenObject().SetKitchenObjectParent(player);
                break;
            case InteractType.handObjectToCounterPlate:
                putObjectToPlate(player.GetKitchenObject(), GetKitchenObject() as PlateKitchenObject);
                break;
            case InteractType.counterObjectToHandPlate:
                putObjectToPlate(GetKitchenObject(), player.GetKitchenObject() as PlateKitchenObject);
                break;
            default:
                break;
        }
    }

}
