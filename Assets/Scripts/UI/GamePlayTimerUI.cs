using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayTimerUI : MonoBehaviour
{
    [SerializeField] private Image image;

    private bool isGamePlaying;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        isGamePlaying = false;
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

    private void Update()
    {
        if (isGamePlaying)
        {
            image.fillAmount = GameManager.Instance.GetFillAmount();
        }
    }
}
