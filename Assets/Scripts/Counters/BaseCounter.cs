using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType
{
    doNothing,
    putDown,
    pickUp,
    handObjectToCounterPlate,
    counterObjectToHandPlate
}

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private GameObject selectedVisual;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    public virtual void InteractOfButton_E(Player player)
    {
        Debug.Log(gameObject.name + "BaseCounter Interact");
    }
    public virtual void InteractOfButton_F(Player player)
    {
        Debug.Log("BaseCounter Button F Interact");
    }

    protected virtual void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(BaseCounter baseCounter)
    {
        if (baseCounter == this)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }

    #region IkitchenObjectParent
    public Transform GetKitchenObjectSpawnTransform()
    {
        return counterTopPoint;
    }
    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
        if (kitchenObject != null)
        {
            SoundManager.Instance.PlaySound(SoundType.objectDrop, transform.position);
        }
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    #endregion

    /// <summary>
    /// 返回一种交互类型
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    protected InteractType GetInteractType(Player player)
    {
        // 编码解释：对于柜台和玩家手中的物品分别进行判断，并得出一个值，然后将两个值相加，得出需要执行的交互类型
        // 对于柜台：0：柜台没有物品；1：柜台物品为其他；2：柜台物品为盘子
        // 对于玩家：4：手中没有物品；8：手中物品为其他；16：手中物品为盘子
        // 最终可以组合得到9个数据：4，8，16，5，9，17，6，10，18
        // 4(0+4),18(2+16),9(1+8)为doNothing,即直接返回
        // 8(0+8),16(0+16)为putDown,即将物品放在柜台上
        // 5(1+4),6(2+4)为pickUp，即将物品拿起
        // 10(2+8)为handObjectToCounterPlate,即将手中的物品添加到柜台上的盘子中
        // 17(1+16)为counterObjectToHandPlate,即将柜台的物品添加到手中的盘子
        int counterCode = 0;
        int playerCode = 0;
        if (HasKitchenObject())
        {
            if (GetKitchenObject() is PlateKitchenObject)
            {
                counterCode = 2;
            }
            else
            {
                counterCode = 1;
            }
        }
        else
        {
            counterCode = 0;
        }

        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject() is PlateKitchenObject)
            {
                playerCode = 16;
            }
            else
            {
                playerCode = 8;
            }
        }
        else
        {
            playerCode = 4;
        }

        switch (counterCode + playerCode)
        {
            case 4:
            case 18:
            case 9:
                return InteractType.doNothing;
            case 8:
            case 16:
                return InteractType.putDown;
            case 5:
            case 6:
                return InteractType.pickUp;
            case 10:
                return InteractType.handObjectToCounterPlate;
            case 17:
                return InteractType.counterObjectToHandPlate;
        }
        return InteractType.doNothing;
    }

    protected void putObjectToPlate(KitchenObject kitchenObject, PlateKitchenObject plate)
    {
        if (plate.TryAddKitchenObjectSO(kitchenObject.GetKitchenObjectSO()))
        {
            kitchenObject.DestroySelf();
        }
    }
}
