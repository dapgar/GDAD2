using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class InventoryItem
{
    public ItemData itemData;
    public int stackSize;
    public int maxSize;

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
        RemoveFromStack();
    }
}
