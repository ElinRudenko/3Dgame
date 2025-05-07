using System;
using System.Collections.Generic;

public class GameState
{
    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay { 
        get => _isDay;
        set
        {
            if(_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }

        }
    }
    #endregion

    #region bool isFpv
    private static bool _isFpv = true;
    public static bool isFpv { 
        get => _isFpv;
        set
        {
            if(_isFpv != value)
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
        if (listener != null && !listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
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
