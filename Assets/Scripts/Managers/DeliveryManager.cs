using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    [SerializeField] private DishRecipeSO dishRecipeSO;
    [SerializeField] private DeliveryCounter deliveryCounter;
    [SerializeField] private float timeInterval;
    [SerializeField] private int maxWaitingDishAmount;

    private float timer;
    private int currentWaitingDishAmount;
    private List<DishSO> waitingDishList;
    private int deliveredAmount;
    private bool isGamePlaying;

    public event System.Action<DishSO> OnAddWaitingDish;
    public event System.Action<DishSO> OnRemoveWaitingDish;
    public event System.Action OnDeliverySuccess;
    public event System.Action OnDeliveryFail;

    private void Awake()
    {
        Instance = this;
        waitingDishList = new List<DishSO>();
    }

    private void Start()
    {
        deliveryCounter.OnDeliverDish += OnDeliverDish;
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.gamePlaying)
        {
            isGamePlaying = true;
        }
        else
        {
            isGamePlaying = false;
        }
    }

    private void OnDeliverDish(DishSO deliveredDishSO)
    {
        if (waitingDishList.Remove(deliveredDishSO))
        {
            OnRemoveWaitingDish?.Invoke(deliveredDishSO);
            OnDeliverySuccess?.Invoke();
            SoundManager.Instance.PlaySound(SoundType.deliverySuccess, deliveryCounter.transform.position);
            currentWaitingDishAmount--;
            deliveredAmount++;
        }
        else
        {
            OnDeliveryFail?.Invoke();
            SoundManager.Instance.PlaySound(SoundType.deliveryFail, deliveryCounter.transform.position);
        }
    }

    private void Update()
    {
        if (isGamePlaying && currentWaitingDishAmount < maxWaitingDishAmount)
        {
            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                DishSO dishSO = dishRecipeSO.dishSOList[Random.Range(0, dishRecipeSO.dishSOList.Count)];
                waitingDishList.Add(dishSO);
                OnAddWaitingDish?.Invoke(dishSO);
                timer = 0;
                currentWaitingDishAmount++;
            }
        }
    }

    public int GetDeliveredAmount()
    {
        return deliveredAmount;
    }
}
