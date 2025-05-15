using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates1Script : MonoBehaviour
{
    [SerializeField] private int keyNum = 1;
    [SerializeField] private Vector3 openDirection = Vector3.forward;
    [SerializeField] private float size = 1.38f;

    private float openTime;
    private float openTime1 = 3.0f;
    private float openTime2 = 10.0f;

    private bool isKeyInserted;
    private bool isKeyCollected;
    private int hitCount;

    private AudioSource keyInsertSound;
    private AudioSource gateOpenSound;
    private bool hasPlayedOpenSound = false;

    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;
        GameState.AddListener(OnGameStateChanged);

        // Збираємо всі звуки з дочірніх або цього об’єкта
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

        if (audioSources.Length > 0)
            keyInsertSound = audioSources[0];
        if (audioSources.Length > 1)
            gateOpenSound = audioSources[1];
    }

    void Update()
    {
        if (isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection);

            if (gateOpenSound != null && !hasPlayedOpenSound)
            {
                gateOpenSound.Play();
                hasPlayedOpenSound = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isKeyCollected)
            {
                isKeyInserted = true;
                openTime = GameState.isKeyInTime ? openTime1 : openTime2;

                if (keyInsertSound != null)
                    keyInsertSound.Play();
            }
            else
            {
                hitCount += 1;
                if (hitCount == 1)
                {
                    ToasterScript.Toast($"Для открытия двери: найдите {keyNum} ключ");
                }
                else
                {
                    ToasterScript.Toast($"{hitCount}й не открывается. Иди ищи ключ {keyNum} ключ!");
                }
            }
        }
    }

    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == $"isKey{keyNum}Collected")
        {
            isKeyCollected = true;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
