using System;
using System.Collections;
using System.Collections.Specialized;
using UnityEngine;

public class Grid<TGridObject>
{
    public event EventHandler<OnGridChangedEventArgs> OnGridValueChanged;
    public class OnGridChangedEventArgs : EventArgs 
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    /*debug*/
    private TextMesh[,] debugTextArray;
    private bool showDebug = true;
    /*debug*/

    public Grid(int width, int height, float cellSize, Vector3 originPosition , Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray = new TGridObject[width, height];

        for(int x = 0; x < width; x++) 
        {
            for(int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        if(showDebug) /*debug*/
        {
            debugTextArray = new TextMesh[width, height];

            for(int x = 0; x < width; x++) 
            {
                for(int y = 0; y < height; y++) 
                {
                    
                    debugTextArray[x, y] = GridUtils.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + (new Vector3(cellSize, cellSize) / 2), 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
        
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        OnGridValueChanged += (object sender, OnGridChangedEventArgs eventArgs) =>
        {
            debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
        };
        }
    }

    //get positions
    public Vector3 GetWorldPosition(int x, int y) 
    {
        return (new Vector3(x, y) * cellSize) + originPosition;
    }
    public void GetGridXY(Vector3 worldPosition, out int x, out int y) //kijken hoe void en out werkt // wil dit naar een vector 2 omzetten denk ik
    {
        x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
    }

    //set value
    public void SetValue(int x, int y, TGridObject value) 
    {
        if(x >= 0 && y >= 0 && x < width && y < height) 
        {
            gridArray[x, y] = value;
            if(OnGridValueChanged != null) 
            {
                OnGridValueChanged(this, new OnGridChangedEventArgs {x = x, y = y });//ff kijk hoe dit werkt
            }
        }
    }
    public void SetValue(Vector3 worldPosition, TGridObject value) //kijken wat out doet
    {
        GetGridXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    //get value
    public TGridObject GetValue(int x, int y) 
    {
        if(x >= 0 && y >= 0 && x < width && y < height) 
        {
            return gridArray[x, y];
        }
        else 
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetGridXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    public int GetWidth() 
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize() 
    {
        return cellSize;
    }

    //trigger OnGridValueChanged event 
    public void TriggerGridObjectChanged(int x, int y) 
    {
        if(OnGridValueChanged != null) 
        {
            OnGridValueChanged(this, new OnGridChangedEventArgs { x = x, y = y });//ff kijk hoe dit werkt
        }
    }
}
