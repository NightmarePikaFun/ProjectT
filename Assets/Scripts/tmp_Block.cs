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

    private GameObject observer;

    //
    private Vector3 startPostition;
    private CellsScript currentCell;
    private bool canMove = false;
    private int[] centerCoordX, centerCoordY;

    private List<CellsScript> usedCells = new List<CellsScript>();
    //TODO rewrite add and clear script to List
    // Start is called before the first frame update
    void Start()
    {
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
        ClearCurrentCell();
        startPostition = transform.position;
        canMove = true;
        observer.GetComponent<Observer>().HoldingItem(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canMove= false;
        observer.GetComponent<Observer>().ClearItem();
    }

    public bool SetCurrentCell(CellsScript inputCells)
    {
        bool retValue = false;
        //Debug.LogWarning("Try set current cell");
        if(currentCell == null)
        {
            //Debug.Log("Set current cells");
            currentCell = inputCells;
            retValue = true;
        }
        return retValue;
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
        //return center coord +1
        //Debug.Log("Calculated slot: x -> " + centerCoordX[selectedCoordX] +" y -> " + centerCoordY[selectedCoordY]);
        //Debug.Log(selectedCoordX + " " + selectedCoordY);
        return new Vector2Int(centerCoordX[selectedCoordX], centerCoordY[selectedCoordY]);
    }

    public Vector2Int GetBlockSize()
    {
        return objectSize;
    }
}
