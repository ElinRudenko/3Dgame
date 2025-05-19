using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static bool isPaused;

    [Header("Default Settings")]
    [SerializeField] private float defaultMusicVolume = 0.2f;
    [SerializeField] private float defaultEffectsVolume = 0.09f;
    [SerializeField] private float defaultLongEffectsVolume = 0.1f;
    [SerializeField] private bool defaultIsMuted = false;

    [SerializeField] private Image[] bagImage;

    private GameObject content;
    private CanvasGroup canvasGroup;
    private Slider effectsSlider;
    private Slider musicSlider;
    private Slider longEffectsSlider;
    private Toggle muteToggle;

    private bool isMuted;
    private float startTimeScale;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        canvasGroup = content.GetComponent<CanvasGroup>();

        effectsSlider = transform.Find("Content/Sounds/EffectsSlider").GetComponent<Slider>();
        musicSlider = transform.Find("Content/Sounds/MusicSlider").GetComponent<Slider>();
        longEffectsSlider = transform.Find("Content/Sounds/GatesSlider").GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/ToggleMute").GetComponent<Toggle>();

        LoadSettings();
        ApplySettingsToUI();
        ApplyMuteState(isMuted);

        startTimeScale = Time.timeScale;
        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (content.activeSelf) Hide(); else Show();
    }

    private void Show()
    {
        content.SetActive(true);
        isPaused = true;
        SetCanvasGroup(true);
        startTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        for (int i = 0; i < bagImage.Length; i++)
        {
            bagImage[i].enabled = GameState.bag.ContainsKey($"Key{i + 1}");
        }
    }

    private void Hide()
    {
        content.SetActive(false);
        isPaused = false;
        SetCanvasGroup(false);
        Time.timeScale = startTimeScale;
    }

    private void SetCanvasGroup(bool state)
    {
        if (canvasGroup == null) return;
        canvasGroup.blocksRaycasts = state;
        canvasGroup.interactable = state;
    }

    private void LoadSettings()
    {
        isMuted = PlayerPrefs.GetInt("isMuted", defaultIsMuted ? 1 : 0) == 1;
        GameState.effectsVolume = PlayerPrefs.GetFloat("effectsVolume", defaultEffectsVolume);
        GameState.musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
        GameState.longEffectsVolume = PlayerPrefs.GetFloat("longEffectsVolume", defaultLongEffectsVolume);
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.SetFloat("effectsVolume", GameState.effectsVolume);
        PlayerPrefs.SetFloat("musicVolume", GameState.musicVolume);
        PlayerPrefs.SetFloat("longEffectsVolume", GameState.longEffectsVolume);
        PlayerPrefs.Save();
    }

    private void ApplySettingsToUI()
    {
        effectsSlider.value = GameState.effectsVolume;
        musicSlider.value = GameState.musicVolume;
        longEffectsSlider.value = GameState.longEffectsVolume;
        muteToggle.isOn = isMuted;
    }

    private void ApplyMuteState(bool mute)
    {
        isMuted = mute;

        if (isMuted)
        {
            GameState.effectsVolume = 0f;
            GameState.musicVolume = 0f;
            GameState.longEffectsVolume = 0f;
        }
        else
        {
            GameState.effectsVolume = effectsSlider.value;
            GameState.musicVolume = musicSlider.value;
            GameState.longEffectsVolume = longEffectsSlider.value;
        }

        SaveSettings();
    }

    public void OnEffectsVolumeValueChanged(float volume)
    {
        if (!isMuted)
            GameState.effectsVolume = volume;
        SaveSettings();
    }

    public void OnLongEffectsVolumeValueChanged(float volume)
    {
        if (!isMuted)
            GameState.longEffectsVolume = volume;
        SaveSettings();
    }


    public void OnMusicVolumeValueChanged(float volume)
    {
        if (!isMuted)
            GameState.musicVolume = volume;
        SaveSettings();
    }

    public void OnMuteValueChanged(bool mute)
    {
        ApplyMuteState(mute);
    }

    public void OnDefaultClic()
    {
        isMuted = defaultIsMuted;
        GameState.effectsVolume = defaultEffectsVolume;
        GameState.musicVolume = defaultMusicVolume;
        GameState.longEffectsVolume = defaultLongEffectsVolume;

        ApplySettingsToUI();
        ApplyMuteState(isMuted);
    }

    public void OnContinueClic()
    {
        Hide();
    }

    public void OnExitClic()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
