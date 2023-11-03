using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunspotScript : MonoBehaviour
{
    public SunspotManager sunspotManager;
    public GameObject promptText;
    bool inSunspot = false;

    [Header("Animation")]
    public Animator crossfadeAnim;

    private void Start()
    {
        sunspotManager = GameObject.Find("SunspotManager").GetComponent<SunspotManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inSunspot)
        {
            promptText.SetActive(false);
            crossfadeAnim.SetTrigger("Start");
            sunspotManager.Rest();
            Debug.Log("Rested");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSunspot = true;
        }
        Debug.Log("In Sunspot");
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSunspot = false;
        }
        Debug.Log("Left Sunpot");
    }
}
