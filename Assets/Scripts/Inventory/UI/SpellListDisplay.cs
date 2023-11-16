using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellListDisplay : MonoBehaviour
{
    public Spellbook spells;

    public Transform targetTransform;
    public SpellDisplay spellDisplayPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Prime(List<Spell> spells)
    {
        Debug.Log("Spell List Primed");
        gameObject.SetActive(true);

        foreach (Transform child in targetTransform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Spell spell in spells)
        {
            if (spell.stackSize > 0)
            {
                SpellDisplay display = (SpellDisplay)Instantiate(spellDisplayPrefab);
                display.transform.SetParent(targetTransform, false);
                display.Prime(spell);
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
