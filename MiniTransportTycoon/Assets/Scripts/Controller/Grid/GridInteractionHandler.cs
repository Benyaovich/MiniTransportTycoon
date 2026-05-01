#nullable enable
using System;
using System.Collections.Generic;
using Controller.Building;
using Model.Cells.Grid;
using Model.Interfaces;
using UnityEngine;
using UniVector3 = UnityEngine.Vector3;

public class GridInteractionHandler
{
    private readonly Grid<ModelGridObject> _grid;
    private readonly CellBuildingManager _cellBuildingManager;
    private readonly DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private readonly ForestSpreadManager _forestSpreadManager;

    public GridInteractionHandler(
        Grid<ModelGridObject> grid,
        CellBuildingManager cellBuildingManager,
        DynamicRoadBuildingManager dynamicRoadBuildingManager,
        ForestSpreadManager forestSpreadManager)
    {
        _grid = grid;
        _cellBuildingManager = cellBuildingManager;
        _dynamicRoadBuildingManager = dynamicRoadBuildingManager;
        _forestSpreadManager = forestSpreadManager;
    }

    public void Bind(GameInput gameInput)
    {
        gameInput.OnLeftClickPressed += GameInputOnLeftClickPressed;
        gameInput.OnDeleteKeyPressed += GameInputOnDeleteKeyPressed;
    }

    public void Unbind(GameInput gameInput)
    {
        gameInput.OnDeleteKeyPressed -= GameInputOnDeleteKeyPressed;
        gameInput.OnLeftClickPressed -= GameInputOnLeftClickPressed;
    }

    public void BuildOnCurrentMousePosition()
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        if (mousePos == Vector3.zero) return;

        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        if (BuildSelectionManager.Instance.SelectedObjectType!.CellType == typeof(DynamicRoadCell))
        {
            _dynamicRoadBuildingManager.TryBuildRoad(new Location(x, y));
            return;
        }

        Cell cell = BuildSelectionManager.Instance.SelectedObjectType.Create(new Location(x, y));

        if (cell is Forest forest)
        {
            _forestSpreadManager.AddForest(forest);
        }

        _cellBuildingManager.TryBuild(cell);
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

    public UniVector3 GetMousePosSnappedToGrid()
    {
        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);
        return _grid.GetWorldPosition(x, y).UVXZ3();
    }

    private void GameInputOnLeftClickPressed(object? sender, EventArgs e)
    {
        if (Utils.IsPointerOverBlockingUI()) return;
        if (RouteCreationManager.Instance.InRouteCreation) return;
        if (BuildSelectionManager.Instance.SelectedObjectType is null) return;

        BuildOnCurrentMousePosition();
    }

    private void GameInputOnDeleteKeyPressed(object? sender, EventArgs e)
    {
        if (Utils.IsPointerOverBlockingUI()) return;
        if (RouteCreationManager.Instance.InRouteCreation) return;

        UniVector3 mousePos = Utils.GetMouseWorldPosition();
        _grid.GetXY(mousePos.SV3(), out int x, out int y);

        ModelGridObject gridObject = _grid.GetGridObject(x, y);
        if (gridObject.Model == null) return;
        if (gridObject.Model is IDestroyable { CanDestroy: false }) return;

        if (gridObject.Model is Forest forest)
        {
            _forestSpreadManager.RemoveForest(forest);
        }

        if (gridObject.Model is DynamicRoadCell)
        {
            _dynamicRoadBuildingManager.TryDemolishRoad(new Location(x, y));
            return;
        }

        _cellBuildingManager.TryDemolish(new Location(x, y));
    }
}
