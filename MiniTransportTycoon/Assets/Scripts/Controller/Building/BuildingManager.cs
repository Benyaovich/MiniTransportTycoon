#nullable enable
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    private readonly Grid<GridObject> _grid;
    private readonly List<IAdvancable> _advancables;
    private readonly CellVisualService _cellVisualService;

    public BuildingManager(
        Grid<GridObject> grid,
        Transform parentTransform,
        List<IAdvancable> advancables,
        List<CellObjectTypeSO> cellObjectTypeSos)
    {
        _grid = grid;
        _advancables = advancables;
        _cellVisualService = new CellVisualService(grid, parentTransform, cellObjectTypeSos);
    }

    public bool TryBuild(CellObjectTypeSO cellObjectTypeSo, Location location)
    {
        Cell cell = cellObjectTypeSo.Create(location);
        List<Location> gridPositionList = cell.GetGridPositionList();

        if (!CheckIfCanBuild(gridPositionList))
        {
            return false;
        }

        Build(cell, gridPositionList);
        return true;
    }

    public void TryDemolish(Location location)
    {
        GridObject go = _grid.GetGridObject(location.X, location.Y);

        if (go.Model is null) return;
        if (!go.Model.Destroyable) return;

        List<Location> gridPositionList = go.Model.GetGridPositionList();
        Demolish(go, gridPositionList);
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

    public void BuildFromExistingGrid()
    {
        for (int x = 0; x < _grid.Size.Width; x++)
        {
            for (int y = 0; y < _grid.Size.Height; y++)
            {
                GridObject gridObject = _grid.GetGridObject(x, y);

                if (gridObject.Model is null) continue;

                if (x != gridObject.Model.Origin.X || y != gridObject.Model.Origin.Y) continue;

                _cellVisualService.CreateVisualForCell(gridObject.Model);
                AddCellToIAdvancableListIfIAdvancable(gridObject.Model);
            }
        }
    }

    private void Build(Cell cell, List<Location> gridPositionList)
    {
        SetModelsValueInGridObjects(cell, gridPositionList);
        _cellVisualService.CreateVisualForCell(cell);
        AddCellToIAdvancableListIfIAdvancable(cell);
    }

    private void Demolish(GridObject go, List<Location> gridPositionList)
    {
        RemoveFromIAdvancablesList(go);
        _cellVisualService.DestroyVisualOnModelOrigin(go);
        go.DestroyModel();
        ClearModelFromGridObjects(gridPositionList);
    }

    private void RemoveFromIAdvancablesList(GridObject go)
    {
        if (go.Model is null) return;

        if (go.Model is IAdvancable advancable && _advancables.Contains(advancable))
        {
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

    private void SetModelsValueInGridObjects(Cell cell, List<Location> gridPositionList)
    {
        if (gridPositionList.Count == 0) return;

        foreach (Location position in gridPositionList)
        {
            GridObject go = _grid.GetGridObject(position.X, position.Y);
            go.SetModel(cell);
        }
    }

    private void AddCellToIAdvancableListIfIAdvancable(Cell cell)
    {
        if (cell is IAdvancable advancable && !_advancables.Contains(advancable))
        {
            _advancables.Add(advancable);
        }
    }
}