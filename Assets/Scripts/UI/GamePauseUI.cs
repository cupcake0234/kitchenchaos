using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    public static GamePauseUI Instance;
    [SerializeField] private GameObject container;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionButton;

    public bool isShowing;

    private void Awake()
    {
        Instance = this;
        container.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += OnGamePaused;
        GameManager.Instance.OnGameUnpaused += OnGameUnpaused;
        mainMenuButton.onClick.AddListener(() => Loader.LoadScene(Scene.MainMenuScene));
        resumeButton.onClick.AddListener(() => GameManager.Instance.OnPauseAction());
        optionButton.onClick.AddListener(() => 
        {
            Hide();
            OptionUI.Instance.Show();
        });
    }

    private void OnGamePaused()
    {
        Show();
    }
    private void OnGameUnpaused()
    {
        Hide();
    }

    public void Hide()
    {
        container.SetActive(false);
        isShowing = false;
    }
    public void Show()
    {
        container.SetActive(true);
        resumeButton.Select();
        isShowing = true;
    }
}

