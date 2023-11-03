using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;
    public int maxSize = 3;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
        stackSize++;
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void UseItem()
    {
        if (stackSize != 0)
        {
            Debug.Log(itemData.itemName + " used.");
            RemoveFromStack();
        }
        else
        {
            Debug.Log("You do not have any " + itemData.itemName + " remaining.");
        }
        
    }
}
