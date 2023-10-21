using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject
{
    public List<ItemInstance> items = new();
    public int maxItems = 10;

    public bool AddItem(ItemInstance itemToAdd)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemToAdd;
                return true;
            }
        }
        
        if (items.Count < maxItems)
        {
            items.Add(itemToAdd);
            return true;
        }

        Debug.Log("No space in the inventory");
        return false;
    }

  

    public void RemoveItem(ItemInstance itemInstance)
    {
        items.Remove(itemInstance);
    }

}
