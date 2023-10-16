using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 100;

    public float atkDamage = 10;
    public float mgkDamage = 30;

    public int attackBias;
    public int[] defendBias = new int[2];

    public GameObject enemySprite;
    CombatManager combatManager;

    private void Start()
    {
        currentHealth = maxHealth;
        combatManager = GameObject.FindObjectOfType<CombatManager>();
        combatManager.attackBias = attackBias;
        combatManager.defendBias = defendBias;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
}
