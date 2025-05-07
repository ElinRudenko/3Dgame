using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
   [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    void Start()
    {
         GameState.AddListener(OnGameStateChange);
         RenderSettings.skybox = GameState.isDay ? daySkybox : nightSkybox;
    }



    private void OnGameStateChange(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            RenderSettings.skybox = GameState.isDay ? daySkybox : nightSkybox;
        }
       
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChange);
    }
}
