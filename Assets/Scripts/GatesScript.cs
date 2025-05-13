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

                   
                    if (soundCount == 1)
                    {
                        if (!openingSound1.isPlaying)
                            openingSound1.Play();
                    }
                    else if (soundCount >= 2)
                    {
                        if (openedInTime)
                        {
                            if (!openingSound1.isPlaying)
                                openingSound1.Play();
                        }
                        else
                        {
                            if (!openingSound2.isPlaying)
                                openingSound2.Play();
                        }
                    }
                   
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

    private void OnGameEvent(GameEvent gameEvent)
    {
        Debug.Log($"Gate received event: {gameEvent.type}, payload: {gameEvent.payload}");

        if (gameEvent.type == $"Key{keyNum} Collected")
        {
            isKeyCollected = true;
            isKeyInserted = (bool)gameEvent.payload;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
