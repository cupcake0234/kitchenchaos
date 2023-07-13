using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (kitchenObjectParent != null)
        {
            // 处理前一个KitchenObjectParent,让它的kitchenObject为空
            kitchenObjectParent.ClearKitchenObject();
        }
        if (newKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("柜台上已经存在一个物体");
        }
        // 处理新的KitchenObjectParent,设置它的kitchenObject
        newKitchenObjectParent.SetKitchenObject(this);
        // 处理kitchenObject的父物体及位置
        transform.parent = newKitchenObjectParent.GetKitchenObjectSpawnTransform();
        transform.localPosition = Vector3.zero;

        kitchenObjectParent = newKitchenObjectParent;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void DestroySelf()
    {
        kitchenObjectParent?.ClearKitchenObject();
        Destroy(gameObject);
    }
}
