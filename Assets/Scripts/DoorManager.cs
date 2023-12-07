using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] AudioSource doorOpen;
    public GameObject[] doors;
    private bool canInteractWithDoor = true;
    private GameObject player;
    public float detectionRadius = 2.0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        ResetDoor();

        for (int i = 0; i < doors.Length; i++)
        {
            if (canInteractWithDoor && DoorInRange(doors[i], 0))
            {
                ShowPrompt(doors[i]);

                if (Input.GetKey(KeyCode.E))
                {
                    doorOpen.Play();
                    GoThroughDoor(doors[i]);
                }
            }
            else
            {
                HidePrompt(doors[i]);
            }
        }
    }

    /// <summary>
    /// Resets interactions for doors
    /// </summary>
    public void ResetDoor()
    {
        bool closeToDoor = false;

        // checks if the player is within a certain range of the door
        for (int i = 0; i < doors.Length; i++)
        {
            if (DoorInRange(doors[i], 0.25f))
            {
                closeToDoor = true;
            }
        }

        // if the player is not close to door to prevent looping backwards
        if (!closeToDoor)
        {
            canInteractWithDoor = true;
        }
    }

    /// <summary>
    /// Checks if the player is close to any doors
    /// </summary>
    /// <param name="door">Door being checked</param>
    /// <param name="additionalDistance">Additional range if needed</param>
    /// <returns></returns>
    public bool DoorInRange(GameObject door, float additionalDistance)
    {
        float distance = Vector3.Distance(door.transform.position, player.transform.position);

        if (distance <= detectionRadius + additionalDistance)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Shows Button Prompt
    /// </summary>
    /// <param name="door">Door to show prompt</param>
    public void ShowPrompt(GameObject door)
    {
        AreaTransiton doorScript = door.GetComponent<AreaTransiton>();
        if (doorScript != null) 
        {
            doorScript.promptImage.SetActive(true);
        }
    }

    /// <summary>
    /// Hides Button Prompt
    /// </summary>
    /// <param name="door">Door to hide prompt</param>
    public void HidePrompt(GameObject door)
    {
        AreaTransiton doorScript = door.GetComponent<AreaTransiton>();
        if (doorScript != null)
        {
            doorScript.promptImage.SetActive(false);
        }
    }

    /// <summary>
    /// Actually moves the player
    /// </summary>
    /// <param name="door">Door Selected</param>
    public void GoThroughDoor(GameObject door)
    {
        //Debug.Log("TRAVEL THROUGH DOOR");
        AreaTransiton doorScript = door.GetComponent<AreaTransiton>();
        if (doorScript != null)
        {
            doorScript.UseDoor();
        }

        canInteractWithDoor = false;
    }    
}
