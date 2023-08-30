using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train_TMP : MonoBehaviour
{
    [SerializeField]
    private int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(speed,0,0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-speed, 0, 0));
        }
    }
}
