using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private GameObject holderItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public bool HoldingItem(GameObject inputItem)
    {
        bool retValue = false;
        if(holderItem == null) 
        {
            holderItem = inputItem;
            retValue = true;
            Debug.Log("-- Item " + holderItem.name + " holding");
        }
        return retValue;
    }

    public bool ClearItem()
    {
        bool retValue = false;
        if (holderItem != null)
        {
            if (gameObject.GetComponent<CellsGenerator>().SetHoldingItem(holderItem))
            {
                retValue = true;
                Debug.Log("-- Item " + holderItem.name + " remove");
                holderItem = null;
            }
        }
        return retValue;
    }

    public GameObject GetHoldingItem()
    {
        return holderItem;
    }
}
