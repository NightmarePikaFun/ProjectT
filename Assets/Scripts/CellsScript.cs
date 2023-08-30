using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellsScript : MonoBehaviour
{
    [SerializeField]
    private int selecSize;
    [SerializeField]
    private Color baseColor;
    [SerializeField]
    private Color mouseOverColor;
    [SerializeField]
    private Color holdItemColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Color tmpGOColor;
        if (checkCoord(mousePos, transform.position))
            tmpGOColor = mouseOverColor;
        else
            tmpGOColor = baseColor;
        transform.gameObject.GetComponent<Image>().color = tmpGOColor;
    }

    private bool checkCoord(Vector3 coord, Vector3 area)
    {
        bool retValue = false;
        if (coord.x > area.x - selecSize && coord.x < area.x + selecSize)
        {
            if (coord.y > area.y - selecSize && coord.y < area.y + selecSize)
            {
                retValue = true;
            }
        }
        return retValue;
    }
}
