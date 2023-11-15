using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunspotManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] enemies;

    private PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    public void Rest()
    {
        playerStatus.currentHealth = playerStatus.maxHealth;
        playerStatus.inventory.GetComponent<Inventory>().RefillPotion();

        foreach (GameObject enemy in enemies)
        {
            //enemy.SetActive(true);
            //enemy.GetComponent<EnemyStatus>().currentHealth = enemy.GetComponent<EnemyStatus>().maxHealth;
            enemy.GetComponent<EnemyStatus>().Reset();
        }
    }
}