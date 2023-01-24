using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingTest : MonoBehaviour
{
    [SerializeField] private HeatMapVisualBoolGenericPathFinding heatMapVisualBoolGenericPathFinding;
    PathFinding pathFinding;
    private bool showDebug = true;

    //toevoegen dat de nodes van kleur verander als je er niet over kan lopen

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

            List<PathFindingNode> path = pathFinding.FindPath(Vector3.one, pathFinding.GetGrid().GetWorldPosition(x, y));

            if(path != null && showDebug) 
            {
                for(int i = 0; i < path.Count - 1; i++)
                {                 
                    Debug.DrawLine(pathFinding.GetGrid().GetWorldPosition(path[i].x, path[i].y) + (Vector3.one * (pathFinding.GetGrid().GetCellSize() * 0.5f)), pathFinding.GetGrid().GetWorldPosition(path[i + 1].x, path[i + 1].y) + (Vector3.one * (pathFinding.GetGrid().GetCellSize() * 0.5f)), Color.red, 10f);
                }   
            }
        }
    }
}
