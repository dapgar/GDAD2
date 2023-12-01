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
    public GameObject promptImage;
    public float detectionRadius = 2.0f;
    private bool enemyInteractedWith;

    [Header("Animation")]
    public Animator crossfadeAnim;

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
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (this.gameObject.GetComponent("EnemyStatus") != null)
        {
            //if(distance <= detectionRadius && !enemyInteractedWith)
            //{
            //    Debug.Log("CAN START COMBAT // COMBAT MANAGER INCOMBAT: " + combatManager.inCombat);    
            //}

            if (distance <= detectionRadius && !combatManager.inCombat && !enemyInteractedWith)
            {
                combatManager.StartCombat(this.gameObject); 
                enemyInteractedWith = true;

                //Debug.Log("STARTED COMBAT FROM INTERACTION ");
            }
            if (distance > detectionRadius + 0.1f)
            {
                enemyInteractedWith = false;
                //Debug.Log("RESET ENEMY INTERACTION");
            }
        }

        NPCScript npcScript = this.gameObject.GetComponent<NPCScript>();
        if (this.gameObject.GetComponent("NPCScript") != null)
        {
            // Uses full dialogue screen
            if (distance <= detectionRadius && !npcManager.inConversation && !npcScript.interactedWith && npcScript.needsInteractionScreen)
            {
                npcScript.ePrompt.SetActive(true);
                if(Input.GetKey(KeyCode.E))
                {
                    npcManager.StartInteraction(this.gameObject, 0);
                }
            }
            else if(distance > detectionRadius || npcScript.interactedWith)
            {
                npcScript.ePrompt.SetActive(false);
            }
            // Uses dialogue box
            //if (distance <= detectionRadius && !dialogueBoxManager.inConversation && !npcScript.interactedWith && !npcScript.needsInteractionScreen)
            //{
            //    //Debug.Log("START DIALOGUE BOX");
            //    dialogueBoxManager.ContinueInteraction(0, 0, 1);
            //}
            // If player is out of range reset interaction so it can be interacted with again
           if (distance >= detectionRadius + 0.5f)
            {
                npcScript.ResetInteraction();
            }
        }
    }
}
