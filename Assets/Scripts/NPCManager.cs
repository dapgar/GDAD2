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
    public TextMeshProUGUI playerHealth;
    public GameObject playerSprite;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueBox;
    public GameObject npcSprite;

    [Header("Test Mode")]
    public bool autoStart = true;

    public bool inConversation;

    public GameObject[] shopIcons;
    public GameObject[] shopButtons;

    //float charactersPerSecond = 90;
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
        npcSprite = npcInformation.npcSprite;
        npcSprite.SetActive(true);

        inConversation = true;

        interactionScreen.SetActive(true);
        //foreach (GameObject button in shopButtons)
        //{
        //    button.SetActive(false);
        //}

        StartDialogue(npcInformation.dialogueAsset.dialogue, npcInformation.StartPosition, npcInformation.npcName);
    }

    public void StartDialogue(string[] dialogue, int startPosition, string name)
    {
        npcName.text = name;
        dialogueBox.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, startPosition));
    }

    //IEnumerator TypeTextUncapped(string line)
    //{
    //    float timer = 0;
    //    float interval = 1 / charactersPerSecond;
    //    string textBuffer = null;
    //    char[] chars = line.ToCharArray();
    //    int i = 0;
    //    while (i < chars.Length)
    //    {
    //        if (timer < Time.deltaTime)
    //        {
    //            textBuffer += chars[i];
    //            dialogueBox.text = textBuffer;
    //            timer += interval;
    //            i++;
    //        }
    //        else
    //        {
    //            timer -= Time.deltaTime;
    //            yield return null;
    //        }
    //    }
    //}

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
