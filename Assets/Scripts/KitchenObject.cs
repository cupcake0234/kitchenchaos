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
            // ����ǰһ��KitchenObjectParent,������kitchenObjectΪ��
            kitchenObjectParent.ClearKitchenObject();
        }
        if (newKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("��̨���Ѿ�����һ������");
        }
        // �����µ�KitchenObjectParent,��������kitchenObject
        newKitchenObjectParent.SetKitchenObject(this);
        // ����kitchenObject�ĸ����弰λ��
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
