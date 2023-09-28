using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public PlayerManager player;
    public GameObject enemy;
    private int playerChoice;
    private int enemyChoice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to determine player choice.
    public void OnAttackPress()
    {
        playerChoice = 1;
        Debug.Log("Player Attacking");
    }

    public void OnDefendPress()
    {
        playerChoice = 2;
        Debug.Log("Player Defending");

    }

    public void OnMagicPress()
    {
        playerChoice = 3;
        Debug.Log("Player Used Magic");
    }
}
