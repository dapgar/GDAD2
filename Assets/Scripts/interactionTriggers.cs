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

    [Header("Animation")]
    public Animator crossfadeAnim;

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
            if (distance <= detectionRadius && !combatManager.inCombat)
            {
                combatManager.StartCombat(this.gameObject);
            }
        }

        NPCScript npcScript = this.gameObject.GetComponent<NPCScript>();
        if (this.gameObject.GetComponent("NPCScript") != null)
        {
            if (distance <= detectionRadius && !npcManager.inConversation && !npcScript.interactedWith)
            {
                npcManager.StartInteraction(this.gameObject);
            }
            else if(distance >= detectionRadius + 0.5f)
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
