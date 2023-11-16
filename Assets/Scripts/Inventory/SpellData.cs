using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]

public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite icon;
    public int cost;
    public int id;


    [TextArea]
    public string description;
}
