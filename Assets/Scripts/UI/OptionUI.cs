using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance;
    [SerializeField] private GameObject container;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Button closeButton;

    [Header("ÐÞ¸Ä¼üÎ»°´Å¥")]
    [SerializeField] private Button[] buttonArray;

    public event System.Action<float> OnMusicVolumeChange;
    public event System.Action<float> OnSoundVolumeChange;
    [HideInInspector] public bool isShowing;

    private void Awake()
    {
        Instance = this;

        musicSlider.onValueChanged.AddListener((float volume) => 
        {
            OnMusicVolumeChange?.Invoke(volume);
            PlayerPrefs.SetFloat(SavedDataKey.MUSIC_VOLUME, volume);
            PlayerPrefs.Save();
        });
        soundSlider.onValueChanged.AddListener((float volume) =>
            {
                OnSoundVolumeChange?.Invoke(volume);
                PlayerPrefs.SetFloat(SavedDataKey.SOUND_VOLUME, volume);
                PlayerPrefs.Save();
            }
        );
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            GamePauseUI.Instance.Show();
        });

        UpdateRebindButtonText();
        RebindButtonInit();

        musicSlider.value = PlayerPrefs.GetFloat(SavedDataKey.MUSIC_VOLUME, 0.3f);
        soundSlider.value = PlayerPrefs.GetFloat(SavedDataKey.SOUND_VOLUME, 1.0f);
    }

    private void Start()
    {
        Hide();
        OnMusicVolumeChange?.Invoke(musicSlider.value);
        OnSoundVolumeChange?.Invoke(soundSlider.value);
    }

    public void Hide()
    {
        container.SetActive(false);
        isShowing = false;
    }

    public void Show()
    {
        container.SetActive(true);
        buttonArray[0].Select();
        isShowing = true;
    }

    private void UpdateRebindButtonText()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].GetComponentInChildren<TMP_Text>().text = GameInput.Instance.GetBindingKeyText(i+Binding.Interact);
        }
    }

    private void RebindButtonInit()
    {
        for (int i = 0; i < buttonArray.Length; i++)
        {
            Button button = buttonArray[i];
            Binding binding = i + Binding.Interact;
            button.onClick.AddListener(() =>
            {
                button.GetComponentInChildren<TMP_Text>().text = "";
                GameInput.Instance.RebindingKey(binding, UpdateRebindButtonText);
            });
        }
    }
}
