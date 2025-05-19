using System;
using System.Collections.Generic;

public class GameState
{

    public static Dictionary<string, int> bag { get; } = new Dictionary<string, int>();

    #region float gatesVolume
    private static float _gatesVolume = 0.07f;
    public static float gatesVolume
    {
        get => _gatesVolume;
        set
        {
            if (_gatesVolume != value)
            {
                _gatesVolume = value;
                Notify(nameof(gatesVolume));
            }
        }
    }
    #endregion


    #region float longEffectsVolume
    private static float _longEffectsVolume = 0.1f;
    public static float longEffectsVolume
    {
        get => _longEffectsVolume;
        set
        {
            if (_longEffectsVolume != value)
            {
                _longEffectsVolume = value;
                Notify(nameof(longEffectsVolume));
            }

        }
    }
    #endregion


    #region float effectsVolume
    private static float _effectsVolume = 0.07f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                Notify(nameof(effectsVolume));
            }

        }
    }
    #endregion


    #region float musicVolume
    private static float _musicVolume = 0.006f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(musicVolume));
            }

        }
    }
    #endregion






    #region bool isKeyInTime
    public static bool isKeyInTime { get; set; }
    #endregion





    #region bool isKeyCollected
    private static bool _isKey1Collected = false;
    public static bool isKey1Collected
    {
        get => _isKey1Collected;
        set
        {
            if (_isKey1Collected != value)
            {
                _isKey1Collected = value;
                Notify(nameof(isKey1Collected));
            }

        }
    }
    #endregion

    #region bool isKe2yCollected
    private static bool _isKey2Collected = false;
    public static bool isKey2Collected
    {
        get => _isKey2Collected;
        set
        {
            if (_isKey2Collected != value)
            {
                _isKey2Collected = value;
                Notify(nameof(isKey2Collected));
            }

        }
    }
    #endregion


    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }

        }
    }
    #endregion

    #region bool isFpv
    private static bool _isFpv = true;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }

        }
    }
    #endregion


    #region Change Notified
    private static List<Action<string>> listeners = new List<Action<string>>();

    public static void AddListener(Action<string> listener)
    {
        //if (listener != null && !listeners.Contains(listener))
        //{
        listeners.Add(listener);
        listener(null);
        //}
    }

    public static void RemoveListener(Action<string> listener)
    {
        if (listener != null && listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public static void Notify(string fieldName)
    {
        foreach (Action<string> listener in listeners)
        {
            listener.Invoke(fieldName);
        }
    }
    #endregion
    

    
}
