using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class tmp_Block : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Vector2Int objectSize;
    [SerializeField]
    private int tmpNumber;
    [SerializeField]
    private int weight;

    private GameObject observer;

    //

    private List<CellsScript> oldCells = new List<CellsScript>();//May be only one Cells

    private Vector3 startPostition;
    private CellsScript currentCell;
    private bool canMove = false;
    private int[] centerCoordX, centerCoordY;

    private List<CellsScript> usedCells = new List<CellsScript>();
    //TODO rewrite add and clear script to List
    // Start is called before the first frame update
    void Start()
    {
        startPostition = transform.position;
        observer = GameObject.FindGameObjectWithTag("Observer");
        centerCoordX = tmpFindCenter(objectSize.x);
        centerCoordY = tmpFindCenter(objectSize.y);
        //Debug.LogWarning("Block center coords size x: "+centerCoordX.Length+"  y: "+ centerCoordY.Length);
        for(int i = 0; i < centerCoordX.Length; i++)
        {
            //Debug.LogWarning("x[" + i+"] " + (centerCoordX[i]));
        }
        for (int i = 0; i < centerCoordY.Length; i++)
        {
            //Debug.LogWarning("y[" + i + "] " + (centerCoordY[i]));
        }
        tmpGetRightCoord(tmpNumber);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canMove)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RemoveCurrentCells();
        //ClearCurrentCell();
        startPostition = transform.position;
        canMove = true;
        observer.GetComponent<Observer>().HoldingItem(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canMove= false;
        if (!observer.GetComponent<Observer>().ClearItem())
        {
            if (oldCells.Count > 0)
                ReturnToOldPosition();//Need current Point
            transform.position = startPostition;
        }
        else
        {
            RememberActiveCells();
        }
    }

    public bool SetCurrentCell(CellsScript inputCell)
    {
        bool retValue = false;
        //Debug.LogWarning("Try set current cell");
        if(currentCell == null)
        {
            //Debug.Log("Set current cells");
            currentCell = inputCell;
            retValue = true;
        }
        return retValue;
    }

    public void AddCurrentCell(CellsScript inputCell)
    {
        usedCells.Add(inputCell);
    }

    public void RemoveCurrentCells()
    {
        foreach(var cell in usedCells)
        {
            cell.RemoveHoldingItem();
        }
        usedCells.Clear();
    }

    public bool ClearCurrentCell()
    {
        bool retValue = false;
        if(currentCell != null)
        {
            //Debug.Log("Remove current cells");
            currentCell.RemoveHoldingItem();
            currentCell = null;
        }
        return retValue;
    }


    private int[] tmpFindCenter(int size)
    {
        int center;
        if(size%2==0)
        {
            center = 2;
        }
        else
        {
            center = 1;
        }
        //OnOutput
        int[] coords = new int[center];
        if(center == 1)
        {
            coords[0] = size / 2;
        }
        else
        {
            coords[0] = size / 2 - 1;
            coords[1] = size / 2;
        }
        return coords;
    }

    public Vector2Int tmpGetRightCoord(int number)
    {
        int selectedCoordX, selectedCoordY;
        if (centerCoordX.Length == 1)
        {
            selectedCoordX = 0;
        }
        else
        {
            if (number < 3)//if(number == 1 || number == 4)
            {
                selectedCoordX = 0;
                Debug.LogWarning("ZERO");
            }
            else
            {
                selectedCoordX = 1;
            }
        }

        if (centerCoordY.Length == 1)
        {
            selectedCoordY = 0;
        }
        else
        {
            if(number == 1 || number == 4)//if (number > 2)
            {
                selectedCoordY = 1;
            }
            else
            {
                selectedCoordY = 0;
            }
        }
        return new Vector2Int(centerCoordX[selectedCoordX], centerCoordY[selectedCoordY]);
    }

    public void RememberActiveCells()
    {
        oldCells.Clear();
        for(int i = 0; i < usedCells.Count;i++)
        {
            oldCells.Add(usedCells[i]);
        }
    }

    public bool ReturnToOldPosition()
    {
        bool retValue = false;
        for(int i = 0; i < oldCells.Count;i++)
        {
            oldCells[i].SetHoldingItem(this.gameObject);
        }
        return retValue;
    }

    public Vector2Int GetBlockSize()
    {
        return objectSize;
    }

    //TODO correct position with size and and and other!
    //if lenghtX = 1 not correct X || if lenghtY = 1 not corretctY
    //if lenghtX%2 = 1 correct X || if lenghtY = 1  corretctY
    // if lenghtX%2 = 0 not correct X || if lenghtY == 0 not correctY
}
