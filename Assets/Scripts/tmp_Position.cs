using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tmp_Position : MonoBehaviour
{
    [SerializeField]
    private Transform tmpGO;
    [SerializeField]
    private Text mouse;
    [SerializeField]
    private Text img;
    [SerializeField]
    private int selecSize;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Screen width " + Screen.width);
        //Debug.Log("Screen height " + Screen.height);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        //img.text = "Img pos: "+tmpGO.position;
        //Debug.Log(tmpGO.position);
        //mouse.text = "Mouse pos: " +Input.mousePosition;
        Color tmpGOColor;
        if(checkCoord(mousePos, tmpGO.position))
             tmpGOColor = Color.green;
        else
            tmpGOColor = Color.red;
        tmpGO.gameObject.GetComponent<Image>().color = tmpGOColor;
    }


    private bool checkCoord(Vector3 coord, Vector3 area)
    {
        bool retValue = false;
        if (coord.x > area.x - selecSize && coord.x < area.x + selecSize)
        {
            if(coord.y > area.y - selecSize && coord.y < area.y + selecSize)
            {
                retValue = true;
            }
        }
        return retValue;
    }

    
}
