using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DishSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public new string name;
    public List<KitchenObjectSO> containedKitchenObjectList;
}
