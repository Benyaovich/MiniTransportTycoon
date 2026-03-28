#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;

public class BuildingManager : IBuildingManager
{
    public event EventHandler<Location>? OnRoadCellBuilt; 
    public event EventHandler<Location>? OnRoadCellDemolished; 
    public event EventHandler<OnModelChangedEventArgs>? OnModelChanged;
    
    
    private readonly Grid<ModelGridObject> _grid;
    private readonly List<IAdvancable> _advancables;
    private readonly CityService _cityService;
    
    
    public BuildingManager(
        Grid<ModelGridObject> grid,
        CityService cityService,
        List<IAdvancable> advancables)
    {
        _grid = grid;
        _cityService = cityService;
        _advancables = advancables;
    }

    public bool TryBuild(Cell cell)
    {
        
        List<Location> gridPositionList = cell.GetGridPositionList();

        if (cell is not City && !CheckIfCanBuild(gridPositionList))
        {
            return false;
        }
        
        Build(cell, gridPositionList);
        
        InvokeRoadCellBuilt(cell, cell.Origin);
        
        return true;
    }

    public void TryDemolish(Location location)
    {
        ModelGridObject go = _grid.GetGridObject(location.X, location.Y);

        if (go.Model is null) return;
        if (!go.Model.Destroyable) return;

        List<Location> gridPositionList = go.Model.GetGridPositionList();
        InvokeRoadCellDemolished(go.Model, location);
        Demolish(go, gridPositionList);
    }

    public bool CheckIfCanBuild(List<Location> gridPositionList)
    {
        bool canBuild = true;

        foreach (Location position in gridPositionList)
        {
            ModelGridObject? go = _grid.GetGridObject(position.X, position.Y);

            if (go is not null && go.CanBuild()) continue;

            canBuild = false;
            
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
                ModelGridObject gridObject = _grid.GetGridObject(x, y);

                if (gridObject.Model is null) continue;

                if (x != gridObject.Model.Origin.X || y != gridObject.Model.Origin.Y) continue;

                AddCellToIAdvancableListIfIAdvancable(gridObject.Model);
                InvokeOnModelChanged(gridObject.Model, gridObject.Model.Origin);
            }
        }
    }


    private void Build(Cell cell, List<Location> gridPositionList)
    {
        if (cell is City){ BuildCity(cell, gridPositionList); }
        else if (cell is BusStop busStop){ BuildBusStop(busStop, gridPositionList);}
        else if (cell is RoadCell roadCell){ BuildRoadCell(roadCell, gridPositionList);}
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

    private void BuildRoadCell(RoadCell roadCell, List<Location> gridPositionList)
    {
        if (IsRoadVertexPoint(roadCell)){ roadCell.SetIsVertexPoint(true); }
        BuildCell(roadCell, gridPositionList);
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

    private void ClearModelFromGridObjects(List<Location> gridPositionList)
    {
        foreach (Location position in gridPositionList)
        {
            ModelGridObject go = _grid.GetGridObject(position.X, position.Y);
            go.ClearModel();
        }
    }

    private void SetModelsValueInGridObjects(Cell cell, List<Location> gridPositionList)
    {
        if (gridPositionList.Count == 0) return;

        foreach (Location position in gridPositionList)
        {
            ModelGridObject go = _grid.GetGridObject(position.X, position.Y);
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
    
    private bool IsRoadVertexPoint(RoadCell roadCell)
    {
        if (_grid.GetGridObject(roadCell.Origin + Direction.Up)?.Model is IVisitableBuiling) return true;
        if (_grid.GetGridObject(roadCell.Origin + Direction.Down)?.Model is IVisitableBuiling) return true;
        if (_grid.GetGridObject(roadCell.Origin + Direction.Left)?.Model is IVisitableBuiling) return true;
        if (_grid.GetGridObject(roadCell.Origin + Direction.Right)?.Model is IVisitableBuiling) return true;
        return false;
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
                
                ModelGridObject gridObject = _grid.GetGridObject(x, y);
                if (gridObject?.Model is not RoadCell roadCell) continue;

                roadCell.SetIsVertexPoint(true);
                InvokeRoadCellBuilt(roadCell, roadCell.Origin);
            }
        }
    }
    
    private void InvokeOnModelChanged(Cell? cell, Location location)
    {
        OnModelChanged?.Invoke(this, new OnModelChangedEventArgs(cell, location));
    }

    private void InvokeRoadCellBuilt(Cell cell, Location location)
    {
        if (cell is not RoadCell) return;
        
        OnRoadCellBuilt?.Invoke(this, location);
    }
    
    private void InvokeRoadCellDemolished(Cell cell, Location location)
    {
        if (cell is not RoadCell) return;
        OnRoadCellDemolished?.Invoke(this, location);
    }
}