using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public static bool isPaused = false;

    private GameObject content;
    private CanvasGroup canvasGroup;

    private bool isMuted;
    private Slider effectsSlider;
    private Slider musicSlider;
    private Toggle muteToggle;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        canvasGroup = content.GetComponent<CanvasGroup>();

        effectsSlider = transform.Find("Content/Sounds/EffectsSlider").GetComponent<Slider>();
        musicSlider = transform.Find("Content/Sounds/MusicSlider").GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/ToggleMute").GetComponent<Toggle>();

        effectsSlider.value = GameState.effectsVolume;
        musicSlider.value = GameState.musicVolume;
        isMuted = muteToggle.isOn;
        OnMuteValueChanged(isMuted);

        Hide();
    }

    void Update()
    {
        // Разрешить нажатие Escape даже если игра на паузе
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (content.activeInHierarchy)
                Hide();
            else
                Show();
        }
    }

    private void Show()
    {
        content.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    private void Hide()
    {
        content.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    public void OnEffectsVolumeValueChanged(float volume)
    {
        if (!isMuted)
            GameState.effectsVolume = volume;
    }

    public void OnMusicVolumeValueChanged(float volume)
    {
        if (!isMuted)
            GameState.musicVolume = volume;
    }

    public void OnMuteValueChanged(bool isMute)
    {
        isMuted = isMute;

        if (isMute)
        {
            GameState.effectsVolume = 0.0f;
            GameState.musicVolume = 0.0f;
        }
        else
        {
            GameState.effectsVolume = effectsSlider.value;
            GameState.musicVolume = musicSlider.value;
        }
    }
}
