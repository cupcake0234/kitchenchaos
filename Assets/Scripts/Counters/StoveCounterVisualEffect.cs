using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisualEffect : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Awake()
    {
        stoveCounter.OnCookingStateChange += (bool hasVisualEffect) => gameObject.SetActive(hasVisualEffect);
        gameObject.SetActive(false);
    }
}
