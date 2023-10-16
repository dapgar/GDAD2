using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int startingLineOfDialogue;

    public string npcName;
    public GameObject npcSprite;
    public DialogueAsset dialogueAsset;

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
}

