#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Cells.RoadCells;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    
    private Size _gridSize = new Size(10,10);
    private CellObjectTypeSO? _itemToBuild;
    private Grid<GridObject> _grid = new Grid<GridObject>(new Size(1,1), 1, new SysVector3(0,0,0), 
    (g, l) => new GridObject(g, l));
    private List<IAdvancable> _advancables = new List<IAdvancable>();
    private List<CellObjectTypeSO>? cellObjectTypeSos;

    #endregion



    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CollectAllCellObjectTypeSosIntoASingleList();
        
        _gridSize = new Size(gridSizeX, gridSizeY);
        _grid = new Grid<GridObject>(_gridSize, gridCellSize, gridOriginPosition.SV3(), 
            (g, l) => new GridObject(g, l));
        
        Location firstGridObjectsLocation = _grid.GetGridObject(0, 0).Location;
        mapFloor!.position = new UniVector3(firstGridObjectsLocation.X, -0.001f, firstGridObjectsLocation.Y);
        mapFloor!.localScale = new UniVector3(_grid.Size.Width, 0, _grid.Size.Height);
        
        BuildFromExistingGrid();
        
        if (showDebug) {
            DebugGridData();
        }
    }

    #region OnEnable - OnDisable

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
    
    #endregion


    private void Update()
    {
        foreach (IAdvancable advancable in _advancables)
        {
            advancable.Tick(Time.deltaTime);
        }

        
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            CycleCellObjectTypesSos(buildingCellObjectTypeSos!);
            InvokeOnSelectedObjectChanged();
        }
        else if(Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            CycleCellObjectTypesSos(twoWayRoadCellObjectTypeSos!);
            InvokeOnSelectedObjectChanged();
        }  
        else if(Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            CycleCellObjectTypesSos(twoWayRoadCornerCellObjectTypeSos!);
            InvokeOnSelectedObjectChanged();
        } 
    }
    
    private void InvokeOnSelectedObjectChanged(){ OnSelectedObjectChanged?.Invoke(this, _itemToBuild!.visual); }

    private void CycleCellObjectTypesSos(List<CellObjectTypeSO> list)
    {
        if (list.Count < 1) throw new Exception("A listában nincsenek elemek.\nRakj bele CellObjectTypeSO-kat");
        if (_itemToBuild is null)
        {
            _itemToBuild = list[0];
            return;
        }
        
        int index = list.IndexOf(_itemToBuild);
        index = (index + 1) % list.Count;
        _itemToBuild = list[index];
        
    }

    private void CollectAllCellObjectTypeSosIntoASingleList()
    {
        cellObjectTypeSos = new List<CellObjectTypeSO>();
        foreach (CellObjectTypeSO cot in buildingCellObjectTypeSos!)
        {
            cellObjectTypeSos.Add(cot);
        }
        foreach (CellObjectTypeSO cot in twoWayRoadCellObjectTypeSos!)
        {
            cellObjectTypeSos.Add(cot);
        }
        foreach (CellObjectTypeSO cot in twoWayRoadCornerCellObjectTypeSos!)
        {
            cellObjectTypeSos.Add(cot);
        }
    }
    
    private void GameInputOnLeftClickPressed(object sender, EventArgs e)
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);
        
        if (_itemToBuild is null) return;
        
        Cell cell = CreateCellByClassName(_itemToBuild, new Location(x,y));
        List<Location> gridPositionList = cell.GetGridPositionList();
        
        if(!CheckIfCanBuild(gridPositionList)){ return; }
        
        Build(cell, gridPositionList);
    }
    
    private void GameInputOnDeleteKeyPressed(object sender, EventArgs e)
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);
        
        GridObject go = _grid.GetGridObject(x, y);
        
        if (go.Model is null) return;
        if (!go.Model.Destroyable) return;

        List<Location> gridPositionList = go.Model!.GetGridPositionList();
        Demolish(go, gridPositionList);
    }

    private void Demolish(GridObject go, List<Location> gridPositionList)
    {
        RemoveFromIAdvancablesList(go);
        DestroyVisualOnModelOrigin(go);
        go.DestroyModel();
        ClearModelFromGridObjects(gridPositionList);
    }

    private void DestroyVisualOnModelOrigin(GridObject go)
    {
        GridObject origin = _grid.GetGridObject(go.Model!.Origin.X, go.Model.Origin.Y);
        Destroy(origin.Visual!.gameObject);
    }

    private void RemoveFromIAdvancablesList(GridObject go)
    {
        if (go.Model is null) return;
        if (go.Model is IAdvancable advancable && _advancables.Contains(advancable))
        {
            Debug.Log("removed form advacneables list");
            _advancables.Remove(advancable);
        }
    }

    private void ClearModelFromGridObjects(List<Location> gridPositionList)
    {
        foreach (Location position in gridPositionList)
        {
            GridObject go = _grid.GetGridObject(position.X, position.Y);
            go.ClearModel();
        }
    }


    public bool CheckIfCanBuild(List<Location> gridPositionList)
    {
        bool canBuild = true;
        foreach (Location position in gridPositionList)
        {
            GridObject? go = _grid.GetGridObject(position.X, position.Y);
            
            if (go is not null && go.CanBuild()) continue;
            
            canBuild = false;
            Debug.Log("Cant build here");
            break;
        }

        return canBuild;
    }

    private void SetModelsValueInGridObjects(Cell cell, List<Location> gridPositionList)
    {
        if (gridPositionList.Count == 0) return;
        
        foreach (Location position in gridPositionList)
        {
            GridObject go = _grid.GetGridObject(position.X, position.Y);
            go.SetModel(cell);
        }
    }

    private void CreateVisualForCellAndLinkToModelOrigin(Cell cell)
    {
        GridObject origin = _grid.GetGridObject(cell.Origin.X, cell.Origin.Y);
        origin.SetVisual(InstantiateCellPrefab(origin));
        LinkVisualToModel(origin);

        
    }

    private void AddCellToIAdvancableListIfIAdvancable(Cell cell)
    {
        if (cell is IAdvancable advancable && !_advancables.Contains(advancable))
        {
            _advancables.Add(advancable);
        }
    }

    private void Build(Cell cell, List<Location> gridPositionList)
    {
        SetModelsValueInGridObjects(cell, gridPositionList);
        CreateVisualForCellAndLinkToModelOrigin(cell);
        AddCellToIAdvancableListIfIAdvancable(cell);
    }

    private Cell CreateCellByClassName(CellObjectTypeSO co, Location location)
    {
        switch (co.buildingType)
        {
            case BuildingTypes.Forest:
                return new Forest(location, growthInterval: 3);
            case BuildingTypes.ProcessingBuildingSteel:
                return new ProcessingBuildingSteel(location, prodInterval: 2f, destroyable: true);
            case BuildingTypes.TwoWayUD:
                return new TwoWayUD(location);
            case BuildingTypes.TwoWayLR:
                return new TwoWayLR(location);
            case BuildingTypes.TwoWayCornerDL:
                return new TwoWayCornerDL(location);
            case BuildingTypes.TwoWayCornerDR:
                return new TwoWayCornerDR(location);
            case BuildingTypes.TwoWayCornerUL:
                return new TwoWayCornerUL(location);
            case BuildingTypes.TwoWayCornerUR:
                return new TwoWayCornerUR(location);
            default:
                throw new Exception("Nincs ilyen class. Fel kell venni, ahogy feljebb van.");
        }
    }
    
    private void LinkVisualToModel(GridObject origin)
    {
        switch (origin.Model!.GetType().Name)
        {
            case nameof(BuildingTypes.Forest):
                ForestVisual fv = origin.Visual!.GetComponent<ForestVisual>();
                fv.Setup(origin.Model as Forest);
                break;
            case nameof(BuildingTypes.ProcessingBuildingSteel):
                break;
        }
    }

    private Transform InstantiateCellPrefab(GridObject go)
    {
        CellObjectTypeSO cellObjectTypeSo = GetCellObjectTypeSoForCell(go.Model!);
        return Instantiate(cellObjectTypeSo.prefab, 
            go.Grid.GetWorldPosition(go.Location.X, go.Location.Y).UVXZ3(),
            UnityEngine.Quaternion.identity, transform);
    }

    private CellObjectTypeSO GetCellObjectTypeSoForCell(Cell value)
    {
        string nameOfCell = value.GetType().Name;
        CellObjectTypeSO co = cellObjectTypeSos!.Find(x => x.buildingType.ToString() == nameOfCell);
        if (co is null)
        {
            Debug.LogError("CellObjectTypeSO doesnt exist.");
            throw new NullReferenceException();
        }
        return co;
    }


    private void BuildFromExistingGrid()
    {
        for (int x = 0; x < _grid.Size.Width; x++) {
            for (int y = 0; y < _grid.Size.Height; y++) {
                GridObject gridObject = _grid.GetGridObject(x, y);
                
                if(gridObject.Model is null) continue;

                if (x != gridObject.Model.Origin.X
                    || y != gridObject.Model.Origin.Y) continue;
                
                CreateVisualForCellAndLinkToModelOrigin(gridObject.Model);
                AddCellToIAdvancableListIfIAdvancable(gridObject.Model);
            }
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

        for (int x = 0; x < _grid.Size.Width; x++) {
            for (int y = 0; y < _grid.Size.Height; y++) {
                debugTextArray[x][y] = Utils.CreateWorldText(_grid.GetGridObject(x,y)?.ToString(), null, _grid.GetWorldPosition(x, y).UVXZ3() + new UniVector3(gridCellSize, 0, gridCellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                Debug.DrawLine(_grid.GetWorldPosition(x, y).UVXZ3(), _grid.GetWorldPosition(x, y + 1).UVXZ3(), Color.white, 100f);
                Debug.DrawLine(_grid.GetWorldPosition(x, y).UVXZ3(), _grid.GetWorldPosition(x + 1, y).UVXZ3(), Color.white, 100f);
            }
        }
        Debug.DrawLine(_grid.GetWorldPosition(0, _grid.Size.Height).UVXZ3(), _grid.GetWorldPosition(_grid.Size.Width, _grid.Size.Height).UVXZ3(), Color.white, 100f);
        Debug.DrawLine(_grid.GetWorldPosition(_grid.Size.Width, 0).UVXZ3(), _grid.GetWorldPosition(_grid.Size.Width, _grid.Size.Height).UVXZ3(), Color.white, 100f);

        _grid.OnGridObjectChanged += (sender, eventArgs) => {
            debugTextArray[eventArgs.location.X][eventArgs.location.Y].text = _grid.GetGridObject(eventArgs.location.X, eventArgs.location.Y).ToString();
        };
    }


    
}
