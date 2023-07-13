using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField] private GameObject deliverySuccess;
    [SerializeField] private GameObject deliveryFail;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += () => StartCoroutine(DeliverySuccess());
        DeliveryManager.Instance.OnDeliveryFail += () => StartCoroutine(DeliveryFail());
    }

    IEnumerator DeliverySuccess()
    {
        deliverySuccess.SetActive(true);
        animator.SetTrigger(AnimatorHash.DeliverFood);
        yield return new WaitForSeconds(1f);
        deliverySuccess.SetActive(false);
    }

    IEnumerator DeliveryFail()
    {
        deliveryFail.SetActive(true);
        animator.SetTrigger(AnimatorHash.DeliverFood);
        yield return new WaitForSeconds(1f);
        deliveryFail.SetActive(false);
    }
}
