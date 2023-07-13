using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void InteractOfButton_E(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            SoundManager.Instance.PlaySound(SoundType.trash, this.transform.position);
        }
    }
}
