using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ItemDetailDisplay : MonoBehaviour
{

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public Image icon;

    public void UpdateInfo(InventoryItem item)
    {
        itemName.text = item.itemData.itemName;
        itemDescription.text = item.itemData.description;
        icon.sprite = item.itemData.icon;
    }
    
}
