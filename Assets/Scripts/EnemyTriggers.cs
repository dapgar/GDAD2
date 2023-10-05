using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggers : MonoBehaviour
{
    CombatManager combatManager;
    GameObject player;
    float detectionRadius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        detectNPC();
    }

    private void detectNPC()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (distance <= detectionRadius && !combatManager.inCombat)
        {
            combatManager.StartCombat(this.gameObject);
        }
    }
}
