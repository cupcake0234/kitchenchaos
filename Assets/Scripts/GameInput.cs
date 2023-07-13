using UnityEngine;
using UnityEngine.InputSystem;

public enum Binding
{   
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight, 
    Interact, 
    InteractAlternate,
    Pause,
    GamePad_Interact,
    GamePad_InteractAlternate,
    GamePad_Pause,
    GamePad_Move,
}

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    public event System.Action OnInteractAction;
    public event System.Action OnInteractAlternateAction;
    public event System.Action OnPauseAction;

    private PlayerInputActions playerInputActions;
    private static string PLAYER_PREFS_BINDINGS;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InterractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InterractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Disable();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke();
    }
    private void InterractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke();
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    public string GetBindingKeyText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamePad_Move:
                return playerInputActions.Player.Move.bindings[10].ToDisplayString();
            case Binding.GamePad_Interact:
                return playerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindingKey(Binding binding, System.Action UpdateVisual)
    {
        InputAction inputAction;
        int bindingIndex;
        playerInputActions.Player.Disable();

        switch (binding)
        {
            default:
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                UpdateVisual?.Invoke();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}
