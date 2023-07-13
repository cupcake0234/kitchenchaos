using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoriaUI : MonoBehaviour
{
    public static TutoriaUI Instance;

    [Header("¼üÅÌ¼üÎ»")]
    [SerializeField] private TMP_Text[] keyboardTextArray;
    [Header("ÊÖ±ú¼üÎ»")]
    [SerializeField] private TMP_Text[] gamePadTextArray;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        GetControlKeyText();
    }

    private void OnGameStateChange(GameState gameState)
    {
        if (gameState == GameState.waitingStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void GetControlKeyText()
    {
        for (int i = 0; i < keyboardTextArray.Length; i++)
        {
            keyboardTextArray[i].text = GameInput.Instance.GetBindingKeyText(i+Binding.MoveUp);
        }
        for (int i = 0; i < gamePadTextArray.Length; i++)
        {
            gamePadTextArray[i].text = GameInput.Instance.GetBindingKeyText(i+Binding.GamePad_Interact);
        }
    }
}
