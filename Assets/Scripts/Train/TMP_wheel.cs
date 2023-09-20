using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TMP_wheel : MonoBehaviour
{
    [SerializeField]
    private int targetSpeed = 0;
    [SerializeField]
    private int currentForce = 0;
    [SerializeField]
    private Text speedText;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HingeJoint[] wheel = GetComponents<HingeJoint>();
        for(int i = 0; i < wheel.Length; i++)
        {
            JointMotor motor = wheel[i].motor;
            if (wheel[i].connectedBody.gameObject.name[0]=='M')
            {
                motor.targetVelocity = targetSpeed*10;
                motor.force = currentForce;
            }
            else
            {
                motor.targetVelocity = 0f;
                motor.force = 0f;
            }
            wheel[i].motor = motor;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(new Vector3(0, 0, -1000));
        }
        if(speedText != null)
            speedText.text = "Speed: " + (rb.velocity.z*-1);

    }
}
