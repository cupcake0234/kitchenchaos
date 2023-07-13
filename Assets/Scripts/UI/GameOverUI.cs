using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMP_Text deliveredAmoutText;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        container.SetActive(false);
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.gameOver)
        {
            container.SetActive(true);
            deliveredAmoutText.text = DeliveryManager.Instance.GetDeliveredAmount().ToString();
        }
        else
        {
            container.SetActive(false);
        }
    }
}
