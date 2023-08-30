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
        canMove = true;
        observer.GetComponent<Observer>().HoldingItem(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canMove= false;
        observer.GetComponent<Observer>().ClearItem();
    }

}
