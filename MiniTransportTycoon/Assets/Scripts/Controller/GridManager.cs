#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UniVector3 = UnityEngine.Vector3;
using SysVector3 = System.Numerics.Vector3;

public class GridManager : MonoBehaviour
{
    [SerializeField] private bool showDebug = true;

    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;
    private Size gridSize = new Size(10,10);
    [SerializeField] private float gridCellSize = 10f;
    [SerializeField] private UniVector3 gridOriginPosition = new UniVector3(0, 0, 0);

    private Grid<GridObject> grid = new Grid<GridObject>(new Size(1,1), 1, new SysVector3(0,0,0), 
    (g, l) => new GridObject(g, l));

    private void Start()
    {
        GameInput.Instance.OnLeftClickPressed += GameInputOnOnLeftClickPressed;
        
        gridSize = new Size(gridSizeX, gridSizeY);
        grid = new Grid<GridObject>(gridSize, gridCellSize, gridOriginPosition.SV3(), 
            (g, l) => new GridObject(g, l));

        if (showDebug) {
            DebugGridData();
        }
    }

    private void OnDisable()
    {
        GameInput.Instance.OnLeftClickPressed -= GameInputOnOnLeftClickPressed;
    }


    private void GameInputOnOnLeftClickPressed(object sender, EventArgs e)
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        grid.GetXY(mousePos.SV3(), out int x, out int y);
            
        // cell to build
        Cell cell = new Cell(new Location(x, y), new Size(3,3));
        List<Location> gridPositionList = cell.GetGridPositionList();
        
        if(!CheckIfCanBuild(gridPositionList)){ return; }
        
        // Build
        Build(cell, gridPositionList);
    }

    public bool CheckIfCanBuild(List<Location> gridPositionList)
    {
        bool canBuild = true;
        foreach (Location position in gridPositionList)
        {
            GridObject? go = grid.GetGridObject(position.X, position.Y);
            if (go is null || !go.CanBuild)
            {
                canBuild = false;
                Debug.Log("Cant build here");
                break;
            }
        }

        return canBuild;
    }

    public void Build(Cell cell, List<Location> gridPositionList)
    {
        foreach (Location position in gridPositionList)
        {
            GridObject go = grid.GetGridObject(position.X, position.Y);
            go.SetValue(cell);
        }
    }
    

    private void DebugGridData()
    {
        TextMesh[][] debugTextArray = new TextMesh[grid.Size.Width][];
        for (int index = 0; index < grid.Size.Width; index++)
        {
            debugTextArray[index] = new TextMesh[grid.Size.Height];
        }

        for (int x = 0; x < grid.Size.Width; x++) {
            for (int y = 0; y < grid.Size.Height; y++) {
                debugTextArray[x][y] = Utils.CreateWorldText(grid.GetGridObject(x,y)?.ToString(), null, grid.GetWorldPosition(x, y).UV3() + new UniVector3(gridCellSize, gridCellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                Debug.DrawLine(grid.GetWorldPosition(x, y).UV3(), grid.GetWorldPosition(x, y + 1).UV3(), Color.white, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y).UV3(), grid.GetWorldPosition(x + 1, y).UV3(), Color.white, 100f);
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(0, grid.Size.Height).UV3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UV3(), Color.white, 100f);
        Debug.DrawLine(grid.GetWorldPosition(grid.Size.Width, 0).UV3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UV3(), Color.white, 100f);

        grid.OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
            debugTextArray[eventArgs.location.X][eventArgs.location.Y].text = grid.GetGridObject(eventArgs.location.X, eventArgs.location.Y).ToString();
        };
    }
    
}
