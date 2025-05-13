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
    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;
        GameState.AddListener(OnGameStateChanged);
        
    }
    void Update()
    {
       if(isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection );

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if(isKeyCollected)
            {
                isKeyInserted = true;
                openTime = GameState.isKeyInTime ? openTime1 : openTime2;
            
            }
            else
            {
                hitCount += 1;
                if(hitCount == 1)
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
        if(fieldName == $"isKey{keyNum}Collected")
        {
            isKeyCollected = true;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
    }
}
