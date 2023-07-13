using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countDownText;

    private Animator animator;
    private bool isCountingDown;
    private int previousNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        isCountingDown = false;
        countDownText.gameObject.SetActive(false);
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.countDown)
        {
            isCountingDown = true;
            previousNumber = 0;
            countDownText.gameObject.SetActive(true);
        }
        else
        {
            isCountingDown = false;
            countDownText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isCountingDown)
        {
            int currentNumber = Mathf.CeilToInt(GameManager.Instance.GetCountDownTimer());
            countDownText.text = currentNumber.ToString();
            if (currentNumber != previousNumber)
            {
                animator.SetTrigger(AnimatorHash.NumberPopup);
                previousNumber = currentNumber;
                SoundManager.Instance.PlaySound(SoundType.warning, Vector3.zero);
            }
        }
    }
}
