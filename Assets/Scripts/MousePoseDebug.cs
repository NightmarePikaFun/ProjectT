using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MousePoseDebug : MonoBehaviour
{
    [SerializeField]
    private Text mouse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mouse.text = "Mouse pos: " + Input.mousePosition;
    }
}
