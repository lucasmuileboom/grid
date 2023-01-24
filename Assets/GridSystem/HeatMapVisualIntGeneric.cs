using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapVisualIntGeneric : MonoBehaviour
{
    private Grid<HeatMapGridIntObject> grid;
    private Mesh mesh;
    private bool UpdateMesh = false;

    public void SetGrid(Grid<HeatMapGridIntObject> grid) 
    {
        this.grid = grid;

        CreateMesh();

        UpdateHeatMapVisual();
        grid.OnGridValueChanged += GridValueChanged;
    }
    private void CreateMesh() 
    {
        mesh = MeshUtils.CreateEmptyQuadMesh(grid.GetWidth() * grid.GetHeight());
        GetComponent<MeshFilter>().mesh = mesh;
    }
    
    private void GridValueChanged(object sender, Grid<HeatMapGridIntObject>.OnGridChangedEventArgs e) 
    {
        UpdateMesh = true;
    }
    private void LateUpdate() 
    {
        if(UpdateMesh) 
        {
            UpdateMesh = false;
            UpdateHeatMapVisual();
        }
    }
    private void UpdateHeatMapVisual() 
    {
        for(int x = 0; x < grid.GetWidth(); x++) 
        {
            for(int y = 0; y < grid.GetHeight(); y++) 
            {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector2(grid.GetCellSize(), grid.GetCellSize());


                HeatMapGridIntObject heatMapGridIntObject = grid.GetValue(x,y);
                float gridValueNormalized = heatMapGridIntObject.GetValueNormalized();
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0);
                mesh = MeshUtils.AddQuadToMesh(mesh, index, grid.GetWorldPosition(x, y) + (quadSize * 0.5f), quadSize, gridValueUV, gridValueUV);
            }
        }
        Debug.Log("done");
    }
}
