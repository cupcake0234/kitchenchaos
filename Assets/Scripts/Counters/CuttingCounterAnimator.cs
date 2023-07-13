using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterAnimator : MonoBehaviour
{
    [SerializeField] CuttingCounter cuttingCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.Oncut += Oncut;
    }

    private void Oncut()
    {
        animator.SetTrigger(AnimatorHash.Cut);
    }
}
