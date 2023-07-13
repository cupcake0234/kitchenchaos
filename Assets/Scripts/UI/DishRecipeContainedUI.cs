using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishRecipeContainedUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;

    public void AddDishContainedIcon(DishSO dishSO)
    {
        for (int i = 0; i < dishSO.containedKitchenObjectList.Count; i++)
        {
            Transform iconTemplateTransform = Instantiate(iconTemplate, transform);
            iconTemplateTransform.localPosition = Vector3.zero;
            iconTemplateTransform.gameObject.SetActive(true);
            iconTemplateTransform.GetComponent<IconTemplate>().SetIconSprite(dishSO.containedKitchenObjectList[i]);
        }
    }

}
