using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private int keyNumber = 1;
    [SerializeField] private string description = "відповідні";

    private GameObject content;
    private Image indicatorImage;
    private float timeout = 5.0f;
    private float lefttime;
    private bool isInTime = true;

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

        if (lefttime >= 0)
        {
            indicatorImage.fillAmount = lefttime / timeout;

            indicatorImage.color = new Color(
                Mathf.Clamp01(2.0f * (1.0f - indicatorImage.fillAmount)),
                Mathf.Clamp01(2.0f * indicatorImage.fillAmount),
                0.0f
            );

            lefttime -= Time.deltaTime;

            if (lefttime < 0)
            {
                isInTime = false;
                GameState.isKeyInTime = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player") // ← заменено!
        {
            GameState.bag.Add($"Key{keyNumber}", 1);
            //Debug.Log("Key collected. Sending event...");

            GameEventSystem.EmitEvent(new GameEvent
            {
                type = $"Key{keyNumber} Collected",
                payload = isInTime,
                toast = $"Ключ {keyNumber} найдет. Открывайте {description} двери",
                sound = isInTime ? EffectsSounds.keyCollectedInTime : EffectsSounds.keyCollectedOutOfTime,
            });

            Destroy(transform.root.gameObject);
        }
    }

}
