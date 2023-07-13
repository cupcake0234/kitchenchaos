using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconTemplate : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetIconSprite(KitchenObjectSO kitchenObjectSO)
    {
        icon.sprite = kitchenObjectSO.sprite;
    }

    public void SetIconSprite(DishSO dishSO)
    {
        icon.sprite = dishSO.sprite;
    }

    public void DestroyIcon(DishSO dishSO)
    {

    }
}
