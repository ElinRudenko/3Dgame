using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyCollectedInTime;
    private AudioSource keyCollectedOutOfTime;

    private AudioSource batteryCollectedSound;
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        keyCollectedInTime = audioSources[0];
        batteryCollectedSound = audioSources[1];
        keyCollectedOutOfTime = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
    }

    private void OnGameEvent(GameEvent gameEvent)
    {

        if (gameEvent.sound != null )
        {
            switch(gameEvent.sound)
            {
                case EffectsSounds.batteryCollected: batteryCollectedSound.Play(); break;
                case EffectsSounds.keyCollectedOutOfTime: keyCollectedOutOfTime.Play(); break;
                default: keyCollectedInTime.Play(); break;
            }
        }
    }


    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
