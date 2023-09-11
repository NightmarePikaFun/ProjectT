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
    [SerializeField]
    private List<Color> sideColor;

    private Vector2Int gridPosition;

    private int cellState = 0; // 0 - empty and not mouse over / 1 - empty and mouse over / 2 - not empty

    private GameObject holdingItem;

    private Observer observer;
    private CellsGenerator tmpGenerator;
    // Start is called before the first frame update
    void Start()
    {
        GameObject observerObject = GameObject.FindGameObjectWithTag("Observer");
        observer = observerObject.GetComponent<Observer>();
        tmpGenerator = observerObject.GetComponent<CellsGenerator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        Color tmpGOColor;
        cellState = checkCoord(mousePos, transform.position);
        switch (cellState) 
        {
            case 0:
                {
                    tmpGOColor = baseColor;
                    break;
                }
            case 1:
                {
                    //tmpGOColor = mouseOverColor;
                    tmpGOColor = TMPSetColor(mousePos, transform.position);
                    break;
                }
            case 2:
                {
                    tmpGOColor = holdItemColor;
                    break;
                }
            default:
                {
                    tmpGOColor = Color.black;
                    break;
                }
        
    }
        transform.gameObject.GetComponent<Image>().color = tmpGOColor;
    }

    private Color TMPSetColor(Vector3 coord, Vector3 area)
    {
        Color retColor = Color.cyan;
        int number = FindQuadSide(coord, area);
        if(number>0)
        {
            retColor = sideColor[number - 1];
        }
        return retColor;
    }

    private int checkCoord(Vector3 coord, Vector3 area)
    {
        int retValue = 0;
        if (cellState == 2)
        {
            retValue = cellState;
        }
        else
        {
            if (coord.x > area.x - selecSize && coord.x < area.x + selecSize)
            {
                if (coord.y > area.y - selecSize && coord.y < area.y + selecSize)
                {
                    retValue = 1;
                }
            }
        }
        return retValue;
    }


    public int GetQuadSied()
    {
        return FindQuadSide(Input.mousePosition, transform.position);
    }

    private int FindQuadSide(Vector3 inputPosition, Vector3 currentPosition)
    {
        int retValue = 0;
        Vector3 relativePos = inputPosition - currentPosition;
        if(relativePos.x > 0)
        {
            if(relativePos.y > 0)
            {
                retValue = 2;
            }
            else
            {
                retValue = 3;
            }
        }
        else
        {
            if (relativePos.y > 0)
            {
                retValue = 1;
            }
            else
            {
                retValue = 4;
            }
        }
        return retValue;
    }

    public bool CheckCoord(Vector3 inputPosition)
    {
        //Change this;
        return checkCoord(inputPosition, transform.position)==1;
    }

    public bool SetGridPosition(Vector2Int inputGridPosition)
    {
        bool retValue = false;
        gridPosition = inputGridPosition;
        return retValue;
    }

    #region HoldingItem
    public bool SetHoldingItem(GameObject inputItem)
    {
        bool retValue = false;
        holdingItem = inputItem;
        cellState = 2;
        inputItem.GetComponent<tmp_Block>().AddCurrentCell(this);//SetCurrentCell(this);
        //tmpGenerator.SetHoldingItem(inputItem, gridPosition);
        return retValue;
    }

    public GameObject GetHoldingItem()
    {
        return holdingItem;
    }

    public bool RemoveHoldingItem()
    {
        bool retValue = false;
        if(holdingItem != null)
        {
            cellState = 0;
            tmpGenerator.ClearCellSpace(gridPosition);
            holdingItem = null;
        }
        return retValue;
    }
    #endregion

    //TODO clear item and cells;
}
