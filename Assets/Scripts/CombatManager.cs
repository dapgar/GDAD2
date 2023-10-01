using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE, OUTCOME }

public class CombatManager : MonoBehaviour
{
    public GameObject combatScreen;

    [Header("Characters")]
    public GameObject enemy;
    public GameObject player;

    private EnemyStatus enemyStatus;
    private PlayerStatus playerStatus;

    [Header("UI Elements")]
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI enemyHealth;
    public TextMeshProUGUI turnIndicator;
    public TextMeshProUGUI battleOutcome;

    public GameObject[] combatIcons;
    public GameObject[] combatButtons;

    private int playerChoice;
    private int enemyChoice;

    private BattleState battleState;

    private bool hasClicked = true;

    // Start is called before the first frame update
    void Start()
    {
        enemyStatus = enemy.GetComponent<EnemyStatus>();
        playerStatus = player.GetComponent<PlayerStatus>();
    }

    // Use this method to trigger the combat sequence.
    public void StartCombat()
    {
        combatScreen.SetActive(true);
        foreach (GameObject button in combatButtons)
        {
            button.SetActive(false);
        }

        battleState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    IEnumerator BeginBattle()
    {
        // Spawn in player
        // Spawn in enemy

        // Set HUDs

        yield return new WaitForSeconds(1f);

        // Fade in character sprites

        // Player's turn
        battleState = BattleState.PLAYERTURN;
        yield return StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        // release the blockade on clicking 
        // so that player can click on 'attack' button
        foreach (GameObject button in combatButtons)
        {
            button.SetActive(true);
        }
        hasClicked = false;
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        foreach (GameObject button in combatButtons)
        {
            button.SetActive(false);
        }

        yield return new WaitForSeconds(2);
        enemyChoice = Random.Range(1, 3);
        combatIcons[enemyChoice + 2].SetActive(true);

        battleState = BattleState.OUTCOME;
        StartCoroutine(CombatOutcome());
    }

    IEnumerator EndBattle()
    {
        // Check if player won
        if (battleState == BattleState.WIN)
        {
            // display message here.
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
        }
        // Check if player lost.
        else if (battleState == BattleState.LOSE)
        {
            // display message here.
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
            // Maybe transition scenes.
        }
    }

    public void OnAttackButtonPress()
    {
        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            playerChoice = 1;
            combatIcons[0].SetActive(true);
            // block user from repeatedly pressing attack button  
            hasClicked = true;

            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnDefendButtonPress()
    {
        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            playerChoice = 2;
            combatIcons[1].SetActive(true);
            // block user from repeatedly pressing attack button  
            hasClicked = true;

            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnMagicButtonPress()
    {
        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            playerChoice = 3;
            combatIcons[2].SetActive(true);
            // block user from repeatedly pressing attack button  
            hasClicked = true;

            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator CombatOutcome()
    {
        // Battle Logic
        #region
        if (playerChoice == 1 && enemyChoice == 1)
        {
            // Both atk
            playerStatus.TakeDamage(10f);
            enemyStatus.TakeDamage(10f);
            battleOutcome.text = "Both Attacked!";
        }
        else if (playerChoice == 2 && enemyChoice == 1)
        {
            // Player def, enemy atk
            battleOutcome.text = "Player Blocked Attack!";
        }
        else if (playerChoice == 3 && enemyChoice == 1)
        {
            // Player mgk, enemy atk
            playerStatus.TakeDamage(30f);
            battleOutcome.text = "Enemy Punished Magic!";
        }
        else if (playerChoice == 1 && enemyChoice == 2)
        {
            // Player atk, enemy def
            battleOutcome.text = "Enemy Blocked Attack!";
        }
        else if (playerChoice == 2 && enemyChoice == 2)
        {
            // Both def
            battleOutcome.text = "Both Blocked!";
        }
        else if (playerChoice == 3 && enemyChoice == 2)
        {
            // Player mgk, enemy def
            enemyStatus.TakeDamage(20f);
            battleOutcome.text = "Player Punished Block!";
        }
        else if (playerChoice == 1 && enemyChoice == 3)
        {
            // Player atk, enemy mgk
            enemyStatus.TakeDamage(30f);
            battleOutcome.text = "Player Punished Magic!";
        }
        else if (playerChoice == 2 && enemyChoice == 3)
        {
            // Player def, enemy mgk
            playerStatus.TakeDamage(20f);
            battleOutcome.text = "Enemy Punished Block!";
        }
        else if (playerChoice == 3 && enemyChoice == 3)
        {
            // Both mgk
            playerStatus.TakeDamage(20f);
            enemyStatus.TakeDamage(20f);
            battleOutcome.text = "Both Used Magic!";
        }
        #endregion

        yield return new WaitForSeconds(2f);

        battleOutcome.text = " ";
        foreach (GameObject icon in combatIcons)
        {
            icon.SetActive(false);
        }

        if (enemyStatus.currentHealth <= 0)
        {
            // if the enemy health drops to 0 player wins.
            battleState = BattleState.WIN;
            yield return StartCoroutine(EndBattle());
        }
        else if (playerStatus.currentHealth <= 0)
        {
            // if the player health drops to 0 the enemy wins.
            battleState = BattleState.LOSE;
            yield return StartCoroutine(EndBattle());
        }
        else
        {
            // if the player health is still above 0 
            // when the turn finishes it's player's turn
            battleState = BattleState.PLAYERTURN;
            yield return StartCoroutine(PlayerTurn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerHealth.text = playerStatus.currentHealth + "/" + playerStatus.maxHealth;
        enemyHealth.text = enemyStatus.currentHealth + "/" + enemyStatus.maxHealth;

        switch(battleState)
        {
            case BattleState.PLAYERTURN:
                turnIndicator.text = "Your Turn";
                break;

            case BattleState.ENEMYTURN:
                turnIndicator.text = "Enemy's Turn";
                break;

            case BattleState.WIN:
                turnIndicator.text = "Enemy Slain";
                break;

            case BattleState.LOSE:
                turnIndicator.text = "You've Died";
                break;

            case BattleState.OUTCOME:
            case BattleState.START:
                turnIndicator.text = " ";
                break;
        }
    }
}
