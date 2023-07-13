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
                // 销毁原有的物体，生成输出,并更新cookingRecepeSO
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
                // 柜台上无物品 且 玩家手中有物品时 且 玩家手中物品能够烹饪，将物品放在柜台上
                if (cookingRecipeSO != null)
                {
                    UpdateEventState(false);
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
                return;
            case InteractType.pickUp:
                // 柜台上有物品且玩家手中无物品时，玩家拿起物品，
                // 且将cookingRecipeSO置空，不然下次放入时会直接从Uncooked变为Burned
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
    /// 根据CookingRecipeSO当前是否为空来修改事件的状态
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
