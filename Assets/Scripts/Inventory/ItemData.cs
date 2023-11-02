using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int id;


    [TextArea]
    public string description;
}
