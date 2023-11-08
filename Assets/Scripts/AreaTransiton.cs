using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class AreaTransiton : MonoBehaviour
{
    public float sendToAreaNumber = 1;
    [Tooltip("Starting position for the player in the next area")]
    public Vector3 playerStartingPosition = Vector3.zero;

    CombatManager combatManager;
    NPCManager npcManager;
    GameObject player;
    public GameObject promptImage;
    public float detectionRadius = 2.0f;

    [Header("Animation")]
    public Animator crossfadeAnim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
    }

    private void Update()
    {
        detectDoor();
    }

    private void detectDoor()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        // Area Transition (Door)
        if (this.gameObject.GetComponent<AreaTransiton>() != null && distance <= detectionRadius)
        {
            promptImage.SetActive(true);
            if (Input.GetKey("e") && !npcManager.inConversation && !combatManager.inCombat)
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

                    // Sets the player's respawn point to the starting point of the new area
                    player.GetComponent<PlayerStatus>().startingPosition = this.gameObject.GetComponent<AreaTransiton>().playerStartingPosition;

                    playerController.MoveToArea(this.gameObject.GetComponent<AreaTransiton>());
                }
            }
        }
        else { promptImage.SetActive(false); }
    }
}
