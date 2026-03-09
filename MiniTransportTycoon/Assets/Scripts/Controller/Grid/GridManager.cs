#nullable enable
using System;
using System.Collections.Generic;
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

    private Grid<GridObject> _grid = new Grid<GridObject>(
        new Size(1, 1),
        1,
        new SysVector3(0, 0, 0),
        (g, l) => new GridObject(g, l));

    private readonly List<IAdvancable> _advancables = new();
    private readonly BuildSelectionManager _buildSelectionManager = new();

    private List<CellObjectTypeSO>? _cellObjectTypeSos;
    private BuildingManager? _buildingManager;

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _buildSelectionManager.OnSelectedObjectChanged += BuildSelectionManagerOnSelectedObjectChanged;
        
        CollectAllCellObjectTypeSosIntoASingleList();

        _gridSize = new Size(gridSizeX, gridSizeY);
        _grid = new Grid<GridObject>(
            _gridSize,
            gridCellSize,
            gridOriginPosition.SV3(),
            (g, l) => new GridObject(g, l));

        _buildingManager = new BuildingManager(
            _grid,
            transform,
            _advancables,
            _cellObjectTypeSos!);

        Location firstGridObjectsLocation = _grid.GetGridObject(0, 0).Location;
        mapFloor!.position = new UniVector3(firstGridObjectsLocation.X, -0.001f, firstGridObjectsLocation.Y);
        mapFloor!.localScale = new UniVector3(_grid.Size.Width, 0, _grid.Size.Height);

        _buildingManager.BuildFromExistingGrid();

        if (showDebug)
        {
            DebugGridData();
        }
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
        _buildSelectionManager.OnSelectedObjectChanged -= BuildSelectionManagerOnSelectedObjectChanged;
    }

    #endregion

    private void Update()
    {
        foreach (IAdvancable advancable in _advancables)
        {
            advancable.Tick(Time.deltaTime);
        }

        HandleBuildSelectionInput();
    }

    private void HandleBuildSelectionInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _buildSelectionManager.CycleSelection(buildingCellObjectTypeSos!);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _buildSelectionManager.CycleSelection(twoWayRoadCellObjectTypeSos!);
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _buildSelectionManager.CycleSelection(twoWayRoadCornerCellObjectTypeSos!);
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

    private void GameInputOnLeftClickPressed(object? sender, EventArgs e)
    {
        if (_buildingManager is null) return;
        if (_buildSelectionManager.SelectedObjectType is null) return;

        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        _buildingManager.TryBuild(_buildSelectionManager.SelectedObjectType, new Location(x, y));
    }

    private void GameInputOnDeleteKeyPressed(object? sender, EventArgs e)
    {
        if (_buildingManager is null) return;

        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        _buildingManager.TryDemolish(new Location(x, y));
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
            debugTextArray[eventArgs.location.X][eventArgs.location.Y].text =
                _grid.GetGridObject(eventArgs.location.X, eventArgs.location.Y).ToString();
        };
    }
}