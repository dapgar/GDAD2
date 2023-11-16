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
    //public Image icon;

    public InventoryItem item;

    public GameObject inventoryDisplay;

    public GameObject itemDetails;
    private ItemDetailDisplay details;



    // Start is called before the first frame update
    void Start()
    {
        itemDetails = GameObject.Find("Canvas/Combat Screen/Inventory Display/Item Details");
        details = itemDetails.GetComponent<ItemDetailDisplay>();
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
        //if (icon != null)
        //{
        //    icon.sprite = item.itemData.icon;
        //}
    }

    public void OnClick()
    {
        item.UseItem();
        inventoryDisplay.GetComponent<InventoryDisplay>().inventory.ItemUsed(item);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //itemDetails.SetActive(true);
        if (details.itemName.gameObject.activeSelf == false) { details.itemName.gameObject.SetActive(true); }
        if (details.itemDescription.gameObject.activeSelf == false) { details.itemDescription.gameObject.SetActive(true); }
        if (details.icon.gameObject.activeSelf == false) { details.icon.gameObject.SetActive(true); }

        details.UpdateInfo(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        //itemDetails.SetActive(false);
    }
}
