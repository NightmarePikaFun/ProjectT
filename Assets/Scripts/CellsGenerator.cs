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
}
