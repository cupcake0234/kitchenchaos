using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectSpawnTransform();
    public void SetKitchenObject(KitchenObject newKitchenObject);
    public void ClearKitchenObject();
    public KitchenObject GetKitchenObject();
    public bool HasKitchenObject();
}
