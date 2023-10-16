using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEditor.Rendering;

//https://gamedevbeginner.com/dialogue-systems-in-unity/
public class NPCManager : MonoBehaviour
{
    public GameObject interactionScreen;

    [Header("Characters")]
    public GameObject player;
    public GameObject npc;

    private PlayerStatus playerStatus;
    private NPCScript npcInformation;

    [Header("UI Elements")]
    public GameObject playerSprite;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueBox;
    public GameObject npcSprite;

    [Header("Test Mode")]
    public bool inConversation;

    public GameObject[] shopIcons;
    public GameObject[] shopButtons;

    public float defaultCharactersPerSecond = 40;
    bool skipLineTriggered;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite.SetActive(true);

        interactionScreen.SetActive(false);
    }

    // Use this method to trigger the npc interaction.
    public void StartInteraction(GameObject newNPC)
    {
        npc = newNPC;
        npcInformation = npc.GetComponent<NPCScript>();
        if (npcInformation.npcSprite != null)
        {
            npcSprite = npcInformation.npcSprite;
            npcSprite.SetActive(true);
        }

        inConversation = true;

        interactionScreen.SetActive(true);
        //foreach (GameObject button in shopButtons)
        //{
        //    button.SetActive(false);
        //}

        StartDialogue(npcInformation.dialogueAsset.dialogue, npcInformation.dialogueAsset.textLineSpeed, npcInformation.StartPosition, npcInformation.npcName);
    }

    public void StartDialogue(string[] dialogue, int[] textSpeed, int startPosition, string name)
    {
        npcName.text = name;
        dialogueBox.gameObject.SetActive(true);
        StopAllCoroutines();
        //StartCoroutine(RunDialogue(dialogue, startPosition));
        StartCoroutine(TypeTextUncapped(dialogue, textSpeed, startPosition));
    }

    // Typewriter effect
    IEnumerator TypeTextUncapped(string[] dialogue, int[] textSpeed, int startPosition)
    {
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
                        dialogueBox.text = textBuffer;
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
            dialogueBox.text = dialogue[i];

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

    // Entire line at once
    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        skipLineTriggered = false;
        for (int i = startPosition; i < dialogue.Length; i++)
        {
            dialogueBox.text = dialogue[i];
            while (skipLineTriggered == false)
            {
                // Wait for the current line to be skipped
                yield return null;
            }
            skipLineTriggered = false;
        }
        EndDialogue();
    }

    public void EndDialogue()
    {
        //Debug.Log("ENDED DIALOGUE");
        dialogueBox.gameObject.SetActive(false);
        inConversation = false;
        interactionScreen.SetActive(false);

        npcInformation.Interacted();
    }

    public void SkipLine()
    {
        skipLineTriggered = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
