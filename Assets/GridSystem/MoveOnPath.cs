using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    private int pathVectorIndex;
    private float speed = 10;
    List<Vector3> VectorPath;

    private Movement movement;

    void Start() 
    {
        movement = gameObject.AddComponent<Movement>();
        movement.reachedTarget += NextTarget;
    }
    private void NextTarget() 
    {
        pathVectorIndex++;
        if(pathVectorIndex >= VectorPath.Count)
        {
            VectorPath = null;
            return;
        }
        movement.StartMovingToTarget(VectorPath[pathVectorIndex], speed);
    }
    public void SetTargetPosition(Vector3 targetPosition) 
    {
        pathVectorIndex = 0;
        VectorPath = PathFinding.GetInstance().FindPath(GetPosition(), targetPosition);
        if(VectorPath != null && VectorPath.Count > 1)
        {
            VectorPath.RemoveAt(0);
        }
        movement.StartMovingToTarget(VectorPath[pathVectorIndex], speed);
    }
    public Vector3 GetPosition() 
    {
        return transform.position;
    }
}
