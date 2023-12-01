using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;

    public int atkDamage = 1;
    public int defaultAtkDamage;

    public float accuracy = 90;
    public float defaultAccuracy;

    public string enemyName;
    public string race;
    public Vector3 startingPosition;

    public bool isDisoriented = false;

    public GameObject enemySprite;

    private void Start()
    {
        currentHealth = maxHealth;
        startingPosition = transform.position;
        //Debug.Log(enemyName + " TRANSFORM POSITON: " + transform.position);
        defaultAtkDamage = atkDamage;
        defaultAccuracy = accuracy;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public void Reset()
    {
        // TRANSFORM POSITON ISN'T CORRECT FIX IN FUTURE
        //Debug.Log(enemyName + " TRANSFORM POSITON: " + transform.position);
        //Debug.Log(enemyName + " START POSITON: " + startingPosition);
        //transform.position = startingPosition;
        currentHealth = maxHealth;
        this.gameObject.SetActive(true);
    }
}
