using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    // If we want to add first time dialogue
    [SerializeField] bool firstInteraction = true;
    [SerializeField] int startingLineOfDialogue;
    public int currentLine = 0;

    public string npcName;
    public GameObject npcSprite;
    public int useDialogueAssetNumber = 0;
    public DialogueAsset[] dialogueAssets;
    public bool needsInteractionScreen = true; // somehow use this to determine using diaogue box or interaction screen
    //public Vector3 startingPosition;

    
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

    [HideInInspector]
    public int CurrentLine
    {
        get
        {
            {
                return currentLine;
            }
        }
        set
        {
            currentLine = value;
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

    /// <summary>
    /// Switches to the next Dialogue Asset
    /// </summary>
    public void ChangeDialogue()
    {
        if (!firstInteraction && !(useDialogueAssetNumber >= dialogueAssets.Length))
        {
            useDialogueAssetNumber++;
        }
    }
    // For scene reset
    //public void Reset()
    //{
    //    firstInteraction = true;
    //    ResetInteraction();
    //    transform.position = startingPosition;
    //}
}

