using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTriggers : MonoBehaviour
{
    CombatManager combatManager;
    NPCManager npcManager;
    DialogueBox dialogueBoxManager;
    GameObject player;
    public float detectionRadius = 2.0f;
    private bool enemyInteractedWith;

    [Header("Animation")]
    public Animator crossfadeAnim;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
        dialogueBoxManager = GameObject.FindGameObjectWithTag("DialogueBoxManager").GetComponent<DialogueBox>();

        enemyInteractedWith = false;
    }

    // Update is called once per frame
    void Update()
    {
        detectNPC();
    }

    private void detectNPC()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (this.gameObject.GetComponent("EnemyStatus") != null)
        {
            if (distance <= detectionRadius && !combatManager.inCombat && !enemyInteractedWith)
            {
                combatManager.StartCombat(this.gameObject);
                enemyInteractedWith = true;
            }
            if (distance > detectionRadius + 0.5f)
            {
                enemyInteractedWith = false;
            }
        }

        NPCScript npcScript = this.gameObject.GetComponent<NPCScript>();
        if (this.gameObject.GetComponent("NPCScript") != null)
        {
            if(npcScript.ePrompt != null)
            {
                npcScript.ePrompt.SetActive(false);
            }
            
            // Uses full dialogue screen
            if (distance <= detectionRadius && !npcManager.inConversation && !npcScript.interactedWith && npcScript.needsInteractionScreen)
            {
                if (npcScript.ePrompt != null)
                {
                    npcScript.ePrompt.SetActive(true);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    npcManager.StartInteraction(this.gameObject, 0);
                }
            }

            // Uses dialogue box
            if (distance <= detectionRadius && !dialogueBoxManager.inConversation && !npcScript.interactedWith && !npcScript.needsInteractionScreen)
            {
                Debug.Log("START DIALOGUE BOX");
                dialogueBoxManager.ContinueInteraction(0, 5, 1);
            }
            // If player is out of range reset interaction so it can be interacted with again
            if (distance > detectionRadius + 0.25f)
            {
                npcScript.ResetInteraction();
            }
        }
    }
}
