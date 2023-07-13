using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IProgressBar
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    private CuttingRecipeSO cuttingRecipeSO;

    public event System.Action<float> OnProgressChange;
    public event System.Action Oncut;

    public override void InteractOfButton_E(Player player)
    {
        InteractType interactType = GetInteractType(player);
        switch (interactType)
        {
            case InteractType.putDown:
                cuttingRecipeSO = GetCuttingRecipe(player.GetKitchenObject().GetKitchenObjectSO());
                // ��̨������Ʒ �� �����������Ʒʱ �� ���������Ʒ�ܹ���Ƭ������Ʒ���ڹ�̨��
                if (cuttingRecipeSO != null)
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    OnProgressChange?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
                }
                return;
            case InteractType.pickUp:
                // ��̨������Ʒ�������������Ʒʱ�����������Ʒ
                GetKitchenObject().SetKitchenObjectParent(player);
                return;
            case InteractType.counterObjectToHandPlate:
                putObjectToPlate(GetKitchenObject(), player.GetKitchenObject() as PlateKitchenObject);
                return;
            default:
                break;
        }
        //if (!HasKitchenObject() && player.HasKitchenObject())
        //{
        //    cuttingRecipeSO = GetCuttingRecipe(player.GetKitchenObject().GetKitchenObjectSO());
        //    // ��̨������Ʒ �� �����������Ʒʱ �� ���������Ʒ�ܹ���Ƭ������Ʒ���ڹ�̨��
        //    if ( cuttingRecipeSO != null)
        //    {
        //        player.GetKitchenObject().SetKitchenObjectParent(this);
        //        cuttingProgress = 0;
        //        OnProgressChange?.Invoke((float)cuttingProgress/cuttingRecipeSO.cuttingProgressMax);
        //    }
        //    return;
        //}
        //if (HasKitchenObject() && !player.HasKitchenObject())
        //{
        //    // ��̨������Ʒ�������������Ʒʱ�����������Ʒ
        //    GetKitchenObject().SetKitchenObjectParent(player);
        //    return;
        //}
    }

    public override void InteractOfButton_F(Player player)
    {
        // ��Ʒ��Ƭ
        if (HasKitchenObject())
        {
            KitchenObject kitchenObject = GetKitchenObject();
            cuttingRecipeSO = GetCuttingRecipe(kitchenObject.GetKitchenObjectSO());
            if (cuttingRecipeSO == null)
            {
                //Debug.LogError("�Ҳ���cuttingRecipeSO");
                return;
            }
            cuttingProgress++;
            OnProgressChange?.Invoke((float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax);
            Oncut?.Invoke();
            SoundManager.Instance.PlaySound(SoundType.chop, transform.position);
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO item in cuttingRecipeSOArray)
        {
            if (item.input == inputKitchenObjectSO)
            {
                return item;
            }
        }
        return null;
    }
}
