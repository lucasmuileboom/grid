using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class gridTest : MonoBehaviour
{
    [SerializeField] private HeatMapVisualBoolGeneric heatMapVisualBoolGeneric;
    [SerializeField] private HeatMapVisualIntGeneric heatMapVisualIntGeneric;

    private Grid<HeatMapGridBoolObject> gridHeatmapBool;
    private Grid<HeatMapGridIntObject> gridHeatmapObject;

    [SerializeField] private bool boolGridHeatmap;

    void Start()
    {
        if(boolGridHeatmap) 
        {
            gridHeatmapBool = new Grid<HeatMapGridBoolObject>(20, 20, 5f, new Vector3(-50, -50, 0), (Grid<HeatMapGridBoolObject> g, int x, int y) => new HeatMapGridBoolObject(g, x, y));
            heatMapVisualBoolGeneric.SetGrid(gridHeatmapBool);
        }
        else 
        {
            gridHeatmapObject = new Grid<HeatMapGridIntObject>(20, 20, 5f, new Vector3(-50, -50, 0), (Grid<HeatMapGridIntObject> g, int x, int y) => new HeatMapGridIntObject(g, x, y));
            heatMapVisualIntGeneric.SetGrid(gridHeatmapObject);
        }
    }
    void Update() 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            if(boolGridHeatmap) 
            {
                HeatMapGridBoolObject heatMapGridBoolObject = gridHeatmapBool.GetValue(mousePosition);
                if(heatMapGridBoolObject != null) 
                {
                    heatMapGridBoolObject.Setvalue(!heatMapGridBoolObject.value);
                }
            }
            else 
            {
                HeatMapGridIntObject heatMapGridObject = gridHeatmapObject.GetValue(mousePosition);
                if(heatMapGridObject != null) 
                {
                    heatMapGridObject.Addvalue(5);
                }
            }
        }
        if(Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            mousePosition.z = 0f;

            if(boolGridHeatmap) 
            {
                UnityEngine.Debug.Log(gridHeatmapBool.GetValue(mousePosition));
            }
            else 
            {
                UnityEngine.Debug.Log(gridHeatmapObject.GetValue(mousePosition));
            }
        }
    }
}
public class HeatMapGridIntObject 
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridIntObject> grid;
    private int x;
    private int y;
    public int value;

    public HeatMapGridIntObject(Grid<HeatMapGridIntObject> grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void Addvalue(int Addvalue) 
    {
        value += Addvalue;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }
    public float GetValueNormalized() 
    {
        return (float)value / MAX;
    }
    public override string ToString() 
    {
        return value.ToString();
    }
}
public class HeatMapGridBoolObject
{
    private Grid<HeatMapGridBoolObject> grid;
    private int x;
    private int y;
    public bool value;

    public HeatMapGridBoolObject(Grid<HeatMapGridBoolObject> grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void Setvalue(bool value) 
    {
        this.value = value;
        grid.TriggerGridObjectChanged(x, y);
    }
    public override string ToString() 
    {
        return value.ToString();
    }
}
