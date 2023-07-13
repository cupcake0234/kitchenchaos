using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStepSound : MonoBehaviour
{
    [SerializeField] private float stepTimeInterval;

    private float timer;

    private void Start()
    {
        timer = stepTimeInterval;
    }

    private void Update()
    {
        if (Player.Instance.isWalking)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SoundManager.Instance.PlaySound(SoundType.footStep, this.transform.position);
                timer = stepTimeInterval;
            }
        }
    }
}
