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

        if(this.gameObject.GetComponent("EnemyStatus") != null)
        {
            Debug.Log("FIGHT TRIGGERED");
            Debug.Log(detectionRadius);
            if (distance <= detectionRadius && !combatManager.inCombat)
            {
                Debug.Log("FIGHT TRIGGERED PT 2");
                combatManager.StartCombat(this.gameObject);
            }
        }

        //if (this.gameObject.GetComponent("NPCScript") != null)
        //{
        //    if (distance <= detectionRadius && !npcManager.inConversation)
        //    {
        //        Debug.Log("NPC DIALOGUE TRIGGERED");
        //        npcManager.StartInteraction(this.gameObject);
        //    }
        //}
    }
}
