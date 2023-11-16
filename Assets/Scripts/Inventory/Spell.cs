using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]

public class Spell
{
    public SpellData spellData;
    public int stackSize;
    public int maxSize = 3;

    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
        stackSize++;
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void UseItem()
    {

    }
}
