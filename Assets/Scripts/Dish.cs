using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ContainedKitchenObject
{
    public GameObject gameObject;
    public KitchenObjectSO kitchenObjectSO;
}

public class Dish : MonoBehaviour
{
    public ContainedKitchenObject[] containedKitchenObjectArray;
}
