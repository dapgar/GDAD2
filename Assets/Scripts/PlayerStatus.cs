using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 5;
    
    public int atkDamage = 1;
    public int mgkDamage = 1;
    public int parDamage = 1;
    public Vector3 startingPosition;

    public GameObject inventory;

    public bool coldIron_itemUsed = false;
    public bool flameBottle_itemUsed = false;

    private void Start()
    {
        currentHealth = 2;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void Heal(int amount)
    {
        for (int i = 0;i < amount; i++)
        {
            if(currentHealth+1 < maxHealth)
            {
                currentHealth++;
            }
        }
  
    }


    //public void Reset()
    //{
    //    currentHealth = maxHealth;
    //    transform.position = startingPosition;
    //}
}
