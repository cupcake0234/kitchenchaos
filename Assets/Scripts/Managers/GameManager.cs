using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    waitingStart,
    countDown,
    gamePlaying,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event System.Action<GameState> OnGameStateChange;
    public event System.Action OnGamePaused;
    public event System.Action OnGameUnpaused;

    private float countDownTime = 5.0f;
    private float gamePlayTime = 300.0f;

    private float timerCountDown;
    private float timerGamePlay;
    private GameState state;
    private bool isGamePaused;

    private void Awake()
    {
        Instance = this;
        state = GameState.waitingStart;
        OnGameStateChange?.Invoke(state);
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += OnPauseAction;
        GameInput.Instance.OnInteractAction += OnInteractAction;
    }

    private void OnInteractAction()
    {
        if (state == GameState.waitingStart)
        {
            state = GameState.countDown;
            OnGameStateChange?.Invoke(state);
            timerCountDown = countDownTime;
        }
    }

    public void OnPauseAction()
    {
        if (OptionUI.Instance.isShowing)
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
            OptionUI.Instance.Hide();
            return;
        }
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke();
        }
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.waitingStart:
                break;
            case GameState.countDown:
                timerCountDown -= Time.deltaTime;
                if (timerCountDown <= 0)
                {
                    state = GameState.gamePlaying;
                    OnGameStateChange?.Invoke(state);
                    timerGamePlay = gamePlayTime;
                }
                break;
            case GameState.gamePlaying:
                timerGamePlay -= Time.deltaTime;
                if (timerGamePlay <= 0)
                {
                    state = GameState.gameOver;
                    OnGameStateChange?.Invoke(state);
                }
                break;
            case GameState.gameOver:
                break;
            default:
                break;
        }
        Debug.Log(state);
    }

    public float GetCountDownTimer()
    {
        return timerCountDown;
    }

    public float GetFillAmount()
    {
        return timerGamePlay / gamePlayTime;
    }

}
