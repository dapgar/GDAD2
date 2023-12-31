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
    public GameObject exclamationPointIcon;
    public GameObject ePrompt;
    public int startingDialogueAsset = 0;
    [HideInInspector]
    public int useDialogueAssetNumber = 0;
    public DialogueAsset[] dialogueAssets;
    public bool needsInteractionScreen = true; // somehow use this to determine using diaogue box or interaction screen
    public bool needsPrompts = true;
    //public Vector3 startingPosition;

    //[HideInInspector]
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

    public void Start()
    {
        useDialogueAssetNumber = startingDialogueAsset;

        if(needsPrompts)
        {
            ePrompt.SetActive(false);
        }
    }

    public void Interacted()
    {
        interactedWith = true;

        if (needsPrompts && exclamationPointIcon != null)
        {
            exclamationPointIcon.SetActive(false);
        }  
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
    public void Reset()
    {
        firstInteraction = true;
        currentLine = 0;
        useDialogueAssetNumber = startingDialogueAsset;
        ResetInteraction();
        if (needsPrompts && exclamationPointIcon != null)
        {
            exclamationPointIcon.SetActive(true);
        }
        
        //transform.position = startingPosition;
    }
}

