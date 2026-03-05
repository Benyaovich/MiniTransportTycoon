#nullable enable
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
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

    [SerializeField] private List<CellObjectTypeSO>? cellObjectTypeSos;

    [SerializeField] private Transform mapFloor; 
    private Grid<GridObject> grid = new Grid<GridObject>(new Size(1,1), 1, new SysVector3(0,0,0), 
    (g, l) => new GridObject(g, l));
    

    private void Start()
    {
        GameInput.Instance.OnLeftClickPressed += GameInputOnOnLeftClickPressed;
        
        gridSize = new Size(gridSizeX, gridSizeY);
        grid = new Grid<GridObject>(gridSize, gridCellSize, gridOriginPosition.SV3(), 
            (g, l) => new GridObject(g, l));

        mapFloor.localScale = new UniVector3(grid.Size.Width, 0, grid.Size.Height);
        
        // GenerateCellPrefabsForExistingGrid();
        
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
        // Cell cell = new Cell(new Location(x, y), new Size(3,3));
        Cell cell = new ProcessingBuilding(Resource.Steel, Resource.Iron, 100, new Location(x, y), size: new Size(2,2));
        List<Location> gridPositionList = cell.GetGridPositionList();
        
        if(!CheckIfCanBuild(gridPositionList)){ return; }
        
        Build(cell, gridPositionList);
    }

    public bool CheckIfCanBuild(List<Location> gridPositionList)
    {
        bool canBuild = true;
        foreach (Location position in gridPositionList)
        {
            GridObject? go = grid.GetGridObject(position.X, position.Y);
            if (go is null || !go.CanBuild())
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
        if (gridPositionList.Count == 0) return;
        
        foreach (Location position in gridPositionList)
        {
            GridObject go = grid.GetGridObject(position.X, position.Y);
            go.SetValue(cell);
        }
        
        GridObject origin = grid.GetGridObject(cell.Origin.X, cell.Origin.Y);
        origin.SetCellPrefab(InstantiateCellPrefab(origin));
    }

    private Transform? InstantiateCellPrefab(GridObject go)
    {
        Transform prefab = GetCellPrefab(go.Value);
        return Instantiate(prefab, 
            go.Grid.GetWorldPosition(go.Location.X, go.Location.Y).UVXZ3(),
            UnityEngine.Quaternion.identity, transform);
    }

    private Transform GetCellPrefab(Cell value)
    {
        string nameOfCell = value.GetType().Name;
        CellObjectTypeSO co = cellObjectTypeSos!.Find(x => x.nameOfCellType == nameOfCell);
        if (co is null)
        {
            Debug.LogError("CellObjectTypeSO doesnt exist or nameOfCellType is spelled wrong.");
            throw new NullReferenceException();
        }
        return co.prefab;
    }


    private void GenerateCellPrefabsForExistingGrid()
    {
        for (int x = 0; x < grid.Size.Width; x++) {
            for (int y = 0; y < grid.Size.Height; y++) {
                GridObject gridObject = grid.GetGridObject(x, y);
                if (x == gridObject.Value.Origin.X
                    && y == gridObject.Value.Origin.Y)
                {
                    gridObject.SetCellPrefab(InstantiateCellPrefab(gridObject));
                }
            }
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
                debugTextArray[x][y] = Utils.CreateWorldText(grid.GetGridObject(x,y)?.ToString(), null, grid.GetWorldPosition(x, y).UVXZ3() + new UniVector3(gridCellSize, 0, gridCellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                Debug.DrawLine(grid.GetWorldPosition(x, y).UVXZ3(), grid.GetWorldPosition(x, y + 1).UVXZ3(), Color.white, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y).UVXZ3(), grid.GetWorldPosition(x + 1, y).UVXZ3(), Color.white, 100f);
            }
        }
        Debug.DrawLine(grid.GetWorldPosition(0, grid.Size.Height).UVXZ3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UVXZ3(), Color.white, 100f);
        Debug.DrawLine(grid.GetWorldPosition(grid.Size.Width, 0).UVXZ3(), grid.GetWorldPosition(grid.Size.Width, grid.Size.Height).UVXZ3(), Color.white, 100f);

        grid.OnGridObjectChanged += (sender, eventArgs) => {
            debugTextArray[eventArgs.location.X][eventArgs.location.Y].text = grid.GetGridObject(eventArgs.location.X, eventArgs.location.Y).ToString();
        };
    }
    
}
