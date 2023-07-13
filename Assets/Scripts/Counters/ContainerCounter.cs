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
            // ��̨û����Ʒ���������û����Ʒʱ������һ����Ʒ���������
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnTakeKitchenObject?.Invoke();
        }
    }
}
