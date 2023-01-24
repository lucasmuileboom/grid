using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private Grid<PathFindingNode> grid;

    public PathFinding(int width, int height) 
    {
        grid = new Grid<PathFindingNode>(width, height, 5f, Vector3.zero, (Grid<PathFindingNode> g, int x, int y) => new PathFindingNode(g, x, y));
    }
    public List<PathFindingNode> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        grid.GetGridXY(startPosition, out int startX, out int startY);
        grid.GetGridXY(endPosition, out int endX, out int endY);
        return FindPath(startX, startY, endX, endY);
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
        Debug.Log("path = null : could not find a path");
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
}
