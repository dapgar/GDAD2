using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunspotScript : MonoBehaviour
{
    public SunspotManager sunspotManager;
    public GameObject promptImage;
    bool inSunspot = false;

    //private DialogueBox dialogueBoxManager;
    //private bool robTalked = false;

    [Header("Animation")]
    public Animator crossfadeAnim;

    private void Start()
    {
        sunspotManager = GameObject.Find("SunspotManager").GetComponent<SunspotManager>();
        //dialogueBoxManager = GameObject.FindGameObjectWithTag("DialogueBoxManager").GetComponent<DialogueBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inSunspot)
        {
            crossfadeAnim.SetTrigger("Start");
            sunspotManager.Rest();
            Debug.Log("Rested");
        }

        //if (inSunspot && !robTalked)
        //{
        //    dialogueBoxManager.ContinueInteraction(0, 5, 1);
        //    robTalked = true;
        //}
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptImage.SetActive(true);
            inSunspot = true;
        }
        Debug.Log("In Sunspot");
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            promptImage.SetActive(false);
            inSunspot = false;
        }
        Debug.Log("Left Sunpot");
    }
}
