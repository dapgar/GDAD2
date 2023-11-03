using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public Inventory inventory;

    public Transform targetTransform;
    public InventoryItemDisplay itemDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Prime(List<InventoryItem> items)
    {
        gameObject.SetActive(true);

        foreach (Transform child in targetTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (InventoryItem item in items)
        {
            if(item.stackSize > 0 || item.itemData.id == 1)
            {
                InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(itemDisplayPrefab);
                display.transform.SetParent(targetTransform, false);
                display.Prime(item);
            }

            
        }
    }

    public void UpdateList()
    {

    }
}
