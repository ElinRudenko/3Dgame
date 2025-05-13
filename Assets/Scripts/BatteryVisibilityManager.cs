using System.Collections.Generic;
using UnityEngine;

public class BatteryVisibilityManager : MonoBehaviour
{
    private List<GameObject> batteryObjects = new List<GameObject>();
    private List<GameObject> batteryMoreAddObjects = new List<GameObject>();

    void Start()
    {
        batteryObjects.AddRange(GameObject.FindGameObjectsWithTag("Battery"));
        batteryMoreAddObjects.AddRange(GameObject.FindGameObjectsWithTag("BatteryMoreAdd"));

        // Скрыть при запуске
        SetBatteriesActive(false);

        // Подпишемся на изменения режима
        GameState.AddListener(OnGameStateChanged);
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isFpv))
        {
            UpdateBatteryVisibility();
        }
    }

    private void UpdateBatteryVisibility()
    {
        bool show = GameState.isFpv;
        Debug.Log("FPV mode: " + show);

        SetBatteriesActive(show);
    }

    private void SetBatteriesActive(bool isActive)
    {
        foreach (var battery in batteryObjects)
            if (battery != null)
                battery.SetActive(isActive);

        foreach (var battery in batteryMoreAddObjects)
            if (battery != null)
                battery.SetActive(isActive);
    }
}
