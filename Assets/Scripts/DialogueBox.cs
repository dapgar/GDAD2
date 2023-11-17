using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public GameObject dialogueBoxScreen;
    public GameObject[] npcs;
    private NPCScript npcInformation;
    private int currentNPC;

    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueBoxText;
    private GameObject npcSprite; //make it use other characters

    public float defaultCharactersPerSecond = 40;
    public bool skipLineTriggered;
    public bool inConversation;

    CombatManager combatManager;

    // allow it to be called to start dialogue from other scripts
    // needs to be called in free roam and also during combat

    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxScreen.SetActive(false);
        combatManager = GameObject.FindObjectOfType<CombatManager>();
    }
    private void Update()
    {
        //if (!combatManager.inCombat)
        //{
        //    EndDialogue();
        //}
    }

    private void ShowScreen()
    {
        npcNameText.text = npcInformation.npcName;
        dialogueBoxText.gameObject.SetActive(true);
        dialogueBoxScreen.SetActive(true);
    }

    /// <summary>
    /// Use this method to trigger the npc interaction.
    /// </summary>
    /// <param name="currentNPCTalking">Index of NPC in npc manager</param>
    public void StartInteraction(int currentNPCTalking, int dialogueAsset)
    {
        currentNPC = currentNPCTalking; 
        GameObject npc = npcs[currentNPC];
        npcInformation = npc.GetComponent<NPCScript>();
        if (npcInformation.npcSprite != null)
        {
            npcSprite = npcInformation.npcSprite;
            npcSprite.SetActive(true);
        }

        inConversation = true;

        StartDialogue(npcInformation.dialogueAssets[dialogueAsset].dialogue, 
            npcInformation.dialogueAssets[dialogueAsset].textLineSpeed,
            npcInformation.StartPosition);
    }

    /// <summary>
    /// Used to start an interaction or continue it from a previous point. Set startingLine to a negative value to start from current line
    /// </summary>
    /// <param name="currentNPCTalking">Index of NPC in npc manager</param>
    /// <param name="startingLine">Starting Line of Dialogue (STARTS FROM 0)</param>
    /// <param name="linesSpoken">Number of Lines Spoken</param>
    public void ContinueInteraction(int currentNPCTalking, int startingLine, int linesSpoken)
    {
        inConversation = true;

        currentNPC = currentNPCTalking;
        GameObject npc = npcs[currentNPC];
        npcInformation = npc.GetComponent<NPCScript>();
        //npcInformation.Interacted();
        if (npcInformation.npcSprite != null)
        {
            npcSprite = npcInformation.npcSprite;
            npcSprite.SetActive(true);
        }

        

        // If starting line is negative, begin from the current line
        if (startingLine < 0)
        {
            // If it will not try to say more lines than currently stored
            if(!(npcInformation.CurrentLine + linesSpoken > npcInformation.dialogueAssets[npcInformation.useDialogueAssetNumber].dialogue.Length))
            {
                StartDialogue(npcInformation.dialogueAssets[npcInformation.useDialogueAssetNumber].dialogue,
                npcInformation.dialogueAssets[npcInformation.useDialogueAssetNumber].textLineSpeed,
                npcInformation.CurrentLine, npcInformation.CurrentLine + linesSpoken);
            }
        }
        else
        {
            StartDialogue(npcInformation.dialogueAssets[npcInformation.useDialogueAssetNumber].dialogue, 
                npcInformation.dialogueAssets[npcInformation.useDialogueAssetNumber].textLineSpeed,
                startingLine, startingLine + linesSpoken);
        }
    }

    /// <summary>
    /// Setups screen for dialogue and starts dialogue with the start position
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="textSpeed">Speed of Text from Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    public void StartDialogue(string[] dialogue, int[] textSpeed, int startPosition)
    {
        //dialogueBoxScreen.gameObject.SetActive(true);
        StopAllCoroutines();
        //StartCoroutine(RunDialogue(dialogue, startPosition));
        StartCoroutine(TypeTextUncapped(dialogue, textSpeed, startPosition));
    }
    /// <summary>
    /// Setups screen for dialogue and starts dialogue with the start and end position
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="textSpeed">Speed of Text from Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <param name="endingPosition">Number of Lines Spoken</param>
    /// <param name="name">Name of NPC</param>
    public void StartDialogue(string[] dialogue, int[] textSpeed, int startPosition, int endingPosition)
    {
        //dialogueBoxScreen.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(TypeTextUncapped(dialogue, textSpeed, startPosition, endingPosition));
    }

    /// <summary>
    /// Shows the entire line with a typewritter effect
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="textSpeed">Speed of Text from Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <returns></returns>
    IEnumerator TypeTextUncapped(string[] dialogue, int[] textSpeed, int startPosition)
    {
        ShowScreen();
        float charactersPerSecond = defaultCharactersPerSecond;
        skipLineTriggered = false;
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;  // Current text shown
        for (int i = startPosition; i < dialogue.Length; i++) // Iterates through all the lines of the dialogue
        {
            textBuffer = "";
            char[] chars = dialogue[i].ToCharArray();
            if (textSpeed[i] > 0)
            {
                charactersPerSecond = textSpeed[i];
            }
            else
            {
                charactersPerSecond = defaultCharactersPerSecond;
            }
            interval = 1 / charactersPerSecond;
            //Debug.Log("Text Speed from Dialogue: " + textSpeed[i]);
            //Debug.Log("Current Dialogue Speed: " + charactersPerSecond);
            int currentCharacter = 0;
            while (currentCharacter < chars.Length && !skipLineTriggered)
            {
                if (!skipLineTriggered)
                {
                    if (timer < Time.deltaTime)
                    {
                        textBuffer += chars[currentCharacter];
                        dialogueBoxText.text = textBuffer;
                        timer += interval;
                        currentCharacter++;
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                        yield return null;
                    }
                }
            }

            // This first resets the skipline trigger so the first press shows the full
            // line rather than waiting
            skipLineTriggered = false;
            dialogueBoxText.text = dialogue[i];

            // Second click will actually progress to the next line
            while (skipLineTriggered == false)
            {
                // Wait for the current line to be skipped
                yield return null;
            }
            skipLineTriggered = false;
        }
        EndDialogue();
    }

    /// <summary>
    /// Only shows lines within the range, Mainly for continuing an interaction from a previous point
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="textSpeed">Speed of Text from Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <param name="endingPosition">Number of Lines Spoken</param>
    /// <returns></returns>
    IEnumerator TypeTextUncapped(string[] dialogue, int[] textSpeed, int startPosition, int endingPosition)
    {
        ShowScreen();
        float charactersPerSecond = defaultCharactersPerSecond;
        skipLineTriggered = false;
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;  // Current text shown
        for (int i = startPosition; i < endingPosition; i++) // Iterates through all the lines of the dialogue
        {
            textBuffer = "";
            char[] chars = { ' ' };

            // Will not show anything if values go out of bound
            if (i < dialogue.Length)
            {
                chars = dialogue[i].ToCharArray();

                if (textSpeed[i] > 0)
                {
                    charactersPerSecond = textSpeed[i];
                }
                else
                {
                    charactersPerSecond = defaultCharactersPerSecond;
                }

                interval = 1 / charactersPerSecond;
                int currentCharacter = 0;
                while (currentCharacter < chars.Length && !skipLineTriggered)
                {
                    if (!skipLineTriggered)
                    {
                        if (timer < Time.deltaTime)
                        {
                            textBuffer += chars[currentCharacter];
                            dialogueBoxText.text = textBuffer;
                            timer += interval;
                            currentCharacter++;
                        }
                        else
                        {
                            timer -= Time.deltaTime;
                            yield return null;
                        }
                    }
                }

                // This first resets the skipline trigger so the first press shows the full
                // line rather than waiting
                skipLineTriggered = false;
                dialogueBoxText.text = dialogue[i];

                //Debug.Log("BUFFER " + textBuffer);
                //Debug.Log("SHOWN TEXT " +dialogueBoxText.text);

                // Second click will actually progress to the next line
                while (skipLineTriggered == false)
                {
                    // Wait for the current line to be skipped
                    yield return null;
                }
                skipLineTriggered = false;
            }
            else
            {
                yield return null;
            }
            
        }
        npcInformation.CurrentLine = endingPosition;
        EndDialogue();
    }

    /// <summary>
    /// Entire line at once
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <returns></returns>
    //IEnumerator RunDialogue(string[] dialogue, int startPosition)
    //{
    //    ShowScreen();
    //    skipLineTriggered = false;
    //    for (int i = startPosition; i < dialogue.Length; i++)
    //    {
    //        dialogueBoxText.text = dialogue[i];
    //        while (skipLineTriggered == false)
    //        {
    //            // Wait for the current line to be skipped
    //            yield return null;
    //        }
    //        skipLineTriggered = false;
    //    }
    //    EndDialogue();
    //}

    /// <summary>
    /// Ends dialogue and disables screen
    /// </summary>
    public void EndDialogue()
    {
        Debug.Log("ENDED DIALOGUE");
        dialogueBoxText.gameObject.SetActive(false);
        inConversation = false;
        dialogueBoxScreen.SetActive(false);

        if (npcInformation != null)
        {
            npcInformation.Interacted();
        }
    }

    /// <summary>
    /// Skips the current line
    /// </summary>
    public void SkipLine()
    {
        //Debug.Log("SKIPPED LINE");
        skipLineTriggered = true;
    }
}
