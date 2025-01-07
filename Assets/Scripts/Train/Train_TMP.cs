using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_TMP : MonoBehaviour
{
    [SerializeField]
    private int speed = 10;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //rb.AddForce(new Vector3(-speed, 0, 0));
            GetComponent<WheelCollider>().motorTorque = speed;
            //GetComponent<Rigidbody>().AddTorque((new Vector3(0, 0, speed)));
        }
        if (Input.GetKey(KeyCode.S))
        {
            //rb.AddForce(new Vector3(speed, 0, 0));
            GetComponent<WheelCollider>().motorTorque = -speed;
            //GetComponent<Rigidbody>().AddTorque((new Vector3(0, 0, -speed)));
        }
    }
}
