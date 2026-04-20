#nullable enable
using System;
using System.Collections.Generic;
using Model.Cells.Grid;
using UnityEngine;

public class CellBuildingManager : BuildingManagerBase
{
    public event EventHandler<OnModelChangedEventArgs>? OnCellChanged;
    
    
    private readonly List<IAdvancable> _advancables;
    private readonly CityService _cityService;
    private readonly DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    
    
    public CellBuildingManager(
        IGrid<ModelGridObject> grid,
        DynamicRoadBuildingManager dynamicRoadBuildingManager,
        CityService cityService,
        List<IAdvancable> advancables) : base(grid)
    {
        _cityService = cityService;
        _advancables = advancables;
        _dynamicRoadBuildingManager = dynamicRoadBuildingManager;
    }

    
    public bool TryBuild(Cell cell)
    {
        List<Location> gridPositionList = cell.GetGridPositionList();
        if (cell is not City && !CheckIfCanBuild(gridPositionList))
        {
            return false;
        }
        
        Build(cell, gridPositionList);
        
        return true;
    }

    public void TryDemolish(Location location)
    {
        ModelGridObject go = Grid.GetGridObject(location.X, location.Y);

        if (go.Model is null) return;
        if (!go.Model.Destroyable) return;

        List<Location> gridPositionList = go.Model.GetGridPositionList();
        Demolish(go, gridPositionList);
    }
    
    
    
    private void Build(Cell cell, List<Location> gridPositionList)
    {
        if (cell is City){ BuildCity(cell, gridPositionList); }
        else if (cell is BusStop busStop){ BuildBusStop(busStop, gridPositionList);}
        else{ BuildCell(cell, gridPositionList); }
        
        if (cell is IVisitableBuiling)
        {
            ConvertRoadsToVertexPoints(cell);
        }
    }

    

    #region BuildMethods

    private void BuildCell(Cell cell, List<Location> gridPositionList)
    {
        SetModelsValueInGridObjects(cell, gridPositionList);
        AddCellToIAdvancableListIfIAdvancable(cell);
        InvokeOnModelChanged(cell, cell.Origin);
    }
    
    private void BuildCity(Cell cell, List<Location> gridPositionList)
    {
        _cityService.AddCity(cell, gridPositionList);
        AddCellToIAdvancableListIfIAdvancable(cell);
    }

    private void BuildBusStop(BusStop busStop, List<Location> gridPositionList)
    {
        busStop.SetCityService(_cityService);
        busStop.LocateAndSetCity();
        BuildCell(busStop, gridPositionList);
    }

    
    
    #endregion

    private void Demolish(ModelGridObject go, List<Location> gridPositionList)
    {
        Location origin = go.Model!.Origin;
        RemoveFromIAdvancablesList(go);
        go.ClearModel();
        ClearModelFromGridObjects(gridPositionList);
        InvokeOnModelChanged(go.Model, origin);
    }

    private void RemoveFromIAdvancablesList(ModelGridObject go)
    {
        if (go.Model is IAdvancable advancable && _advancables.Contains(advancable))
        {
            _advancables.Remove(advancable);
        }
    }

    private void AddCellToIAdvancableListIfIAdvancable(Cell cell)
    {
        if (cell is IAdvancable advancable && !_advancables.Contains(advancable))
        {
            _advancables.Add(advancable);
        }
    }
    
    
    private void ConvertRoadsToVertexPoints(Cell cell)
    {
        int yFrom = cell.Origin.Y - 1;
        int yTo = cell.Origin.Y + cell.Size.Height;
        int xFrom = cell.Origin.X - 1;
        int xTo = cell.Origin.X + cell.Size.Width;
        
        for (int y = yFrom; y <= yTo; y++)
        {
            for (int x = xFrom; x <= xTo; x++)
            {
                if((x == xFrom && y == yFrom) || (x == xTo && y == yTo) ||
                   (x == xFrom && y == yTo) || (x == xTo && y == yFrom)) continue;
                ModelGridObject gridObject = Grid.GetGridObject(x, y);
                if (gridObject?.Model is not RoadCell roadCell) continue;
                
                roadCell.SetIsVertexPoint(true);
                if(roadCell is not DynamicRoadCell dynamicRoadCell) continue;
                _dynamicRoadBuildingManager.RefreshRoad(dynamicRoadCell);
            }
        }
    }
    
    private void InvokeOnModelChanged(Cell? cell, Location location)
    {
        OnCellChanged?.Invoke(this, new OnModelChangedEventArgs(cell, location));
    }

    
}