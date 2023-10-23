using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;

    public float atkDamage = 1;
    public float mgkDamage = 1;
    public float parDamage = 1;

    public int attackBias;
    public int[] defendBias = new int[2];

    public string enemyName;
    //public Vector3 startingPosition;

    public GameObject enemySprite;

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
    //    transform.position = startingPosition;
    //    currentHealth = maxHealth;
    //    this.gameObject.SetActive(true);
    //}
}
