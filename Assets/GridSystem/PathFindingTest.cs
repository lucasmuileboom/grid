using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    [SerializeField] private HeatMapVisualBoolGenericPathFinding heatMapVisualBoolGenericPathFinding;
    [SerializeField] private MoveOnPath moveOnPath;
    PathFinding pathFinding;
    private bool showDebug = true;

    //overal getters en setter toevoegen??

    void Start()
    {
        pathFinding = new PathFinding(10,10);
        heatMapVisualBoolGenericPathFinding.SetGrid(pathFinding.GetGrid());
    }
    void Update() 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            pathFinding.GetGrid().GetValue(mousePosition).SetWalkable(!pathFinding.GetGrid().GetValue(mousePosition).walkable);
        }
        if(Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            pathFinding.GetGrid().GetGridXY(mousePosition, out int x, out int y);

            

            if(showDebug) 
            {
                List<Vector3> path = pathFinding.FindPath(moveOnPath.GetPosition(), pathFinding.GetGrid().GetWorldPosition(x, y));
                if(path != null) 
                {
                    for(int i = 0; i < path.Count - 1; i++) 
                    {
                        Debug.DrawLine(path[i], path[i + 1], Color.red, 10f);
                    } 
                } 
            }
            moveOnPath.SetTargetPosition(mousePosition);
        }
    }
}
