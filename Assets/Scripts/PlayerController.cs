using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    private Vector3 direction = Vector3.right;  // Starting Direction
    private Vector3 velocity = Vector3.zero;
    private Vector3 movementInput;              // Captures movement from input
    public PlayerInput playerInput;

    [Header("Animation")]
    public Animator animator;

    // Hard-Coded limits
    //public float zMax = 4f;
    //public float zMin = -5f;
    //public float xMax = 4.5f;
    //public float xMin = -5f;

    private CombatManager combatManager;
    private NPCManager  npcManager;
    private DialogueBox dialogueBoxManager;
    private void Start()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
        npcManager = GameObject.FindGameObjectWithTag("NPCManager").GetComponent<NPCManager>();
        dialogueBoxManager = GameObject.FindGameObjectWithTag("DialogueBoxManager").GetComponent<DialogueBox>();
    }

    private void Update()
    {
        AnimationChecks();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!combatManager.inCombat && !npcManager.inConversation)
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

        SkipDialogue();

        //if(npcManager.inConversation)
        //{
        //    playerInput.SwitchCurrentActionMap("UI");
        //}
        //else
        //{
        //    playerInput.SwitchCurrentActionMap("Player");
        //}

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

    private void AnimationChecks()
    {
        animator.SetBool("walkLeft", Input.GetKey("a") || Input.GetKey("left"));
        animator.SetBool("walkRight", Input.GetKey("d") || Input.GetKey("right"));
        animator.SetBool("walkUp", Input.GetKey("w") || Input.GetKey("up"));
        animator.SetBool("walkDown", Input.GetKey("s") || Input.GetKey("down"));
    }

    public void SkipDialogue()
    {
        // Will only skip the line in dialogue if the player is currently in a coversation
        if (Mouse.current.leftButton.wasPressedThisFrame && npcManager.inConversation)
        {
            npcManager.SkipLine();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && dialogueBoxManager.inConversation)
        {
            dialogueBoxManager.SkipLine();
        }
    }

    //public void SkipDialogue(InputAction.CallbackContext context)
    //{
    //    // Will only skip the line in dialogue if the player is currently in a coversation
    //    if (Mouse.current.leftButton.wasPressedThisFrame && npcManager.inConversation)
    //    {
    //        npcManager.SkipLine();
    //    }

    //    Debug.Log("BEFORE LEFT CLICK");
    //    if (Mouse.current.leftButton.wasPressedThisFrame && dialogueBoxManager.inConversation)
    //    {
    //        Debug.Log("LEFTCLICK");
    //        dialogueBoxManager.SkipLine();
    //    }
    //}

    public void MoveToArea(AreaTransiton area)
    {
        transform.position = area.playerStartingPosition;
    }
}

