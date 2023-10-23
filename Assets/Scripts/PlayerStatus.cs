using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;

    public float atkDamage = 1;
    public float mgkDamage = 1;
    public float parDamage = 1;
    //public Vector3 startingPosition;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
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
