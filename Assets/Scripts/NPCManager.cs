using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine.UI;
//using UnityEditor.Rendering;

//https://gamedevbeginner.com/dialogue-systems-in-unity/
public class NPCManager : MonoBehaviour
{
    [SerializeField] AudioSource talkingSound;

    public GameObject interactionScreen;

    //[Header("Characters")]
    //public GameObject player;
    private GameObject npc;

    private PlayerStatus playerStatus;
    private NPCScript npcInformation;

    [Header("UI Elements")]
    //public GameObject playerSprite;
    public TextMeshProUGUI npcName;
    private GameObject npcSprite;
    public TextMeshProUGUI dialogueBox;
    public GameObject leftClickIcon; 
    
    [Header("Test Mode")]
    public bool inConversation;

    //public GameObject[] shopIcons;
    //public GameObject[] shopButtons;

    public float defaultCharactersPerSecond = 40;
    bool skipLineTriggered;

    // Start is called before the first frame update
    void Start()
    {
        //playerSprite.SetActive(true);

        interactionScreen.SetActive(false);
    }

    /// <summary>
    /// Use this method to trigger the npc interaction.
    /// </summary>
    /// <param name="newNPC">NPC that is talking</param>
    public void StartInteraction(GameObject newNPC, int dialogueAsset)
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
        leftClickIcon.SetActive(true);
        //foreach (GameObject button in shopButtons)
        //{
        //    button.SetActive(false);
        //}

        StartDialogue(npcInformation.dialogueAssets[dialogueAsset].dialogue, npcInformation.dialogueAssets[dialogueAsset].textLineSpeed,
            npcInformation.StartPosition, npcInformation.npcName);
    }

    /// <summary>
    /// Setups screen for dialogue and starts dialogue with the start position
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="textSpeed">Speed of Text from Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <param name="name">Name of NPC</param>
    public void StartDialogue(string[] dialogue, int[] textSpeed, int startPosition, string name)
    {
        npcName.text = name;
        dialogueBox.gameObject.SetActive(true);
        StopAllCoroutines();
        //StartCoroutine(RunDialogue(dialogue, startPosition));
        StartCoroutine(TypeTextUncapped(dialogue, textSpeed, startPosition));
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
        float charactersPerSecond = defaultCharactersPerSecond;
        skipLineTriggered = false;
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;  // Current text shown
        for (int i = startPosition; i < dialogue.Length; i++) // Iterates through all the lines of the dialogue
        {
            talkingSound.Play();
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

    /// <summary>
    /// Entire line at once
    /// </summary>
    /// <param name="dialogue">Dialogue Asset</param>
    /// <param name="startPosition">Starting line of dialogue</param>
    /// <returns></returns>
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

    /// <summary>
    /// Ends dialogue and disables screen
    /// </summary>
    public void EndDialogue()
    {
        //Debug.Log("ENDED DIALOGUE");
        dialogueBox.gameObject.SetActive(false);
        inConversation = false;
        interactionScreen.SetActive(false);

        npcInformation.Interacted();
    }

    /// <summary>
    /// Skips the current line
    /// </summary>
    public void SkipLine()
    {
        skipLineTriggered = true;
    }
}
