using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : WorldSpaceUI
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private Image progressBarImage;

    private Animator animator;
    private Color normalColor;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        if (!baseCounter.TryGetComponent<IProgressBar>(out IProgressBar progressBar))
        {
            Debug.LogError("当前柜台没有继承IProgressBar接口");
        }

        progressBar.OnProgressChange += OnProgressChange;
        if (baseCounter is StoveCounter)
        {
            normalColor = progressBarImage.color;
            (baseCounter as StoveCounter).OnBeginWarning += () => animator.SetBool(AnimatorHash.IsFlashing, true);
            (baseCounter as StoveCounter).OnStopWarning += () =>
            {
                animator.SetBool(AnimatorHash.IsFlashing, false);
                progressBarImage.color = normalColor;
            };
        }
        gameObject.SetActive(false);
    }

    private void OnProgressChange(float fillAmount)
    {
        gameObject.SetActive(true);
        progressBarImage.fillAmount = fillAmount;
        if (fillAmount == 0 || fillAmount == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
