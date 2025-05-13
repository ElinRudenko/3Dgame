using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ← добавлено для Image

public class Key1Script : MonoBehaviour
{
    private GameObject content;
    private Image indicatorImage;
    private float timeout = 5.0f;
    private float lefttime;

    void Start()
    {
        content = transform.Find("Component").gameObject;
        indicatorImage = transform.Find("indicator/Canvas/Foreground").GetComponent<Image>();
        indicatorImage.fillAmount = 1.0f;
        lefttime = timeout;
        GameState.isKeyInTime = true;
    }

    void Update()
    {
        content.transform.Rotate(0, Time.deltaTime * 30f, 0);
        indicatorImage.color = new Color(
            Mathf.Clamp01(2.0f * (1.0f - indicatorImage.fillAmount)),
            Mathf.Clamp01(2.0f * indicatorImage.fillAmount),
            0.0f
        );
        if(lefttime >= 0)
        {
            indicatorImage.fillAmount = lefttime / timeout;
            lefttime -= Time.deltaTime;
            if(lefttime < 0)
            {
                GameState.isKeyInTime = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.name == "Player")
        {
            GameState.isKey1Collected = true;
            Destroy(this.gameObject);
        }
    }
}
