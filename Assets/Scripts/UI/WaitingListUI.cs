using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaitingDishStruct
{
    public Transform transform;
    public DishSO dishSO;

    public WaitingDishStruct(Transform transform, DishSO dishSO)
    {
        this.transform = transform;
        this.dishSO = dishSO;
    }
}

public class WaitingListUI : MonoBehaviour
{
    [SerializeField] private Transform dishRecipeTemplate;

    private List<WaitingDishStruct> waitingList;

    private void Awake()
    {
        waitingList = new List<WaitingDishStruct>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnAddWaitingDish += OnAddWaitingDish;
        DeliveryManager.Instance.OnRemoveWaitingDish += OnRemoveWaitingDish;
    }

    private void OnAddWaitingDish(DishSO dishSO)
    {
        Transform dishRecipeTransform = Instantiate(dishRecipeTemplate, dishRecipeTemplate.parent);
        dishRecipeTransform.localPosition = Vector3.zero;
        dishRecipeTransform.gameObject.SetActive(true);        
        waitingList.Add(new WaitingDishStruct(dishRecipeTransform, dishSO));
        dishRecipeTransform.GetComponent<IconTemplate>().SetIconSprite(dishSO);
        dishRecipeTransform.GetComponentInChildren<DishRecipeContainedUI>().AddDishContainedIcon(dishSO);
    }

    private void OnRemoveWaitingDish(DishSO dishSO)
    {
        for (int i = 0; i < waitingList.Count; i++)
        {
            if (waitingList[i].dishSO == dishSO)
            {
                Destroy(waitingList[i].transform.gameObject);
                waitingList.RemoveAt(i);
                return;
            }
        }
    }


}
