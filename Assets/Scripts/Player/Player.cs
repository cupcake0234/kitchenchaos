using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask counterLayer;
    [SerializeField] private Transform holdKitchenObjectPoint;

    // �ƶ�
    private Vector2 inputVector => GameInput.Instance.GetMovementVectorNormalized();
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private float deadZoneMin = 0.15f;
    private float moveDistance => moveSpeed * Time.deltaTime;
    public bool isWalking => inputVector != Vector2.zero;

    // ����
    private float interactDistance = 1.5f;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private bool isGamePlaying;

    public event System.Action<BaseCounter> OnSelectedCounterChanged;

    public Joystick joystick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("����һ�����ʵ��");
        }
        isGamePlaying = false;
    }
    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        GameManager.Instance.OnGameStateChange += OnGameStateChange;

        //
        joystick = FindObjectOfType<Joystick>();
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.gamePlaying)
        {
            isGamePlaying = true;
        }
        else
        {
            isGamePlaying = false;
        }
    }

    private void GameInput_OnInteractAlternateAction()
    {
        if(!isGamePlaying)
        {
            return;
        }
        selectedCounter?.InteractOfButton_F(this);
    }

    private void GameInput_OnInteractAction()
    {
        if (!isGamePlaying)
        {
            return;
        }
        selectedCounter?.InteractOfButton_E(this);
    }
     
    private void Update()
    {
        MoveHandler();
        InteractHandler();
    }

    private void MoveHandler()
    {
        transform.position += moveDistance * GetMoveDirection();
        //transform.forward = Vector3.Slerp(transform.forward, new Vector3(inputVector.x, 0, inputVector.y), rotateSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(joystick.Horizontal, 0, joystick.Vertical), rotateSpeed * Time.deltaTime);
    }
    private void InteractHandler()
    {
        // ���ǰ���Ƿ��пɽ�������
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, interactDistance, counterLayer))
        {
            BaseCounter baseCounter;
            if (hitInfo.collider.TryGetComponent<BaseCounter>(out baseCounter))
            {
                SetSelecteCounter(baseCounter);
            }
            else
            {
                SetSelecteCounter(null);
            }
        }
        else
        {
            SetSelecteCounter(null);
        }
    }

    /// <summary>
    /// ��ȡ��ǰ�����ƶ��ķ���
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMoveDirection()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        //Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance))
        {
            return moveDirection;
        }

        // ����X�᷽��
        if (Mathf.Abs(inputVector.x) > deadZoneMin)
        {
            Vector3 moveDirectionX = new Vector3(inputVector.x, 0, 0);
            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance))
            {
                return moveDirectionX.normalized;
            }
        }

        if (Mathf.Abs(inputVector.y) > deadZoneMin)
        {
            // ����Z�᷽��
            Vector3 moveDirectionZ = new Vector3(0, 0, inputVector.y);
            if (!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance))
            {
                return moveDirectionZ.normalized;
            }
        }

        // ��ǰ���з����޷��ƶ�
        return Vector3.zero;
    }

    private void SetSelecteCounter(BaseCounter baseCounter)
    {
        selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(selectedCounter);
    }

    public Transform GetKitchenObjectSpawnTransform()
    {
        return holdKitchenObjectPoint;
    }

    public void SetKitchenObject(KitchenObject newKitchenObject)
    {
        kitchenObject = newKitchenObject;
        if (kitchenObject != null)
        {
            SoundManager.Instance.PlaySound(SoundType.objectPickUp, transform.position);
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
}
