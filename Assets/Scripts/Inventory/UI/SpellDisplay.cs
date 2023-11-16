using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SpellDisplay : MonoBehaviour
{
    public TextMeshProUGUI spellName;
    public TextMeshProUGUI spellCost;
    public Image icon;

    public Spell spell;

    public GameObject spellListDisplay;

    //public GameObject spellDetails;



    // Start is called before the first frame update
    void Start()
    {
        spellListDisplay = GameObject.Find("Canvas/Combat Screen/Spell List");

        if (spell != null) { Prime(spell); }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Prime(Spell spell)
    {
        this.spell = spell;
        if (spellName != null)
        {
            spellName.text = spell.spellData.spellName;
        }
        if (spellCost != null)
        {
            spellCost.text = spell.spellData.cost + " MP";
        }
        if (icon != null)
        {
            icon.sprite = spell.spellData.icon;
        }
    }

    public void OnClick()
    {
        spellListDisplay.GetComponent<SpellListDisplay>().spells.SpellUsed(spell);
        spellListDisplay.SetActive(false);
    }
}
