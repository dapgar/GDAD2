using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemInstance : MonoBehaviour
{
    public ItemData itemData;
    public int amount;

    public ItemInstance(ItemData itemData)
    {
        this.itemData = itemData;
    }
}
