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

    [Header("Animation")]
    public Animator crossfadeAnim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
        dialogueBoxManager = GameObject.FindGameObjectWithTag("DialogueBoxManager").GetComponent<DialogueBox>();
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
            if (distance <= detectionRadius && !combatManager.inCombat)
            {
                combatManager.StartCombat(this.gameObject);
            }
        }
        
        NPCScript npcScript = this.gameObject.GetComponent<NPCScript>();
        if (this.gameObject.GetComponent("NPCScript") != null)
        {
            // Uses full dialogue screen
            if (distance <= detectionRadius && !npcManager.inConversation && !npcScript.interactedWith && npcScript.needsInteractionScreen)
            {
                npcManager.StartInteraction(this.gameObject);
            }
            // Uses dialogue box
            if (distance <= detectionRadius && !dialogueBoxManager.inConversation && !npcScript.interactedWith && !npcScript.needsInteractionScreen)
            {
                //Debug.Log("START DIALOGUE BOX");
                dialogueBoxManager.ContinueInteraction(0, -1, 2);
            }
            // If player is out of range reset interaction so it can be interacted with again
            if(distance >= detectionRadius + 0.5f)
            {
                npcScript.ResetInteraction();
            }
        }

        if (this.gameObject.GetComponent<AreaTransiton>() != null && distance <= detectionRadius && Input.GetKey("e") && !npcManager.inConversation && !combatManager.inCombat)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                crossfadeAnim.SetTrigger("Start");

                // make actual place transition wait until fade is over
                float counter = 0;
                float waitTime = 5;
                while (counter < waitTime)
                {
                    //Increment Timer until counter >= waitTime
                    counter += Time.deltaTime;
                    Debug.Log("We have waited for: " + counter + " seconds");
                    //Wait for a frame so that Unity doesn't freeze
                }

                playerController.MoveToArea(this.gameObject.GetComponent<AreaTransiton>());
            }
        }
    }
}
