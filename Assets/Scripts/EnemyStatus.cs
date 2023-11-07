using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public int atkDamage = 1;
    public int mgkDamage = 1;

    public int attackBias;
    public int[] defendBias = new int[2];

    public string enemyName;
    public string race;
    public Vector3 startingPosition;

    public GameObject enemySprite;

    private void Start()
    {
        currentHealth = maxHealth;
        //startingPosition = transform.position;
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
    //    transform.position = startingPosition;
    //    currentHealth = maxHealth;
    //    this.gameObject.SetActive(true);
    //}
}
