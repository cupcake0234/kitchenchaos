using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;
    [SerializeField] private DishRecipeSO dishRecipeSO;

    public event System.Action<KitchenObjectSO> OnAddKitchenObject;

    private List<KitchenObjectSO> containedKitchenObjectList;
    private Transform currentDishTransform;
    private DishSO dishSO;

    private void Awake()
    {
        containedKitchenObjectList = new List<KitchenObjectSO>();
    }

    public bool TryAddKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        if (validKitchenObjectSOList.Contains(kitchenObjectSO) && !containedKitchenObjectList.Contains(kitchenObjectSO))
        {
            containedKitchenObjectList.Add(kitchenObjectSO);
            OnAddKitchenObject?.Invoke(kitchenObjectSO);
            if (!MatchDishRecipe(containedKitchenObjectList, out dishSO))
            {
                Debug.LogError("找不到对应的菜谱");
            }
            if (currentDishTransform != null)
            {
                Destroy(currentDishTransform.gameObject);
            }
            currentDishTransform = Instantiate(dishSO.prefab, transform);
            currentDishTransform.localPosition = Vector3.zero;

            return true;
            //if (dish == null)
            //{
            //    Transform dishTransform = Instantiate(dishSO.prefab, this.transform);
            //    dishTransform.localPosition = Vector3.zero;
            //    dish = dishTransform.GetComponent<Dish>();
            //    foreach (ContainedKitchenObject item in dish.containedKitchenObjectArray)
            //    {
            //        item.gameObject.SetActive(false);
            //    }
            //}
            //if (dish.containedKitchenObjectArray[index].kitchenObjectSO == kitchenObjectSO)
            //{
            //    dish.containedKitchenObjectArray[index].gameObject.SetActive(true);
            //    OnAddKitchenObject?.Invoke(dish.containedKitchenObjectArray[index].kitchenObjectSO);
            //    index++;
            //    return true;
            //}
        }
        return false;
    }

    private bool MatchDishRecipe(List<KitchenObjectSO> plateContaiedList, out DishSO matchedDishSO)
    {
        float maxMatchRate = 0;
        float matchRate = 0;
        int matchCount = 0;
        int index = -1;
        // 遍历所有的菜谱，寻找与当前盘子包含的物品列表重合度最高的菜谱
        for (int i = 0; i < dishRecipeSO.dishSOList.Count; i++)
        {
            DishSO dishSO = dishRecipeSO.dishSOList[i];
            matchCount = 0;
            matchRate = 0;
            for (int j = 0; j < plateContaiedList.Count; j++)
            {
                if (dishSO.containedKitchenObjectList.Contains(plateContaiedList[j]))
                {
                    matchCount++;
                }
            }
            matchRate = (float)matchCount / dishSO.containedKitchenObjectList.Count;
            if (matchRate >= maxMatchRate)
            {
                maxMatchRate = matchRate;
                index = i;
            }
        }
        if (index == -1)
        {
            matchedDishSO = null;
            return false;
        }
        else
        {
            matchedDishSO = dishRecipeSO.dishSOList[index];
            return true;
        }
    }

    public DishSO GetDishSO()
    {
        return dishSO;
    }
}
