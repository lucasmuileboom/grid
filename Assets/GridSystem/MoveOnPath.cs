using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    private int pathVectorIndex;
    private float speed = 10;
    List<Vector3> VectorPath;

    void Update()
    {
        Move();
    }
    public void Move() 
    {
        if(VectorPath != null) 
        {
            Vector3 targetPosition = VectorPath[pathVectorIndex];
            float distanceToTarget = Vector3.Distance(GetPosition(), targetPosition);
            if(distanceToTarget > speed * Time.deltaTime)
            {
                Vector3 moveDirection = (targetPosition - GetPosition()).normalized;
                transform.position = GetPosition() + ((moveDirection * speed) * Time.deltaTime);
            }
            else 
            {
                pathVectorIndex++;
                if(pathVectorIndex >= VectorPath.Count) 
                {
                    StopMoving();
                }
            }
        }
    }
    private void StopMoving() 
    {
        VectorPath = null;
    }
    public Vector3 GetPosition() 
    {
        return transform.position;
    }
    public void SetTargetPosition(Vector3 targetPosition) 
    {
        pathVectorIndex = 0;
        VectorPath = PathFinding.GetInstance().FindPath(GetPosition(), targetPosition);
        if(VectorPath != null && VectorPath.Count > 1) //kijken of ik VectorPath.Count > 1 wil houden
        {
            VectorPath.RemoveAt(0);
        }
    }
}
