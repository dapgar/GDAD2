using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllers : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 direction = Vector3.right;  // Starting Direction
    private Vector3 velocity = Vector3.zero;
    private Vector3 movementInput;              // Captures movement from input
    public PlayerInput playerInput;

    // Hard-Coded limits
    //public float zMax = 4f;
    //public float zMin = -5f;
    //public float xMax = 4.5f;
    //public float xMin = -5f;

    private CombatManager combatManager;
    private NPCManager  npcManager;

    private void Start()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!combatManager.inCombat || !npcManager.inConversation)
        {
            direction = movementInput;
            velocity = direction * speed * Time.deltaTime;
            transform.position += velocity;
        }
        else
        {
            velocity = Vector3.zero;
            direction = Vector3.zero;
            movementInput = Vector3.zero;
        }

        if(npcManager.inConversation)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
        }

        // uncomment if you want sprite to change where it is facing
        /*
        if(direction != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        */

        // Limits of area not needed
        //Vector3 currentPosition = transform.position;
        //if (currentPosition.x >= xMax)
        //{
        //    currentPosition.x = xMax;
        //}
        //else if (currentPosition.x <= xMin)
        //{
        //    currentPosition.x = xMin;
        //}

        //if (currentPosition.z >= zMax)
        //{
        //    currentPosition.z = zMax;
        //}
        //else if (currentPosition.z <= zMin)
        //{
        //    currentPosition.z = zMin;
        //}

        //transform.position = currentPosition;
    }

    public void OnMove(InputAction.CallbackContext moveContext)
    {
        Vector2 playerMovementInput = moveContext.ReadValue<Vector2>();
        Vector3 toConvert = new Vector3(playerMovementInput.x, 0, playerMovementInput.y);
        movementInput = IsoVectorConvert(toConvert);
    }

    // Source - https://micha-l-davis.medium.com/isometric-player-movement-in-unity-998d86193b8a
    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }

    public void SkipDialogue(InputAction.CallbackContext context)
    {
        // Will only skip the line in dialogue if the player is currently in a coversation
        if (Mouse.current.leftButton.wasPressedThisFrame && npcManager.inConversation)
        {
            npcManager.SkipLine();
        }
    }
}

