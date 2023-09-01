using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class tmp_Block : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Vector2Int objectSize;

    private GameObject observer;

    //
    private CellsScript currentCell;
    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        observer = GameObject.FindGameObjectWithTag("Observer");
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
        Debug.LogWarning("Try set current cell");
        if(currentCell == null)
        {
            Debug.Log("Set current cells");
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
            Debug.Log("Remove current cells");
            currentCell.RemoveHoldingItem();
            currentCell = null;
        }
        return retValue;
    }

}
