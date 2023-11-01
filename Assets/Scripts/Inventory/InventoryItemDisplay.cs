using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQty;
    public Image icon;

    public InventoryItem item;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if (item != null) { Prime (item); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Prime(InventoryItem item)
    {
        this.item = item;
        if(itemName != null)
        {
            itemName.text = item.itemData.itemName;
        }
        if (itemQty != null)
        {
            itemQty.text = item.stackSize.ToString();
        }
        if (icon != null)
        {
            icon.sprite = item.itemData.icon;
        }
    }
}
