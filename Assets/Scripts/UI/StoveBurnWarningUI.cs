using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private Image warningImage;
    [SerializeField] private StoveCounter stoveCounter;

    private bool isWarning;
    private float timer;

    private void Awake()
    {
        warningImage.gameObject.SetActive(false);
        stoveCounter.OnBeginWarning += () => isWarning = true;
        stoveCounter.OnStopWarning += () =>
        {
            isWarning = false;
            warningImage.gameObject.SetActive(false);
        };
    }

    private void Update()
    {
        if (isWarning)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                warningImage.gameObject.SetActive(!warningImage.IsActive());
                SoundManager.Instance.PlaySound(SoundType.warning, Vector3.zero);
                timer = 0;
            }
        }
    }
}
