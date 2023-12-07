using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    public List<Spell> spellbook = new List<Spell>();
    private Dictionary<SpellData, Spell> spellDictionary = new Dictionary<SpellData, Spell>();

    public SpellListDisplay spellListDisplay;

    //public bool spellSelected = false;

    public GameObject player;
    public GameObject combatManager;

    //Hardcoded ItemData for Testing
    public SpellData blindingburst;
    public SpellData corrosion;
    public SpellData disorient;


    public void Display(Transform parentTransform)
    {
        //InventoryDisplay inventoryDisplay = (InventoryDisplay)Instantiate(inventoryDisplayPrefab);
        //inventoryDisplay.transform.SetParent(parentTransform, false);

        spellListDisplay.Prime(spellbook);

    }

    private void Start()
    {
        Add(blindingburst);
        Add(corrosion);
        Add(disorient);
    }

    public void Add(SpellData spellData)
    {
        if (spellDictionary.TryGetValue(spellData, out Spell spell))
        {
            spell.AddToStack();
        }
        else
        {
            Spell newSpell = new Spell(spellData);
            spellbook.Add(newSpell);
            spellDictionary.Add(spellData, newSpell);
        }
    }


    public void Remove(SpellData spellData)
    {
        if (spellDictionary.TryGetValue(spellData, out Spell spell))
        {
            spell.RemoveFromStack();
            if (spell.stackSize == 0)
            {
                spellbook.Remove(spell);
                spellDictionary.Remove(spellData);
            }
        }
    }

    public void SpellUsed(Spell spell)
    {
        if (spell.spellData.name == "shockingray")
        {

        }
        if (player.GetComponent<PlayerStatus>().currentMana > 0)
        {
            player.GetComponent<PlayerStatus>().currentMana -= spell.spellData.cost;
            combatManager.GetComponent<CombatManager>().SpellUsed(spell);
        }
    }

}
