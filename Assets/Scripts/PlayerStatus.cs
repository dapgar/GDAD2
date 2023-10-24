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
    //public Vector3 startingPosition;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    //public void Reset()
    //{
    //    currentHealth = maxHealth; 
    //    transform.position = startingPosition;
    //}
}
