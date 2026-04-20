#nullable enable
using System;
using System.Collections.Generic;
using Controller.Building;
using Model;
using Model.Cells.Grid;
using Model.Interfaces;
using UnityEngine;
using UniVector3 = UnityEngine.Vector3;
using SysVector3 = System.Numerics.Vector3;

public class GridManager : MonoBehaviour
{
    #region Static

    public static GridManager? Instance { get; private set; }

    #endregion

    #region Events

    public event EventHandler<Transform?>? OnSelectedObjectChanged;

    #endregion

    #region Public Properties

    public DynamicRoadBuildingManager DynamicRoadBuildingManager => _dynamicRoadBuildingManager;
    public Grid<ModelGridObject> Grid => _grid;
    
    #endregion

    #region Private SerializeFields

    [SerializeField] private bool showDebug = true;
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;
    [SerializeField] private float gridCellSize = 10f;
    [SerializeField] private UniVector3 gridOriginPosition = new UniVector3(0, 0, 0);
    [SerializeField] private Transform? mapFloor;
   

    #endregion

    #region Fields

    private Size _gridSize = new Size(10, 10);

    private Grid<ModelGridObject> _grid = new Grid<ModelGridObject>(
        new Size(1, 1),
        1,
        new SysVector3(0, 0, 0),
        (g, l) => new ModelGridObject(g, l));

    private readonly List<IAdvancable> _advancables = new();
    
    private CellBuildingManager? _cellBuildingManager;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager = null!;
    
    private CityService? _cityService;
    private CellVisualService? _cellVisualService;
    private DynamicRoadVisualService _dynamicRoadVisualService = null!;

    #endregion

    private void Awake()
    {
        Instance = this;
        
        _gridSize = new Size(gridSizeX, gridSizeY);
        _grid = new Grid<ModelGridObject>(
            _gridSize,
            gridCellSize,
            gridOriginPosition.SV3(),
            (g, l) => new ModelGridObject(g, l));
        
        _cityService = new CityService();
        
        _dynamicRoadBuildingManager = new DynamicRoadBuildingManager(_grid);
        
        _cellBuildingManager = new CellBuildingManager(
            _grid,
            _dynamicRoadBuildingManager,
            _cityService,
            _advancables);

        
        _cellVisualService = new CellVisualService(_grid, _cellBuildingManager, transform,
            BuildSelectionManager.Instance.CellLookup);
        _dynamicRoadVisualService = new DynamicRoadVisualService(_grid, _dynamicRoadBuildingManager, transform,
            BuildSelectionManager.Instance.CellLookup);
        
        var firstGridObjectsPosition = _grid.GetWorldPosition(0, 0);
        mapFloor!.position = new UniVector3(firstGridObjectsPosition.X, -0.01f, firstGridObjectsPosition.Y);
        mapFloor!.localScale = new UniVector3(_grid.Size.Width, 0, _grid.Size.Height);
        
        if (showDebug)
        {
            DebugGridData();
        }
    }

    private void Start()
    {
        BuildSelectionManager.Instance.OnSelectedObjectChanged += BuildSelectionManagerOnSelectedObjectChanged;
    }

    #region OnEnable - OnDisable - OnDestroy

    private void OnEnable()
    {
        GameInput.Instance.OnLeftClickPressed += GameInputOnLeftClickPressed;
        GameInput.Instance.OnDeleteKeyPressed += GameInputOnDeleteKeyPressed;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnDeleteKeyPressed -= GameInputOnDeleteKeyPressed;
        GameInput.Instance.OnLeftClickPressed -= GameInputOnLeftClickPressed;
    }

    private void OnDestroy()
    {
        BuildSelectionManager.Instance.OnSelectedObjectChanged -= BuildSelectionManagerOnSelectedObjectChanged;
    }

    #endregion

    private void Update()
    {
        foreach (IAdvancable advancable in _advancables)
        {
            advancable.Tick(GameManager.Instance.DeltaTime);
        }
    }

    

    private void BuildSelectionManagerOnSelectedObjectChanged(object? sender, Transform? selectedVisual)
    {
        OnSelectedObjectChanged?.Invoke(this, selectedVisual);
    }
    
    

    private void GameInputOnLeftClickPressed(object? sender, EventArgs e)
    {
        if (Utils.IsPointerOverBlockingUI()) return;
        if (RouteCreationManager.Instance.InRouteCreation) return;
        if (_cellBuildingManager is null) return;
        if (BuildSelectionManager.Instance.SelectedObjectType is null) return;

        BuildOnCurrentMousePosition();
    }

    public void BuildOnCurrentMousePosition()
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        if (mousePos == Vector3.zero) return;
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        
        if (BuildSelectionManager.Instance!.SelectedObjectType!.CellType == typeof(DynamicRoadCell)){
            _dynamicRoadBuildingManager.TryBuildRoad(new Location(x,y));
            return;
        }
        
        _cellBuildingManager!.TryBuild(
            BuildSelectionManager.Instance.SelectedObjectType.Create(new Location(x, y)));
        
    }

    public void BuildOnLocations(List<Location> locations)
    {
        foreach (Location location in locations)
        {
            if (BuildSelectionManager.Instance.SelectedObjectType == null) return;
            if (BuildSelectionManager.Instance.SelectedObjectType.CellType == typeof(DynamicRoadCell))
            {
                _dynamicRoadBuildingManager.TryBuildRoad(location);
            }
        }
    }

    private void GameInputOnDeleteKeyPressed(object? sender, EventArgs e)
    {
        if (Utils.IsPointerOverBlockingUI()) return;
        if (_cellBuildingManager is null) return;
        if (RouteCreationManager.Instance.InRouteCreation) return;
        
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        ModelGridObject gridObject = _grid.GetGridObject(x, y);
        if (gridObject.Model == null) return;
        
        if (gridObject.Model is IDestroyable { CanDestroy: false }) return;

        if (gridObject.Model is DynamicRoadCell)
        {
            _dynamicRoadBuildingManager.TryDemolishRoad(new Location(x,y));
        }
        else
        {
            _cellBuildingManager.TryDemolish(new Location(x, y));
        }
    }

    public UniVector3 GetMousePosSnappedToGrid()
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);
        return _grid.GetWorldPosition(x, y).UVXZ3();
    }
    private void DebugGridData()
    {
        TextMesh[][] debugTextArray = new TextMesh[_grid.Size.Width][];
        for (int index = 0; index < _grid.Size.Width; index++)
        {
            debugTextArray[index] = new TextMesh[_grid.Size.Height];
        }

        for (int x = 0; x < _grid.Size.Width; x++)
        {
            for (int y = 0; y < _grid.Size.Height; y++)
            {
                debugTextArray[x][y] = Utils.CreateWorldText(
                    _grid.GetGridObject(x, y)?.ToString(),
                    null,
                    _grid.GetWorldPosition(x, y).UVXZ3() + new UniVector3(gridCellSize, 0, gridCellSize) * .5f,
                    20,
                    Color.white,
                    TextAnchor.MiddleCenter,
                    TextAlignment.Center);

                Debug.DrawLine(_grid.GetWorldPosition(x, y).UVXZ3(), _grid.GetWorldPosition(x, y + 1).UVXZ3(), Color.white, 100f);
                Debug.DrawLine(_grid.GetWorldPosition(x, y).UVXZ3(), _grid.GetWorldPosition(x + 1, y).UVXZ3(), Color.white, 100f);
            }
        }

        Debug.DrawLine(_grid.GetWorldPosition(0, _grid.Size.Height).UVXZ3(), _grid.GetWorldPosition(_grid.Size.Width, _grid.Size.Height).UVXZ3(), Color.white, 100f);
        Debug.DrawLine(_grid.GetWorldPosition(_grid.Size.Width, 0).UVXZ3(), _grid.GetWorldPosition(_grid.Size.Width, _grid.Size.Height).UVXZ3(), Color.white, 100f);

        _grid.OnGridObjectChanged += (sender, eventArgs) =>
        {
            debugTextArray[eventArgs.Location.X][eventArgs.Location.Y].text =
                _grid.GetGridObject(eventArgs.Location.X, eventArgs.Location.Y).ToString();
        };
    }

    
}