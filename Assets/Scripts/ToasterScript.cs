using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private GameObject content;
    private Text text;
    private CanvasGroup contentGroup;

    private float timeout;
    private float showtime = 3.0f;

    private Queue<ToastMessage> messageQueue = new Queue<ToastMessage>();

    void Start()
    {
        instance = this;

        Transform t = this.transform.Find("Content");
        content = t.gameObject;
        contentGroup = content.GetComponent<CanvasGroup>();

        text = t.Find("Text").GetComponent<Text>();
        content.SetActive(false);
        timeout = 0.0f;
        GameState.AddListener(OnGameStateChanged);
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        if (timeout > 0)
        {
            timeout -= Time.deltaTime;
            contentGroup.alpha = Mathf.Clamp01(timeout * 2.0f);
            if (timeout <= 0)
            {
                content.SetActive(false);
            }
        }
        else if (messageQueue.Count > 0)
        {
            var toast = messageQueue.Dequeue();
            content.SetActive(true);
            text.text = toast.message;
            timeout = toast.time > 0 ? toast.time : showtime;
        }
    }




    private void OnGameStateChanged(string fieldName)
    {
        /*
        if(fieldName == nameof(GameState.isKey1Collected))
        {
            Toast("Ключ 1 найдет. Открывайте зеленые двери");
        }
        */
    }
    private void OnGameEvent(GameEvent gameEvent)
    {
        if (!string.IsNullOrEmpty(gameEvent.toast))
        {
            Toast(gameEvent.toast);
        }
        else if (!string.IsNullOrEmpty(gameEvent.type))
        {
            Toast(gameEvent.type);
        }
    }



    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
        GameEventSystem.Unsubscribe(OnGameEvent);
    }

    public static void Toast(string message, float time = 0.0f)
    {
        if (instance != null)
        {
            instance.messageQueue.Enqueue(
                new ToastMessage { 
                    message = message,
                     time = time > 0.0f ? time : instance.showtime
                });
        }
    }

    private class ToastMessage
    {
        public string message { get; set; }
        public float time { get; set; }
    }
}
