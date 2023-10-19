using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    // If we want to add first time dialogue
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int startingLineOfDialogue;

    public string npcName;
    public GameObject npcSprite;
    public DialogueAsset dialogueAsset;
    //public Vector3 startingPosition;

    [HideInInspector]
    public bool interactedWith = false;

    [HideInInspector]
    public int StartPosition
    {
        get
        {
            if (firstInteraction)
            {
                firstInteraction = false;
                return 0;
            }
            else
            {
                return startingLineOfDialogue;
            }
        }
    }

    public void Interacted()
    {
        interactedWith = true;
    }

    public void ResetInteraction()
    {
        interactedWith = false;
    }

    // For scene reset
    //public void Reset()
    //{
    //    firstInteraction = true;
    //    ResetInteraction();
    //    transform.position = startingPosition;
    //}
}

