using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement movement;
    private float playerSpeed = 10;
    void Start()
    {
        movement = gameObject.AddComponent<Movement>();
    }
    void Update()
    {
        ControleMovement();
    }
    private void ControleMovement() 
    {
        Vector3 movementDirection = new Vector3();
        if(Input.GetKey(KeyCode.D)) 
        {
            movementDirection.x += 1;
        }
        if(Input.GetKey(KeyCode.A)) 
        {
            movementDirection.x += -1;
        }
        if(Input.GetKey(KeyCode.W)) 
        {
            movementDirection.y += 1;
        }
        if(Input.GetKey(KeyCode.S)) 
        {
            movementDirection.y += -1;
        }
        movement.Move(movementDirection, playerSpeed);
    }
}
