#nullable enable
using System;
using System.Collections.Generic;
using System.Dynamic;
using Controller.Building;
using Model.Cells.Grid;
using UnityEngine;
using UnityEngine.InputSystem;
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

    public CellBuildingManager CellBuildingManager => _cellBuildingManager!;
    public DynamicRoadBuildingManager DynamicRoadBuildingManager => _dynamicRoadBuildingManager;
    public Grid<ModelGridObject> Grid => _grid;
    public IBuildSelectionManager BuildSelectionManager => _buildSelectionManager!;
    
    #endregion

    #region Private SerializeFields

    [SerializeField] private bool showDebug = true;
    [SerializeField] private int gridSizeX = 10;
    [SerializeField] private int gridSizeY = 10;
    [SerializeField] private float gridCellSize = 10f;
    [SerializeField] private UniVector3 gridOriginPosition = new UniVector3(0, 0, 0);
    [SerializeField] private Transform? mapFloor;
    [SerializeField] private List<CellObjectTypeSO>? buildingCellObjectTypeSos;
    [SerializeField] private List<CellObjectTypeSO>? twoWayRoadCellObjectTypeSos;
    [SerializeField] private List<CellObjectTypeSO>? twoWayRoadCornerCellObjectTypeSos;

    #endregion

    #region Fields

    private Size _gridSize = new Size(10, 10);

    private Grid<ModelGridObject> _grid = new Grid<ModelGridObject>(
        new Size(1, 1),
        1,
        new SysVector3(0, 0, 0),
        (g, l) => new ModelGridObject(g, l));

    private IBuildSelectionManager? _buildSelectionManager;
    
    private readonly List<IAdvancable> _advancables = new();
    private Dictionary<Type, CellObjectTypeSO> _cellLookup = new();
    private List<CellObjectTypeSO> _cellObjectTypeSos = new();
    
    private CellBuildingManager? _cellBuildingManager;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager = null!;
    
    private CityService? _cityService;
    private CellVisualService? _cellVisualService;
    private DynamicRoadVisualService _dynamicRoadVisualService = null!;

    #endregion

    private void Awake()
    {
        Instance = this;
        
        CollectAllCellObjectTypeSosIntoASingleList();
        BuildLookup();
        
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

        
        _cellVisualService = new CellVisualService(_grid, _cellBuildingManager, transform, _cellLookup);
        _dynamicRoadVisualService = new DynamicRoadVisualService(_grid, _dynamicRoadBuildingManager, transform, _cellLookup);
        
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
        BuildSelectionManager.OnSelectedObjectChanged += BuildSelectionManagerOnSelectedObjectChanged;
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
        BuildSelectionManager.OnSelectedObjectChanged -= BuildSelectionManagerOnSelectedObjectChanged;
    }

    #endregion

    private void Update()
    {
        foreach (IAdvancable advancable in _advancables)
        {
            advancable.Tick(Time.deltaTime);
        }
    }

    public void HandleBuildSelectionInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            BuildSelectionManager.CycleSelection(buildingCellObjectTypeSos!);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            BuildSelectionManager.CycleSelection(twoWayRoadCellObjectTypeSos!);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            BuildSelectionManager.CycleSelection(twoWayRoadCornerCellObjectTypeSos!);
        }
    }

    private void BuildSelectionManagerOnSelectedObjectChanged(object? sender, Transform? selectedVisual)
    {
        OnSelectedObjectChanged?.Invoke(this, selectedVisual);
    }

    private void CollectAllCellObjectTypeSosIntoASingleList()
    {
        _cellObjectTypeSos = new List<CellObjectTypeSO>();

        foreach (CellObjectTypeSO cellObjectTypeSo in buildingCellObjectTypeSos!)
        {
            _cellObjectTypeSos.Add(cellObjectTypeSo);
        }

        foreach (CellObjectTypeSO cellObjectTypeSo in twoWayRoadCellObjectTypeSos!)
        {
            _cellObjectTypeSos.Add(cellObjectTypeSo);
        }

        foreach (CellObjectTypeSO cellObjectTypeSo in twoWayRoadCornerCellObjectTypeSos!)
        {
            _cellObjectTypeSos.Add(cellObjectTypeSo);
        }
    }
    
    private void BuildLookup()
    {
        _cellLookup = new Dictionary<Type, CellObjectTypeSO>();
        
        foreach (var so in _cellObjectTypeSos)
        {
            _cellLookup.Add(so.CellType, so);
        }
    }

    private void GameInputOnLeftClickPressed(object? sender, EventArgs e)
    {
        if (Utils.IsPointerOverBlockingUI()) return;
        if (RouteCreationManager.Instance.InRouteCreation) return;
        if (_cellBuildingManager is null) return;
        if (BuildSelectionManager.SelectedObjectType is null) return;

        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        if (BuildSelectionManager.SelectedObjectType.CellType == typeof(DynamicRoadCell))
        {
            _dynamicRoadBuildingManager.TryBuildRoad(new Location(x,y));
        }
        else
        {
            _cellBuildingManager.TryBuild(BuildSelectionManager.SelectedObjectType.Create(new Location(x, y)));
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
    public void SetBuildSelectionManager(IBuildSelectionManager buildSelectionManager)
    {
        _buildSelectionManager = buildSelectionManager;
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