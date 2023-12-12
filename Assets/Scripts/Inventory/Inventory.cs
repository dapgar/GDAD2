using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    public InventoryDisplay inventoryDisplay;

    public bool itemSelected = false;

    public GameObject player;
    public GameObject combatManager;

    //Hardcoded ItemData for Testing
    public ItemData potion;
    public ItemData bottle;
    public ItemData iron;

    public void Display(Transform parentTransform)
    {
        //InventoryDisplay inventoryDisplay = (InventoryDisplay)Instantiate(inventoryDisplayPrefab);
        //inventoryDisplay.transform.SetParent(parentTransform, false);

        itemSelected = false;
        inventoryDisplay.Prime(inventory);

    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Add(potion);
            Add(bottle);
            Add(iron);
        }
        
    }

    public void Add(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
    }


    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if(item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }

    public void ItemUsed(InventoryItem item)
    {
       
        if(item.stackSize >= 0)
        {
            itemSelected = true;
            combatManager.GetComponent<CombatManager>().ItemUsed(item);
        }
        
    }

    public void RefillPotion()
    { 

        foreach (InventoryItem item in inventory)
        {
            item.stackSize = item.maxSize;
        }
    }
}
