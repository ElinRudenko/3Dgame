using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource music;
    void Start()
    {   
        music = GetComponent<AudioSource>();
        music.volume = GameState.musicVolume; 
        GameState.AddListener(OnGameStateChange);
    }
    private void OnGameStateChange(string fieldName)
    {
        if (fieldName == nameof(GameState.musicVolume))
        {
           music.volume = GameState.musicVolume;
        }
    }

    

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChange);
    }
}
