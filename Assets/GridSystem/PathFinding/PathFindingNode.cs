using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode
{
    private Grid<PathFindingNode> grid;
    public int x;
    public int y;
    //moet een getter toevoegen voor x,y

    public int gCost;
    public int hCost;
    public int fCost;


    public PathFindingNode previousNode;
    public bool walkable = true;

    public PathFindingNode(Grid<PathFindingNode> grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void calculateFCost() 
    {
        fCost = gCost + hCost;
    }
    public void SetWalkable(bool walkable) 
    {
        this.walkable = walkable;
        grid.TriggerGridObjectChanged(x, y);
    }
    public override string ToString() 
    {
        return x + " " + y;
    }
}
