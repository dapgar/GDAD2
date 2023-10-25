using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine.UI;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE, OUTCOME }

public class CombatManager : MonoBehaviour
{
    public GameObject combatScreen;

    [Header("Characters")]
    public GameObject player;
    GameObject enemy;

    public int attackBias;
    public int[] defendBias = new int[2];

    private EnemyStatus enemyStatus;
    private PlayerStatus playerStatus;

    [Header("UI Elements")]
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI enemyHealth;
    public TextMeshProUGUI turnIndicator;
    public TextMeshProUGUI battleOutcome;
    public TextMeshProUGUI enemyNameUI;
    public GameObject playerSprite;
    string enemyName;
    GameObject enemySprite;
    public Animator crossfadeAnim; // HEY OVER HERE

    [Header("Test Mode")]
    public bool autoStart = true;

    public bool inCombat;

    public GameObject[] combatIcons;
    public GameObject[] combatButtons;
    public GameObject[] confirmButtons;
    public GameObject backButton;

    private int playerChoice;
    private int enemyChoice;

    private BattleState battleState;

    private bool hasClicked = true;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite.SetActive(true);

        combatScreen.SetActive(false);
        playerStatus = player.GetComponent<PlayerStatus>();

        if (autoStart)
        {
            //StartCombat();

            //Reset();
        }
    }

    // Use this method to trigger the combat sequence.
    public void StartCombat(GameObject newEnemy)
    {
        enemy = newEnemy;
        enemyStatus = enemy.GetComponent<EnemyStatus>();
        enemySprite = enemyStatus.enemySprite;
        enemySprite.SetActive(true);
        enemyName = enemyStatus.enemyName;
        enemyNameUI.text = enemyName;
        attackBias = enemyStatus.attackBias;
        defendBias = enemyStatus.defendBias;

        inCombat = true;

        HideCombatButtons();
        HideConfirmButtons();

        battleState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    // Combat methods
    IEnumerator BeginBattle()
    {
        // Spawn in player
        // Spawn in enemy

        // Set HUDs

        // cross fades into the battle screen
        crossfadeAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1f); // has to wait a second to make sure that the fade starts before the battle screen appears

        combatScreen.SetActive(true);

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
        ShowCombatButtons();
        hasClicked = false;
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        HideCombatButtons();
        HideConfirmButtons();

        yield return new WaitForSeconds(1);

        enemyChoice = Random.Range(1, 11);
        // Attack Chance
        if (enemyChoice <= attackBias)
        {
            combatIcons[3].SetActive(true);
            enemyChoice = 1;
        }
        // Defend Chance
        else if (enemyChoice >= defendBias[0] && enemyChoice <= defendBias[1])
        {
            combatIcons[4].SetActive(true);
            enemyChoice = 2;
        }
        // Magic Chance
        else
        {
            combatIcons[5].SetActive(true);
            enemyChoice = 3;
        }

        battleState = BattleState.OUTCOME;
        StartCoroutine(CombatOutcome());
    }

    IEnumerator EndBattle()
    {
        // Check if player won
        if (battleState == BattleState.WIN)
        {
            // display message here.
            enemySprite.SetActive(false);
            Destroy(enemy);
            //enemy.SetActive(false);
            yield return new WaitForSeconds(1);
            crossfadeAnim.SetTrigger("Start"); // HEY OVER HERE
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
        }
        // Check if player lost.
        else if (battleState == BattleState.LOSE)
        {
            playerSprite.SetActive(false);
            // display message here.
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
            // Maybe transition scenes.
        }

        inCombat = false;
    }

    IEnumerator CombatOutcome()
    {
        // Battle Logic
        #region
        if (playerChoice == 1 && enemyChoice == 1)
        {
            // Both atk
            playerStatus.TakeDamage(enemyStatus.atkDamage);
            enemyStatus.TakeDamage(playerStatus.atkDamage);
            battleOutcome.text = "Both Attacked!";
        }
        else if (playerChoice == 2 && enemyChoice == 1)
        {
            // Player def, enemy atk
            playerStatus.TakeDamage(-10f);

            // Stops overhealing
            if (playerStatus.currentHealth > playerStatus.maxHealth)
            {
                playerStatus.currentHealth = playerStatus.maxHealth;
            }
            battleOutcome.text = "Player Blocked Attack!";
        }
        else if (playerChoice == 3 && enemyChoice == 1)
        {
            // Player mgk, enemy atk
            playerStatus.TakeDamage(enemyStatus.mgkDamage / 2);
            battleOutcome.text = "Enemy Punished Magic!";
        }
        else if (playerChoice == 1 && enemyChoice == 2)
        {
            // Player atk, enemy def
            enemyStatus.TakeDamage(-10f);

            // Stops overhealing
            if (enemyStatus.currentHealth > enemyStatus.maxHealth)
            {
                enemyStatus.currentHealth = enemyStatus.maxHealth;
            }
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
            enemyStatus.TakeDamage(playerStatus.mgkDamage);
            battleOutcome.text = "Player Punished Block!";
        }
        else if (playerChoice == 1 && enemyChoice == 3)
        {
            // Player atk, enemy mgk
            enemyStatus.TakeDamage(playerStatus.mgkDamage / 2);
            battleOutcome.text = "Player Punished Magic!";
        }
        else if (playerChoice == 2 && enemyChoice == 3)
        {
            // Player def, enemy mgk
            playerStatus.TakeDamage(enemyStatus.mgkDamage);
            battleOutcome.text = "Enemy Punished Block!";
        }
        else if (playerChoice == 3 && enemyChoice == 3)
        {
            // Both mgk
            playerStatus.TakeDamage(enemyStatus.mgkDamage);
            enemyStatus.TakeDamage(playerStatus.mgkDamage);
            battleOutcome.text = "Both Used Magic!";
        }
        #endregion

        yield return new WaitForSeconds(1f);

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

    // Attack button methods
    public void OnAttackButtonPress()
    {
        HideCombatButtons();
        backButton.SetActive(true);
        confirmButtons[0].SetActive(true);
    }

    public void OnAttackConfirmPress()
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

    // Defend button methods
    public void OnDefendButtonPress()
    {
        HideCombatButtons();
        backButton.SetActive(true);
        confirmButtons[1].SetActive(true);
    }

    public void OnDefendConfirmPress()
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

    // Magic button methods
    public void OnMagicButtonPress()
    {
        HideCombatButtons();
        backButton.SetActive(true);
        confirmButtons[2].SetActive(true);
    }

    public void OnMagicConfirmPress()
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

    // Back button method
    public void OnBackButtonPress()
    {
        HideConfirmButtons();
        ShowCombatButtons();
    }

    // Show/Hide buttons
    private void ShowCombatButtons()
    {
        foreach (GameObject button in combatButtons)
        {
            button.SetActive(true);
        }
    }

    private void HideCombatButtons()
    {
        foreach (GameObject button in combatButtons)
        {
            button.SetActive(false);
        }
    }

    private void HideConfirmButtons()
    {
        foreach (GameObject button in confirmButtons)
        {
            button.SetActive(false);
        }
        backButton.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        playerHealth.text = playerStatus.currentHealth + "/" + playerStatus.maxHealth;

        if (enemy != null)
        {
            enemyHealth.text = enemyStatus.currentHealth + "/" + enemyStatus.maxHealth;
        }

        switch (battleState)
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

    // Resets all NPCs, Enemies and player to default values
    public void Reset()
    {
        //EnemyStatus[] enemies = GameObject.FindObjectsByType<EnemyStatus>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        //foreach (EnemyStatus enemy in enemies)
        //{ 
        //    enemy.Reset(); 
        //}
        //NPCScript[] npcs = GameObject.FindObjectsByType<NPCScript>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        //foreach (NPCScript npc in npcs)
        //{
        //    npc.Reset();
        //}
        //playerStatus.Reset();
        // ADD SOMETHING FOR ALLIES HERE LATER

        //Debug.Log("Number of Enemies: " + enemies.Length);
        //Debug.Log("Number of NPCs: " + enemies.Length);
    }
}
