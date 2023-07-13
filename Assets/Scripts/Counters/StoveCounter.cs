using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IProgressBar
{
    [SerializeField] private CookingRecipeSO[] cookingRecipeSOArray;
    [SerializeField] private KitchenObjectSO meatPattyBurned;
    [SerializeField] private LoopSoundPlayer loopSoundPlayer;
    private CookingRecipeSO cookingRecipeSO;
    private float cookingTimer;

    public event System.Action<bool> OnCookingStateChange;
    public event System.Action<float> OnProgressChange;
    public event System.Action OnBeginWarning;
    public event System.Action OnStopWarning;

    protected override void Start()
    {
        base.Start();
        UpdateEventState(true);
    }

    private void Update()
    {
        if (HasKitchenObject() && cookingRecipeSO != null)
        {
            if (cookingRecipeSO.output == meatPattyBurned)
            {
                OnBeginWarning?.Invoke();
            }
            cookingTimer += Time.deltaTime;
            OnProgressChange?.Invoke(cookingTimer / cookingRecipeSO.cookingTimeMax);
            if (cookingTimer >= cookingRecipeSO.cookingTimeMax)
            {
                // ����ԭ�е����壬�������,������cookingRecepeSO
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(cookingRecipeSO.output, this);
                cookingRecipeSO = GetCookingRecipe(GetKitchenObject().GetKitchenObjectSO());
                UpdateEventState(cookingRecipeSO == null);
            }
        }
    }

    public override void InteractOfButton_E(Player player)
    {
        InteractType interactType = GetInteractType(player);

        switch (interactType)
        {
            case InteractType.putDown:
                cookingRecipeSO = GetCookingRecipe(player.GetKitchenObject().GetKitchenObjectSO());
                // ��̨������Ʒ �� �����������Ʒʱ �� ���������Ʒ�ܹ���⿣�����Ʒ���ڹ�̨��
                if (cookingRecipeSO != null)
                {
                    UpdateEventState(false);
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                return;
            case InteractType.pickUp:
                // ��̨������Ʒ�������������Ʒʱ�����������Ʒ��
                // �ҽ�cookingRecipeSO�ÿգ���Ȼ�´η���ʱ��ֱ�Ӵ�Uncooked��ΪBurned
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingRecipeSO = null;
                UpdateEventState(true);
                return;
            case InteractType.counterObjectToHandPlate:
                putObjectToPlate(GetKitchenObject(), player.GetKitchenObject() as PlateKitchenObject);
                cookingRecipeSO = null;
                UpdateEventState(true);
                return;
            default:
                break;
        }
    }

    /// <summary>
    /// ����CookingRecipeSO��ǰ�Ƿ�Ϊ�����޸��¼���״̬
    /// </summary>
    /// <param name="isCookingRecipeSONull"></param>
    private void UpdateEventState(bool isCookingRecipeSONull)
    {
        if (!isCookingRecipeSONull)
        {
            OnCookingStateChange?.Invoke(true);
            OnProgressChange?.Invoke(cookingTimer / cookingRecipeSO.cookingTimeMax);
            cookingTimer = 0;
            loopSoundPlayer.PlayLoopSound();
            return;
        }
        else
        {
            OnCookingStateChange?.Invoke(false);
            OnProgressChange?.Invoke(0f);
            loopSoundPlayer.StopLoopSound();
            OnStopWarning?.Invoke();
            return;
        }
    }

    private CookingRecipeSO GetCookingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CookingRecipeSO item in cookingRecipeSOArray)
        {
            if (item.input == inputKitchenObjectSO)
            {
                return item;
            }
        }
        return null;
    }
}
