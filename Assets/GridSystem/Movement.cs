using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool isMoving;
    private float movementSpeed;
    private Vector3 targetPosition;

    public delegate void ReachedTarget();
    public event ReachedTarget reachedTarget;

    void Update() 
    {
        if(isMoving) 
        {
            MoveToTarget();
        }        
    }
    public void StartMovingToTarget(Vector3 targetPosition, float movementSpeed) 
    {
        this.targetPosition = targetPosition;
        this.movementSpeed = movementSpeed;
        isMoving = true;
    }
    public void StopMovingToTarget()
    {
        isMoving = false;
    }
    private void MoveToTarget() 
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if(distanceToTarget >= movementSpeed * Time.deltaTime) 
        {
            Vector3 moveDirection = targetPosition - transform.position;
            Move(moveDirection, movementSpeed);
        }
        else 
        {
            isMoving = false;
            reachedTarget?.Invoke();
        }
    }
    public void Move(float x, float y, float z, float movementSpeed) 
    {
        Vector3 moveDirection = new Vector3(x, y, z);
        Move(moveDirection, movementSpeed);
    }
    public void Move(Vector3 moveDirection, float movementSpeed) 
    {
        transform.position = transform.position + ((moveDirection.normalized * movementSpeed) * Time.deltaTime);
    }
}
