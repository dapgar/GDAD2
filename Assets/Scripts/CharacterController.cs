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
    public float zMax = 4f;
    public float zMin = -5f;
    public float xMax = 4.5f;
    public float xMin = -5f;
    //public CollisionManager collisionManager;
    private CombatManager combatManager;

    // Start is called before the first frame update
    void Start()
    {
        combatManager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = movementInput;

        // Velocity is our direction * speed
        velocity = direction * speed * Time.deltaTime;

        // Add our velocity to position
        transform.position += velocity;

        // uncomment if you want sprite to change where it is facing
        /*
        if(direction != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        */

        Vector3 currentPosition = transform.position;
        // Limits of area 
        if (currentPosition.x >= xMax)
        {
            currentPosition.x = xMax;
        }
        else if (currentPosition.x <= xMin)
        {
            currentPosition.x = xMin;
        }

        if (currentPosition.z >= zMax)
        {
            currentPosition.z = zMax;
        }
        else if (currentPosition.z <= zMin)
        {
            currentPosition.z = zMin;
        }
        transform.position = currentPosition;

        detectNPC();
    }

    private void detectNPC()
    {
        // Cycle through enemy data structure
        //for (int i = 0; i < length; i++) { }

        float detectionRadius = 2.0f;

        GameObject enemy = GameObject.Find("Enemy");

        float distance = Vector3.Distance(this.transform.position, enemy.transform.position);

        if(distance <= detectionRadius)
        {
            combatManager.StartCombat();
        }
        

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
}

