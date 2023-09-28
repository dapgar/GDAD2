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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction = movementInput;

        // Velocity is our direction * speed
        velocity = direction * speed * Time.deltaTime;

        // Add our velocity to position
        transform.position += (Vector3)velocity;

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
    }

    public void OnMove(InputAction.CallbackContext moveContext)
    {
        var value = moveContext.ReadValue<Vector2>();
        movementInput = new Vector3(value.x, 0, value.y);
    }
}
