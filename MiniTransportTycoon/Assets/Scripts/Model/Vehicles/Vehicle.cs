#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;
using Model.Interfaces;

public abstract class Vehicle : IAdvancable
{
    public Resource Resource { get; private set;}
    public float MoveSpeed { get; private set; }
    public int MaintenanceCost { get; private set; }
    public int PurchaseCost { get; private set; }
    public int ResourceAmount { get; protected set; }
    private Route? _route;
    public Route? Route => _route;
    public Location? CurrentLocation => _route?.CurrentPosition;
    
    private IGrid<ModelGridObject> _grid;
    public IGrid<ModelGridObject> Grid => _grid;
    private int _maxCapacity;
    protected int MaxCapacity => _maxCapacity;
    private Timer _maintenanceTimer;
    private Timer? _moveTimer;
    
    public event EventHandler<Vehicle>? OnMove;
    public event EventHandler? OnRouteSet;

    protected Vehicle(Grid<ModelGridObject> grid, Resource resource, float speed, int maintenanceCost,
        int purchaseCost, int maxCapacity, float maintenanceInterval = 100)
    {
        _grid = grid;
        Resource = resource;
        MoveSpeed = speed;
        MaintenanceCost = maintenanceCost;
        PurchaseCost = purchaseCost;
        _maxCapacity = maxCapacity;
        ResourceAmount = 0;
        
        _maintenanceTimer = new Timer(maintenanceInterval);
        _maintenanceTimer.OnTimerElapsed += MaintenanceTimerOnTimerElapsed;
    }

    

    private void TryMove(object sender, EventArgs e)
    {
        MoveNext();
    }
    
    public void MoveNext()
    {
        if (_route == null) return;
        RoadCell nextRoadCell = (_grid.GetGridObject(CurrentLocation + _route.CurrentDirection).Model as RoadCell)!;
        if (_route == null || !CanMove(nextRoadCell)) return;

        RoadCell currentRoadCell = (_grid.GetGridObject(CurrentLocation).Model as RoadCell)!;
        currentRoadCell.RemoveVehicle(this);
        _route.Step();
        nextRoadCell!.AddVehicle(this);
        OnMove?.Invoke(this, this);

        List<Cell> neighbouringCells = GetNeighbouringCells();
        DepositToNeighbours(neighbouringCells);
        LoadFromNeighbours(neighbouringCells);
        
    }

    private void DepositToNeighbours(List<Cell> neighbouringCells)
    {
        if (_route == null) return;
        
        foreach (var neighbouringCell in neighbouringCells)
        {
            if (neighbouringCell is not IDepositPoint depositPoint) continue;
            
            if (depositPoint.RequiredResource == Resource && ResourceAmount > 0)
            {
                UnloadResource(depositPoint);
            }
        }
    }

    private void LoadFromNeighbours(List<Cell> neighbouringCells)
    {
        if (_route == null) return;
        
        foreach (var neighbouringCell in neighbouringCells)
        {
            if (neighbouringCell is not IResourceProvider resourceProvider) continue;
            
            if (resourceProvider.ProducedResource == Resource && ResourceAmount < MaxCapacity)
            {
                LoadResource(resourceProvider);
            }
        }
    }

    private bool CanMove(RoadCell road)
    {
        if (RightDirecion(road) && SafeToMove(road) && _route != null)
        {
            return true;
        }
        
        return false;
    }

    public void SetRoute(Route route)
    {
        _route = route;
        _moveTimer = new Timer(MoveSpeed);
        _moveTimer.OnTimerElapsed += TryMove;
        
        RoadCell startingRoadCell = (_grid.GetGridObject(CurrentLocation).Model as RoadCell)!;
        startingRoadCell!.AddVehicle(this);
        OnRouteSet?.Invoke(this, EventArgs.Empty);
        LoadFromNeighbours(GetNeighbouringCells());
    }

    private bool SafeToMove(RoadCell road)
    {
        if (road.IsIntersection)
        {
            if (road.HasLamp ) // ide vissza terni lampa implementalas utan: && road.Lamp.Passable( == false)
            {
                //return false
            }
            
            foreach (var observedVehicle in road.Vehicles)
            {
                if (observedVehicle._route == null)
                    throw new InvalidOperationException("Observed vehicle doesn't have a route.");
                if (!IsInterSectionPassable(observedVehicle._route))
                {
                    return false;
                }
            }
        }
        
        foreach (var observedVehicle in road.Vehicles)
        {
            if (observedVehicle._route!.PreviousDirection == _route!.CurrentDirection)
            {
                return false;
            }
        }
        
        return true;
    }

    private bool IsInterSectionPassable(Route otherVehiclesRoute)
    {
        if (_route == null) return false;
        
        //jobb kanyar
        if (_route.CurrentDirection.TurnRightClockwise() == _route.NextDirection)
        {
            if (otherVehiclesRoute.CurrentDirection == _route.NextDirection)
            {
                return false;
            }
            
            return true;
        } 
        
        //egyenesen
        if (_route.CurrentDirection == _route.NextDirection)
        {
            //szembol jon
            if (otherVehiclesRoute.PreviousDirection.Opposite() == _route.CurrentDirection && 
                (otherVehiclesRoute.CurrentDirection == _route.CurrentDirection.Opposite() || otherVehiclesRoute.CurrentDirection.TurnRightClockwise() == _route.CurrentDirection))
            {
                return true;
            } //balrol jon
            else if (otherVehiclesRoute.PreviousDirection.TurnLeftClockwise() == _route.CurrentDirection && otherVehiclesRoute.CurrentDirection == _route.CurrentDirection.Opposite())
            {
                return true;
            }
        } //bal kanyar
        else if (_route.CurrentDirection.TurnLeftClockwise() == _route.NextDirection)
        {
            if (otherVehiclesRoute.PreviousDirection.TurnLeftClockwise() == _route.CurrentDirection && otherVehiclesRoute.CurrentDirection == _route.CurrentDirection.Opposite())
            {
                return true;
            }
        } //vissza fordulas - ha ures a lista
        
        return false;
    }

    
    private bool RightDirecion(RoadCell road)
    {
        Location locationDiff = road.Origin - CurrentLocation;
    
        if (locationDiff.X == 0 && locationDiff.Y == 0)
            throw new ArgumentException("Ezen a cellán van a jármű.");
    
        Direction dirToRoad = locationDiff.ToDirection();
        return road.Directions.Contains(dirToRoad.Opposite());
    }

    private List<Cell> GetNeighbouringCells()
    {
        List<Cell> neighbours = new();
        if(_grid.GetGridObject(CurrentLocation + Direction.Up)?.Model is Cell up) {neighbours.Add(up);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Down)?.Model is Cell down) {neighbours.Add(down);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Left)?.Model is Cell left) {neighbours.Add(left);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Right)?.Model is Cell right) {neighbours.Add(right);}
        return neighbours;
    }

    public void RemoveFromRoadCell()
    {
        if (CurrentLocation == null) return;
        if (_grid.GetGridObject(CurrentLocation)?.Model is RoadCell roadCell)
        {
            roadCell.RemoveVehicle(this);
        }
    }

    protected abstract void LoadResource(IResourceProvider resourceProvider);

    protected abstract void UnloadResource(IDepositPoint depositPoint);

    private void MaintenanceTimerOnTimerElapsed(object sender, EventArgs e)
    {
        
    }
    public void Tick(float delta)
    {
        if(_moveTimer != null){ _moveTimer.Tick(delta); }
        
        _maintenanceTimer.Tick(delta);
    }
    
}

