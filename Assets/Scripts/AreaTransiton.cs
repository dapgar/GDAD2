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

    GameObject player;
    public GameObject promptImage;

    [Header("Animation")]
    public Animator crossfadeAnim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        
    }

    public void UseDoor()
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
            player.GetComponent<PlayerStatus>().startingPosition = playerStartingPosition;

            playerController.MoveToArea(this);
        }
    }
}
