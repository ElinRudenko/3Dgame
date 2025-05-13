using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gates2Script : MonoBehaviour
{
    private Vector3 openDirection = Vector3.forward;
    private float size = 1.38f;
    private float openTime;
    private float openTime1 = 3.0f;
    private float openTime2 = 10.0f;
    private bool isKeyInserted;
    private int hitCount;
    void Start()
    {
        isKeyInserted = false;
        hitCount = 0;
        
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
            if(GameState.isKey2Collected)
            {
                isKeyInserted = true;
                openTime = GameState.isKeyInTime ? openTime1 : openTime2;
            
            }
            else
            {
                hitCount += 1;
                if(hitCount == 1)
                {
                    ToasterScript.Toast("Для открытия двери: найдите зеленый ключ");
                }
                else
                {
                    ToasterScript.Toast("${hitCount}й не открывается. Иди ищи ключ!");
                }

            }
        }
        
       
    }
}
