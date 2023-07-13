using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterAnimator : MonoBehaviour
{
    [SerializeField] ContainerCounter containerCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnTakeKitchenObject += OnTakeKitchenObject;
    }

    private void OnTakeKitchenObject()
    {
        animator.SetTrigger(AnimatorHash.TakeKitchenObject);
    }
}
