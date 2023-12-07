using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ManaBar : MonoBehaviour
{
    public GameObject manaIcon;
    public Transform targetTransform;

    public bool populated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Populate(Spell spell)
    {
        if (!populated)
        {
            for (int i = 0; i < spell.spellData.cost; i++)
            {
                GameObject manaObject = Instantiate(manaIcon);
                manaObject.transform.SetParent(targetTransform, false);
            }

            populated = true;
        }

        
        

    }
}