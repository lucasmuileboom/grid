using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private Grid<PathFindingNode> grid;
    private static PathFinding instance;//werkt alleen als je maar 1 pathfinding script gebruikt

    //kijken of ik een boolean wil meegeven die ervoor zorgt dat je niet meer schuin kan lopen

    public PathFinding(int width, int height) 
    {
        instance = this;
        grid = new Grid<PathFindingNode>(width, height, 5f, Vector3.zero, (Grid<PathFindingNode> g, int x, int y) => new PathFindingNode(g, x, y));
    }
    public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        grid.GetGridXY(startPosition, out int startX, out int startY);
        grid.GetGridXY(endPosition, out int endX, out int endY);

        List<PathFindingNode> path = FindPath(startX, startY, endX, endY);

        if(path != null) 
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathFindingNode pathFindingNode in path) 
            {
                vectorPath.Add(new Vector3(pathFindingNode.x, pathFindingNode.y) * grid.GetCellSize() + new Vector3(grid.GetCellSize(), grid.GetCellSize()) * 0.5f);
            }
            return vectorPath;
        }

        return null;
    }
    public List<PathFindingNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathFindingNode startNode = grid.GetValue(startX, startY);
        PathFindingNode endNode = grid.GetValue(endX, endY);

        List<PathFindingNode> openSet = new List<PathFindingNode>();
        List<PathFindingNode> closedSet = new List<PathFindingNode>();

        openSet.Add(startNode);

        for(int x = 0; x < grid.GetWidth(); x++) 
        {
            for(int y = 0; y < grid.GetHeight(); y++) 
            {
                PathFindingNode pathFindingNode = grid.GetValue(x, y);
                pathFindingNode.gCost = int.MaxValue;
                pathFindingNode.calculateFCost();
                pathFindingNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = GetDistanceBetweenNodes(startNode, endNode);
        startNode.calculateFCost();

        while(openSet.Count > 0) 
        {
            PathFindingNode currentNode = GetLowestCostNode(openSet);

            if(currentNode == endNode)
            {
                return retracePath(endNode);
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
           
            foreach(PathFindingNode neighbour in GetNeighbourList(currentNode)) 
            {
                if(!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newGCost = currentNode.gCost + GetDistanceBetweenNodes(currentNode, neighbour);
                if(newGCost < neighbour.gCost) 
                {
                    neighbour.previousNode = currentNode;
                    neighbour.gCost = newGCost;
                    neighbour.hCost = GetDistanceBetweenNodes(neighbour, endNode);
                    neighbour.calculateFCost();
                    if(!openSet.Contains(neighbour)) 
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

        }
        Debug.Log("could not find a path : path = null");
        return null;
    }
    private List<PathFindingNode> retracePath(PathFindingNode endNode) 
    {
        List<PathFindingNode> path = new List<PathFindingNode>();
        path.Add(endNode);

        PathFindingNode currentNode = endNode;

        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private List<PathFindingNode> GetNeighbourList(PathFindingNode currentNode) 
    {
        List<PathFindingNode> result = new List<PathFindingNode>();

        for(int i = -1; i < 2; i++) 
        {
            for(int j = -1; j < 2; j++) 
            {
                if(currentNode.x + i >= 0 && currentNode.x + i < grid.GetWidth() && currentNode.y + j >= 0 && currentNode.y + j < grid.GetHeight()) 
                {
                    result.Add(grid.GetValue(currentNode.x + i, currentNode.y + j));
                }
            }
        }
        return result;
    }
    private int GetDistanceBetweenNodes(PathFindingNode Node1, PathFindingNode Node2) 
    {
        int DistanceX = Mathf.Abs(Node1.x - Node2.x);
        int DistanceY = Mathf.Abs(Node1.y - Node2.y);

        if(DistanceX > DistanceY) 
        {
            return 14 * DistanceY + (10 * (DistanceX - DistanceY));
        }
        return 14 * DistanceX + (10 * (DistanceY - DistanceX));
    }
    private PathFindingNode GetLowestCostNode(List<PathFindingNode> pathNodeList) 
    {
        PathFindingNode currentNode = pathNodeList[0];
        for(int i = 1; i < pathNodeList.Count; i++) 
        {
            if(pathNodeList[i].fCost < currentNode.fCost || pathNodeList[i].fCost == currentNode.fCost && pathNodeList[i].hCost < currentNode.hCost) 
            {
                currentNode = pathNodeList[i];
            }
        }
        return currentNode;
    }

    public Grid<PathFindingNode> GetGrid() 
    {
        return grid;
    }
    public static PathFinding GetInstance() //werkt alleen als er maar 1 pathfinding script word gebruikt
    {
        return instance;
    }
}
