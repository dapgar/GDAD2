using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine.UI;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using System.Net;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOSE, FLEE,}

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
    public TextMeshProUGUI enemyNameUI;
    public TextMeshProUGUI battleText;
    public GameObject playerSprite;
    string enemyName;
    GameObject enemySprite;
    public Animator crossfadeAnim;
    public GameObject inventoryDisplay;
    public GameObject spellListDisplay;

    [Header("Test Mode")]
    public bool autoStart = true;

    public bool inCombat;

    public GameObject playerHUD;
    public GameObject[] playerHearts;
    public GameObject[] playerMana;
    public GameObject[] enemyHearts;

    private BattleState battleState;

    private bool hasClicked = true;

    private DialogueBox dialogueBoxManager;

    public int turnNumber;

    private bool isBlinded = false;
    private int blindCountdown;

    private bool isCorrosion = false;
    private int corrosionCountdown;

    private bool isDisoriented = false;
    private int disorientedCountdown;

    //[Header("Animations")]
    //public Animator playerAnimation;

    public EnemyStatus[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindObjectsByType<EnemyStatus>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        playerSprite.SetActive(true);

        combatScreen.SetActive(false);
        playerStatus = player.GetComponent<PlayerStatus>();
        dialogueBoxManager = dialogueBoxManager = GameObject.FindGameObjectWithTag("DialogueBoxManager").GetComponent<DialogueBox>();
        HidePlayerHUD();
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
        battleText.text = "";

        inCombat = true;

        HidePlayerHUD();

        battleState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    // Combat methods
    IEnumerator BeginBattle()
    {
        // cross fades into the battle screen
        crossfadeAnim.SetTrigger("Start");

        combatScreen.SetActive(true);

        yield return new WaitForSeconds(1f);

        // Player's turn
        if (enemy.tag == "TrainingDummy")
        {
            dialogueBoxManager.ContinueInteraction(0, 0, 5);
        }

        battleState = BattleState.PLAYERTURN;
        yield return StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        // release the blockade on clicking 
        // so that player can click on 'attack' button
        ResetStatus();
        battleText.text = "";
        ShowPlayerHUD();
        hasClicked = false;
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        HidePlayerHUD();
        yield return new WaitForSeconds(1f);
        battleText.text = "";

        if (enemyStatus.currentHealth > 0)
        {
            Attack(player);
            battleState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    IEnumerator EndBattle()
    {
        // Check if player won
        if (battleState == BattleState.WIN)
        {
            // display message here.
            enemySprite.SetActive(false);
            enemy.SetActive(false);
            yield return new WaitForSeconds(1);
            //crossfadeAnim.SetTrigger("Start"); // HEY OVER HERE
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
        }
        // Check if player lost.
        else if (battleState == BattleState.LOSE)
        {
            //playerSprite.SetActive(false);
            // display message here.
            yield return new WaitForSeconds(1);
            combatScreen.SetActive(false);
            enemySprite.SetActive(false);
            // Maybe transition scenes.
            Reset();
        }
        else if (battleState == BattleState.FLEE)
        {
            yield return new WaitForSeconds(1);
            crossfadeAnim.SetTrigger("Start"); // HEY OVER HERE
            dialogueBoxManager.EndDialogue();
            yield return new WaitForSeconds(1);
            enemyStatus.currentHealth = enemyStatus.maxHealth;
            enemySprite.SetActive(false);
            combatScreen.SetActive(false);
            
        }

        inCombat = false;
    }

    // Combat Buttons
    public void OnAttackButtonPress()
    {
        CloseMenus();
        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            Attack(enemy);

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
        if (!hasClicked && spellListDisplay.activeSelf == false)
        {
            playerStatus.inventory.GetComponent<Spellbook>().Display(combatScreen.transform);
        }
        else
        {
            CloseMenus();
        }
    }

    
    public void SpellUsed(Spell spell)
    {
        CloseMenus();
        hasClicked = true;
        float rand = Random.Range(0, 101);

        if (rand < playerStatus.accuracy)
        {
            // Spell logic
            switch (spell.spellData.name)
            {
                case "blindingburst":
                    enemyStatus.accuracy = 10.0f;
                    battleText.text = "Enemy Blinded!";
                    break;

                case "corrosion":
                    enemyStatus.atkDamage -= 1;
                    battleText.text = "Enemy Weakened!";
                    break;

                case "disorient":
                    enemyStatus.isDisoriented = true;
                    battleText.text = "Enemy DIsoriented!";
                    break;
            }
        }
        else
        {
            battleText.text = "Spell Missed.";
        }
        battleState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnItemButtonPress()
    {
        spellListDisplay.SetActive(false);

        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        // allow only a single action per turn
        if (!hasClicked && inventoryDisplay.activeSelf == false)
        {

            playerStatus.inventory.GetComponent<Inventory>().Display(combatScreen.transform);
        }
        else
        {
            CloseMenus();
        }
    }

    public void ItemUsed(InventoryItem item)
    {
        //battleState = BattleState.ENEMYTURN;
        //StartCoroutine(EnemyTurn());

        hasClicked = true;
        CloseMenus();

        switch (item.itemData.id)
        {
            case 1: //heal from potion.
                playerStatus.Heal(2);
                break;
            case 2: //cold iron
                playerStatus.coldIron_itemUsed = true;
                break;
            case 3: //spider item
                playerStatus.flameBottle_itemUsed = true;
                break;
        }

        StartCoroutine(PlayerTurn());
    }

    public void OnFleeButtonPress()
    {
        CloseMenus();

        // don't allow player to click on 'attack' unless player turn
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (!hasClicked)
        {
            hasClicked = true;

            battleState = BattleState.FLEE;
            StartCoroutine(EndBattle());
        }
    }

    private void CloseMenus()
    {
        if (inventoryDisplay.activeSelf == true) { inventoryDisplay.SetActive(false); }
        if (spellListDisplay.activeSelf == true) { spellListDisplay.SetActive(false); }
    }


    // Button Effects
    private void Attack(GameObject target)
    {
        float rand = Random.Range(0, 101);
        // Enemy Attack Logic
        if (target.CompareTag("Player") && rand < enemyStatus.accuracy && !enemyStatus.isDisoriented)
        {
            target.GetComponent<PlayerStatus>().TakeDamage(enemyStatus.atkDamage);
            //playerAnimation.SetTrigger("Hit");
        }
        else if (enemyStatus.isDisoriented && rand < enemyStatus.accuracy)
        {
            enemyStatus.TakeDamage(enemyStatus.atkDamage);
        }
        // Player Attack Logic
        else if (!target.CompareTag("Player"))
        {
            if (playerStatus.flameBottle_itemUsed && enemyStatus.race == "Spider" && rand < playerStatus.accuracy)
            {
                target.GetComponent<EnemyStatus>().TakeDamage(playerStatus.flameBottleDamage);
                playerStatus.flameBottle_itemUsed = false;
            }
            else if (playerStatus.coldIron_itemUsed && enemyStatus.race == "Fae" && rand < playerStatus.accuracy)
            {
                target.GetComponent<EnemyStatus>().TakeDamage(playerStatus.coldIronDamage);
                playerStatus.coldIron_itemUsed = false;
            }
            else if (rand < playerStatus.accuracy)
            {
                target.GetComponent<EnemyStatus>().TakeDamage(playerStatus.atkDamage);
            }
            else
            {
                battleText.text = "Attack Missed.";
            }
            //playerAnimation.SetTrigger("Attack");
        }
        else
        {
            battleText.text = "Attack Missed.";
        }
    }

    private void ResetStatus()
    {
        enemyStatus.accuracy = enemyStatus.defaultAccuracy;
        enemyStatus.isDisoriented = false;
        playerStatus.atkDamage = playerStatus.defaultAtkDamage;
        playerStatus.accuracy = playerStatus.defaultAccuracy;
    }

    // Show/Hide buttons
    private void ShowPlayerHUD()
    {
        playerHUD.SetActive(true);
    }

    private void HidePlayerHUD()
    {
        playerHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null && playerStatus.currentHealth <= 0)
        {
            battleState = BattleState.LOSE;
            StartCoroutine(EndBattle());
        }
        if (enemy != null && enemyStatus.currentHealth <= 0)
        {
            battleState = BattleState.WIN;
            StartCoroutine(EndBattle());
        }

        playerHealth.text = playerStatus.currentHealth + "/" + playerStatus.maxHealth;

        if (enemy != null)
        {
            enemyHealth.text = enemyStatus.currentHealth + "/" + enemyStatus.maxHealth;
        }

        // Handles Hearts Display
        if (inCombat)
        {
            switch (playerStatus.currentHealth)
            {
                case 5:
                    playerHearts[4].SetActive(true);
                    playerHearts[3].SetActive(true);
                    playerHearts[2].SetActive(true);
                    playerHearts[1].SetActive(true);
                    playerHearts[0].SetActive(true);
                    break;
                case 4:
                    playerHearts[4].SetActive(false);
                    playerHearts[3].SetActive(true);
                    playerHearts[2].SetActive(true);
                    playerHearts[1].SetActive(true);
                    playerHearts[0].SetActive(true);
                    break;
                case 3:
                    playerHearts[4].SetActive(false);
                    playerHearts[3].SetActive(false);
                    playerHearts[2].SetActive(true);
                    playerHearts[1].SetActive(true);
                    playerHearts[0].SetActive(true);
                    break;
                case 2:
                    playerHearts[4].SetActive(false);
                    playerHearts[3].SetActive(false);
                    playerHearts[2].SetActive(false);
                    playerHearts[1].SetActive(true);
                    playerHearts[0].SetActive(true);
                    break;
                case 1:
                    playerHearts[4].SetActive(false);
                    playerHearts[3].SetActive(false);
                    playerHearts[2].SetActive(false);
                    playerHearts[1].SetActive(false);
                    playerHearts[0].SetActive(true);
                    break;
                case 0:
                    playerHearts[4].SetActive(false);
                    playerHearts[3].SetActive(false);
                    playerHearts[2].SetActive(false);
                    playerHearts[1].SetActive(false);
                    playerHearts[0].SetActive(false);
                    break;
            }
            switch (playerStatus.currentMana)
            {
                case 3:
                    playerMana[2].SetActive(true);
                    playerMana[1].SetActive(true);
                    playerMana[0].SetActive(true);
                    break;
                case 2:
                    playerMana[2].SetActive(false);
                    playerMana[1].SetActive(true);
                    playerMana[0].SetActive(true);
                    break;
                case 1:
                    playerMana[2].SetActive(false);
                    playerMana[1].SetActive(false);
                    playerMana[0].SetActive(true);
                    break;
                case 0:
                    playerMana[2].SetActive(false);
                    playerMana[1].SetActive(false);
                    playerMana[0].SetActive(false);
                    break;
            }
            switch (enemyStatus.currentHealth)
            {
                case 3:
                    enemyHearts[2].SetActive(true);
                    enemyHearts[1].SetActive(true);
                    enemyHearts[0].SetActive(true);
                    break;
                case 2:
                    enemyHearts[2].SetActive(false);
                    enemyHearts[1].SetActive(true);
                    enemyHearts[0].SetActive(true);
                    break;
                case 1:
                    enemyHearts[2].SetActive(false);
                    enemyHearts[1].SetActive(false);
                    enemyHearts[0].SetActive(true);
                    break;
                case 0:
                    enemyHearts[2].SetActive(false);
                    enemyHearts[1].SetActive(false);
                    enemyHearts[0].SetActive(false);
                    break;
            }
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

            case BattleState.START:
                turnIndicator.text = " ";
                break;

            case BattleState.FLEE:
                turnIndicator.text = "You Ran Away!";
                PlayerController playerControl = player.GetComponent<PlayerController>();
                break;
        }
    }

    public BattleState getBattleState()
    {
        return battleState;
    }

    //Resets all NPCs, Enemies and player to default values
    public void Reset()
    {
        /*EnemyStatus[]*/ enemies = GameObject.FindObjectsByType<EnemyStatus>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (EnemyStatus enemy in enemies)
        {
            enemy.Reset();
        }
        NPCScript[] npcs = GameObject.FindObjectsByType<NPCScript>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (NPCScript npc in npcs)
        {
            npc.Reset();
        }
        playerStatus.Reset();
        //ADD SOMETHING FOR ALLIES HERE LATER

        Debug.Log("Number of Enemies: " + enemies.Length);
        Debug.Log("Number of NPCs: " + npcs.Length);
    }
}
