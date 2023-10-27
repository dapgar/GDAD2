using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
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
        foreach (InventoryItem item in items)
        {
            InventoryItemDisplay display = (InventoryItemDisplay)Instantiate(itemDisplayPrefab);
            display.transform.SetParent(targetTransform, true);
            display.Prime(item);
        }
    }
}
