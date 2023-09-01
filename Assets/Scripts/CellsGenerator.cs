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
        Debug.LogWarning("Start generetion cells");
        Generate();
        Debug.LogWarning("Complete generetion cells");
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
        bool retValye = false;
        Vector2Int itemCoords = GetCurrentCells();
        if (itemCoords.x >= 0 && cellsPlace[itemCoords.x][itemCoords.y])
        {
            Debug.Log("Right coords and free space");
            cellsPlace[itemCoords.x][itemCoords.y] = false;
            inputItem.transform.position = cells[itemCoords.x][itemCoords.y].transform.position;
            cells[itemCoords.x][itemCoords.y].GetComponent<CellsScript>().SetHoldingItem(inputItem);
            //Hold item in this cells, may be create GameObject matirx?
        }
        return retValye;
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
                    Debug.Log("Find ret Item");
                    break;
                }
            }
        }
        return retItem;
    }

    public void ClearCellSpace(Vector2Int inputCoords)
    {
        cellsPlace[inputCoords.x][inputCoords.y] = true;
    }
}
