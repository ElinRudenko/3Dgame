using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour

{
    private Rigidbody rb;

    private static PlayerScript prevInstance = null;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (prevInstance != null)
        {
            //this.rb.velocity = prevInstance.rb.velocity;
            //this.rb.angularVelocity = prevInstance.rb.angularVelocity;
            //GameObject.Destroy(prevInstance.gameObject);

            GameObject.Destroy(this.gameObject);
            //Debug.Log("Destroy");

        }
        else
        {
            
        }
        prevInstance = this;

       
    }

    void Update()
    {
        Vector2 moveValue = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //rb.AddForce(moveValue.x, 0f, moveValue.y);

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        if(camForward == Vector3.zero)
        {
            camForward = Camera.main.transform.up;
        }
        else
        {
            camForward.Normalize();
        }

        Vector3 force = camForward * moveValue.y + camRight * moveValue.x;
        rb.AddForce(force * Time.timeScale);

    }
}
