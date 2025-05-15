using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform cameraAnchor;

    private float angleY;
    private float angleX;
    private float angleY0;
    private float angleX0;
    [SerializeField] private float sensitivityY = 10.0f;
    [SerializeField] private float sensitivityX = 5.0f;

    private float minAngleY = 10f; // вниз до погляду "під ноги" 
    private float maxAngleY = 90f;  // вгору — до межі видимості 
    private float minAngleYFpv = -10f; 
    private float maxAngleYFpv = 45f; 

    private Vector3 offset;

    private float minoffset = 1.5f;
    private float maxnoffset = 12f;

    //public static bool isFpv;

    public static bool isFixed;
    public static Transform fixedCameraPosition = null;

    // Стан керування
    private bool isControlling = false;

    // Структура для елементів сцени з заданим масштабом та матеріалом
    [System.Serializable]
    public class SceneElement
    {
        public Transform objTransform;
        public Vector3 desiredScale;
        public Material overrideMaterial;
    }

    [SerializeField] private SceneElement[] sceneObjects;

    void Start()
    {
        offset = this.transform.position - cameraAnchor.position;
        angleY = angleY0 = this.transform.eulerAngles.y;
        angleX = angleX0 = this.transform.eulerAngles.x;
        GameState.isFpv = offset.magnitude < minoffset;

        foreach (SceneElement element in sceneObjects)
        {
            if (element.objTransform != null)
            {
                element.objTransform.localScale = element.desiredScale;

                if (element.overrideMaterial != null)
                {
                    Renderer renderer = element.objTransform.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = element.overrideMaterial;
                    }
                }
            }
        }

        EnableCursor(true);
    }

    void Update()
    {

        if (MenuScript.isPaused) return;

        if (isFixed)
        {
            this.transform.position = fixedCameraPosition.position;
            this.transform.rotation = fixedCameraPosition.rotation;
        }
        else
        {
            Vector2 zoom = Input.mouseScrollDelta;
            if (zoom.y > 0 && !GameState.isFpv)
            {
                offset *= 0.9f;
                if (offset.magnitude < minoffset)
                {
                    offset *= 0.01f;
                    GameState.isFpv = true;
                }
            }
            else if (zoom.y < 0)
            {
                if (GameState.isFpv)
                {
                    offset *= minoffset / offset.magnitude;
                    GameState.isFpv = false;
                }

                if (offset.magnitude < maxnoffset)
                {
                    offset *= 1.1f;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EnableCursor(true);
                isControlling = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                EnableCursor(false);
                isControlling = true;
            }

            if (isControlling)
            {
                Vector2 lookValue = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

                angleY += lookValue.x * sensitivityY;
                angleX += lookValue.y * sensitivityX;

                // Обмеження вертикального кута камери згідно режиму
                if (GameState.isFpv)
                {
                    angleX = Mathf.Clamp(angleX, minAngleYFpv, maxAngleYFpv);
                }
                else
                {
                    angleX = Mathf.Clamp(angleX, minAngleY, maxAngleY);
                }

                this.transform.eulerAngles = new Vector3(angleX, angleY, 0f);
                this.transform.position = cameraAnchor.position +
                                          Quaternion.Euler(angleX - angleX0, angleY - angleY0, 0f) * offset;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }

    private void EnableCursor(bool enable)
    {
        Cursor.visible = enable;
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
