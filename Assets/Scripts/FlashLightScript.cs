using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    public static float charge;
    public float chargeLifeTime = 60.0f;

    [Header("Настройки углов луча")]
    public float[] anglePresets = new float[] { 25f, 35f, 50f, 65f, 80f };
    private int currentAngleIndex = 2; // стартовое значение = 50
    private float targetAngle;

    public float angleLerpSpeed = 5.0f;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player == null)
        {
           // Debug.Log("FlashLightScript: no Player");
        }

        _light = GetComponent<Light>();
        if (_light == null)
        {
            //Debug.LogError("FlashLightScript: No Light component found on this GameObject");
        }

        charge = 1.0f;
        targetAngle = anglePresets[currentAngleIndex];

        if (_light != null)
        {
            _light.spotAngle = targetAngle;
        }
    }

    void Update()
    {
        if (player == null || _light == null) return;

        if (GameState.isFpv && !GameState.isDay)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;

            // Управление предустановками угла клавишами '+' и '-'
            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Plus)) // '+'
            {
                if (currentAngleIndex < anglePresets.Length - 1)
                {
                    currentAngleIndex++;
                    targetAngle = anglePresets[currentAngleIndex];
                }
            }
            else if (Input.GetKeyDown(KeyCode.Minus)) // '-'
            {
                if (currentAngleIndex > 0)
                {
                    currentAngleIndex--;
                    targetAngle = anglePresets[currentAngleIndex];
                }
            }

            // Плавное приближение к нужному углу
            _light.spotAngle = Mathf.Lerp(_light.spotAngle, targetAngle, Time.deltaTime * angleLerpSpeed);

            // Разрядка
            charge = charge < 0 ? 0 : charge - Time.deltaTime / chargeLifeTime;
            
            charge = Mathf.Clamp01(charge);
            _light.intensity = charge;
            


            
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.name);

        if (other.gameObject.CompareTag("Battery"))
        {
            charge += 1.0f;
            Destroy(other.gameObject);
            //Debug.Log("Battery collected: " + charge);
            ToasterScript.Toast($"You knew the battery. Likhtarik charge increased to {charge:F1}", 5.0f);
        }
    }


}
