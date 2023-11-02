using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTriggers : MonoBehaviour
{
    CombatManager combatManager;
    NPCManager npcManager;
    GameObject player;
    public float detectionRadius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
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
            //Debug.Log("FIGHT TRIGGERED");
            //Debug.Log("Detection Radius: " + detectionRadius);
            //Debug.Log("distance: " + distance);
            if (distance <= detectionRadius && !combatManager.inCombat)
            {
                combatManager.StartCombat(this.gameObject);
            }
        }

        NPCScript npcScript = this.gameObject.GetComponent<NPCScript>();
        if (this.gameObject.GetComponent("NPCScript") != null)
        {
            //Debug.Log("distance: " + distance);
            if (distance <= detectionRadius && !npcManager.inConversation && !npcScript.interactedWith)
            {
                //Debug.Log("NPC DIALOGUE TRIGGERED");
                npcManager.StartInteraction(this.gameObject);
            }
            else if(distance >= detectionRadius + 0.5f)
            {
                npcScript.ResetInteraction();
            }
        }
    }
}
