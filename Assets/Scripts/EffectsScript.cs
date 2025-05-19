using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private static EffectsScript instance;

    private AudioSource keyCollectedInTime;
    private AudioSource keyCollectedOutOfTime;
    private AudioSource batteryCollectedSound;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }
    
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        keyCollectedInTime = audioSources[0];
        batteryCollectedSound = audioSources[1];
        keyCollectedOutOfTime = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChange);
        OnGameStateChange(nameof(GameState.effectsVolume));
    }

    private void OnGameEvent(GameEvent gameEvent)
    {

        if (gameEvent.sound != null)
        {
            switch (gameEvent.sound)
            {
                case EffectsSounds.batteryCollected: batteryCollectedSound.Play(); break;
                case EffectsSounds.keyCollectedOutOfTime: keyCollectedOutOfTime.Play(); break;
                default: keyCollectedInTime.Play(); break;
            }
        }
    }

    private void OnGameStateChange(string fieldName)
    {
        //Debug.Log(fieldName + " " + GameState.effectsVolume);
        if (fieldName == null || fieldName == nameof(GameState.effectsVolume))
        {
           
            keyCollectedInTime.volume = 
            batteryCollectedSound.volume = 
            keyCollectedOutOfTime.volume = GameState.effectsVolume;
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChange);
    }
}
