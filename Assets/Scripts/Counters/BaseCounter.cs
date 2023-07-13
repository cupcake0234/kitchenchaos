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
    /// ����һ�ֽ�������
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    protected InteractType GetInteractType(Player player)
    {
        // ������ͣ����ڹ�̨��������е���Ʒ�ֱ�����жϣ����ó�һ��ֵ��Ȼ������ֵ��ӣ��ó���Ҫִ�еĽ�������
        // ���ڹ�̨��0����̨û����Ʒ��1����̨��ƷΪ������2����̨��ƷΪ����
        // ������ң�4������û����Ʒ��8��������ƷΪ������16��������ƷΪ����
        // ���տ�����ϵõ�9�����ݣ�4��8��16��5��9��17��6��10��18
        // 4(0+4),18(2+16),9(1+8)ΪdoNothing,��ֱ�ӷ���
        // 8(0+8),16(0+16)ΪputDown,������Ʒ���ڹ�̨��
        // 5(1+4),6(2+4)ΪpickUp��������Ʒ����
        // 10(2+8)ΪhandObjectToCounterPlate,�������е���Ʒ��ӵ���̨�ϵ�������
        // 17(1+16)ΪcounterObjectToHandPlate,������̨����Ʒ��ӵ����е�����
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
