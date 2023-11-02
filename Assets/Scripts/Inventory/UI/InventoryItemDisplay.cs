using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQty;
    public Image icon;

    public InventoryItem item;

    public GameObject inventoryDisplay;
    public GameObject itemDetails;
   


    // Start is called before the first frame update
    void Start()
    {
        itemDetails = GameObject.Find("Canvas/Combat Screen/Inventory Display/ItemDetails");
        inventoryDisplay = GameObject.Find("Canvas/Combat Screen/Inventory Display");

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

    public void OnClick()
    {
        item.UseItem();
        inventoryDisplay.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDetails.SetActive(true);
        itemDetails.GetComponent<ItemDetailDisplay>().UpdateInfo(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        itemDetails.SetActive(false);
    }
}
