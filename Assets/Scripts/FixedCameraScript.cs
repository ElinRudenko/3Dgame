using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraScript : MonoBehaviour
{
    void Start()
    {
        CameraScript.fixedCameraPosition = transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            CameraScript.isFixed = !CameraScript.isFixed;
        }
    }
}
