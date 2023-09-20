using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsGenerator : MonoBehaviour
{
    [SerializeField]
    private int betweenLenght = 2;
    [SerializeField]
    private int cellsSize = 30;
    [SerializeField]
    private Vector2Int cellsGridSize;
    [SerializeField]
    private GameObject cellObject;
    [SerializeField]
    private Transform parentObject;

    private GameObject[][] cells;
    private bool[][] cellsPlace;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.LogWarning("Start generetion cells");
        Generate();
        //Debug.LogWarning("Complete generetion cells");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate()
    {
        int startPositionX = CalcStartPosition(cellsGridSize.x, betweenLenght, cellsSize), 
            startPositionY = CalcStartPosition(cellsGridSize.x, betweenLenght, cellsSize);
        cells = new GameObject[cellsGridSize.y][];
        cellsPlace = new bool[cells.Length][];
        for(int i = 0; i < cells.Length;i++)
        {
            cells[i] = new GameObject[cellsGridSize.x];
            cellsPlace[i] = new bool[cells[i].Length];
            for(int j = 0; j < cells[i].Length; j++)
            {
                cellsPlace[i][j] = true;
                cells[i][j] = Instantiate(cellObject, parentObject);
                cells[i][j].transform.position = new Vector3(-startPositionX + betweenLenght * j + cellsSize * j, -startPositionY + betweenLenght * i + cellsSize * i,0) + parentObject.position;
                cells[i][j].GetComponent<CellsScript>().SetGridPosition(new Vector2Int(i, j));
            }
        }
    }

    private int CalcStartPosition(int gridSize, int between, int cellsSize)
    {
        int retValue;
        if (gridSize % 2 == 0)
        {
            int n = (gridSize - 2) / 2;
            retValue = cellsSize / 2 + between / 2 + n * cellsSize + n * between;
        }
        else
        {
            int n = (gridSize - 3) / 2;
            retValue = cellsSize + between + n * cellsSize + n * between;
        }

        return retValue;
    }

    public bool SetHoldingItem(GameObject inputItem)
    {
        bool retValue = false;
        Vector2Int itemCoords = GetCurrentCells();
        if (itemCoords.x >= 0 && cellsPlace[itemCoords.x][itemCoords.y])
        {
            Debug.Log("Right coords and free space");
            //cellsPlace[itemCoords.x][itemCoords.y] = false;
            inputItem.transform.position = cells[itemCoords.x][itemCoords.y].transform.position;
            //cells[itemCoords.x][itemCoords.y].GetComponent<CellsScript>().SetHoldingItem(inputItem);
            int sideNumber = cells[itemCoords.x][itemCoords.y].GetComponent<CellsScript>().GetQuadSied();
            //Debug.Log(sideNumber);
            Vector2Int itemHoldCoord = inputItem.GetComponent<tmp_Block>().tmpGetRightCoord(sideNumber);
            //Debug.Log(itemHoldCoord);
            if(CheckZoneCoord(itemCoords, itemHoldCoord, inputItem))
            {
                retValue = true;
            }
            //Hold item in this cells, may be create GameObject matirx?
        }
        return retValue;
    }

    public bool CheckZoneCoord(Vector2Int cellCoord, Vector2Int sizeStartCoord, GameObject inputItem)
    {
        Debug.LogError("START ZONE CHECK ON COORD (x: "+cellCoord.x+" y: "+cellCoord.y+")");
        //Debug.Log(cellCoord - sizeStartCoord);
        bool retValue = true;
        Vector2Int startCoord = cellCoord-sizeStartCoord;
        Vector2Int sizeItemCoord = startCoord + inputItem.GetComponent<tmp_Block>().GetBlockSize();
        Debug.Log("Start coord: " + startCoord);
        if(startCoord.x>=0 && startCoord.y >=0)
        {
            for (int i = startCoord.x; i < sizeItemCoord.x; i++)
            {
                for (int j = startCoord.y; j < sizeItemCoord.y; j++)
                {
                    if (i>=cellsPlace.Length || j >= cellsPlace[i].Length || !cellsPlace[i][j])
                    {
                        Debug.Log("Wrong size");
                        retValue = false;
                        break;
                    }
                }
                if(!retValue)
                { 
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Wrong size");
            retValue = false;
        }
        if(retValue)
        {
            for (int i = startCoord.x; i < sizeItemCoord.x; i++)
            {
                for (int j = startCoord.y; j < sizeItemCoord.y; j++)
                {
                    //inputItem.GetComponent<tmp_Block>().AddCurrentCell(cells[i][j].GetComponent<CellsScript>());
                    cellsPlace[i][j] = false;
                    cells[i][j].GetComponent<CellsScript>().SetHoldingItem(inputItem);
                }
            }
        }
        return retValue;
    }

    public Vector2Int GetCurrentCells()
    {
        Vector2Int retItem = new Vector2Int(-1,-1);
        for (int i = 0; i < cells.Length; i++)
        {
            for (int j = 0; j < cells[i].Length; j++)
            {
                if(cells[i][j].GetComponent<CellsScript>().CheckCoord(Input.mousePosition))
                {
                    retItem = new Vector2Int(i, j);
                    i = cells.Length;
                    //Debug.Log("Find ret Item");
                    break;
                }
            }
        }
        return retItem;
    }

    public void DisableCell(Vector2Int cellCoord)
    {
        //Debug.Log("Disable");
        cellsPlace[cellCoord.x][cellCoord.y] = false;
    }

    public void ClearCellSpace(Vector2Int inputCoords)
    {
        cellsPlace[inputCoords.x][inputCoords.y] = true;
    }

    private void tmp()
    {

    }
    //get number
    //get center
    //check coords if all can store item
}
