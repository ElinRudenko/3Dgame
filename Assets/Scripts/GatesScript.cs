using System.Collections;
using UnityEngine;

public class GatesScript : MonoBehaviour
{
    [SerializeField] private int keyNum = 1;
    [SerializeField] private Vector3 openDirection = Vector3.forward;
    [SerializeField] private float size = 1.38f;

    private float openTime;
    private float openTime1 = 3.0f;
    private float openTime2 = 10.0f;

    private bool isKeyInserted;
    private bool isKeyCollected;
    private bool hasStartedOpening = false;
    private bool openedInTime = true;

    private int hitCount;

    private AudioSource openingSound1;
    private AudioSource openingSound2;
    private int soundCount = 0;

    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;

        AudioSource[] openingSounds = GetComponents<AudioSource>();
        soundCount = openingSounds.Length;

        if (soundCount >= 1) openingSound1 = openingSounds[0];
        if (soundCount >= 2) openingSound2 = openingSounds[1];

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChange);

        ApplyVolumeSettings();
    }

    void Update()
    {
        if (!isKeyInserted || openTime <= 0) return;

        if (transform.localPosition.magnitude < size)
        {
            Vector3 move = (size / openTime) * Time.deltaTime * openDirection;
            transform.Translate(move);

            if (transform.localPosition.magnitude >= size)
            {
                isKeyInserted = false;
                hasStartedOpening = false;

                if (openingSound1 != null && openingSound1.isPlaying)
                    openingSound1.Stop();

                if (openingSound2 != null && openingSound2.isPlaying)
                    openingSound2.Stop();
            }
        }


        if (openingSound1 != null && openingSound1.isPlaying)
        {
            openingSound1.volume = Time.timeScale == 0 ? 0 : GameState.longEffectsVolume;
        }

        if (openingSound2 != null && openingSound2.isPlaying)
        {
            openingSound2.volume = Time.timeScale == 0 ? 0 : GameState.longEffectsVolume;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isKeyCollected)
            {
                if (!hasStartedOpening)
                {
                    isKeyInserted = true;
                    hasStartedOpening = true;

                    openedInTime = GameState.isKeyInTime;
                    openTime = openedInTime ? openTime1 : openTime2;

                    Debug.Log($"openedInTime: {openedInTime}");

                    PlayGateSound(openedInTime);
                }
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

    private void PlayGateSound(bool inTime)
    {
        if (openingSound1 != null && openingSound1.isPlaying) openingSound1.Stop();
        if (openingSound2 != null && openingSound2.isPlaying) openingSound2.Stop();

        if (soundCount == 1)
        {
            openingSound1?.Play();
        }
        else if (soundCount >= 2)
        {
            if (inTime)
            {
                openingSound1?.Play();
            }
            else
            {
                openingSound2?.Play();
            }
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        Debug.Log($"Gate received event: {gameEvent.type}, payload: {gameEvent.payload}");

        if (gameEvent.type == $"Key{keyNum} Collected")
        {
            isKeyCollected = true;
            isKeyInserted = (bool)gameEvent.payload;
        }
    }

    private void OnGameStateChange(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.longEffectsVolume))
        {
            ApplyVolumeSettings();
        }
    }

    private void ApplyVolumeSettings()
    {
        if (openingSound1 != null)
            openingSound1.volume = GameState.longEffectsVolume;

        if (openingSound2 != null)
            openingSound2.volume = GameState.longEffectsVolume;
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChange);
    }
}
