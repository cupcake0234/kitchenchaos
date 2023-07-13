using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private Transform platePrafab;
    [SerializeField] private float spawnTimeInterval;
    [SerializeField] private int maxPlateAmount;

    private float spawnPlateTimer;
    private int currentPlateAmount;
    private Transform[] plateTransforms;
    private float plateInterval = 0.1f;
    private bool isGamePlaying;

    private void Awake()
    {
        plateTransforms = new Transform[maxPlateAmount];
    }

    protected override void Start()
    {
        base.Start();
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

    private void Update()
    {
        if (isGamePlaying && currentPlateAmount < maxPlateAmount)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer >= spawnTimeInterval)
            {
                // 只生成生成盘子的模型，在玩家交互时再生成盘子
                plateTransforms[currentPlateAmount] = Instantiate(platePrafab, GetKitchenObjectSpawnTransform());
                plateTransforms[currentPlateAmount].localPosition = plateInterval * currentPlateAmount * Vector3.up;
                spawnPlateTimer = 0f;
                currentPlateAmount++;
            }
        }
    }

    public override void InteractOfButton_E(Player player)
    {
        if (!player.HasKitchenObject() && currentPlateAmount >=1)
        {
            // 玩家手中无物品时，玩家拿起一个盘子
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            Destroy(plateTransforms[currentPlateAmount - 1].gameObject);
            currentPlateAmount--;
        }
    }
}
