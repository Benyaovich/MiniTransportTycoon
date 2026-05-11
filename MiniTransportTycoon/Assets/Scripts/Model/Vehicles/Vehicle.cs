#nullable enable
using System;
using System.Collections.Generic;
using Model;
using Model.Enumerations;
using Model.Interfaces;

public abstract class Vehicle : IAdvancable, IPurchasable
{
    public Resource Resource { get; private set;}
    public float MoveSpeed { get; private set; }
    public int MaintenanceCost { get; private set; }
    public int Price { get; private set; }
    public int ResourceAmount { get; protected set; }
    public int DepositPerCellInCity { get; protected set; } = 5;
    private Route? _route;
    public Route? Route => _route;
    public Location? CurrentLocation => _route?.CurrentPosition;
    
    protected IGrid<ModelGridObject> _grid;
    public IGrid<ModelGridObject> Grid => _grid;
    protected CityService? _cityService;
    public CityService? CityService => _cityService;
    public int MaxCapacity { get; protected set; }
    public Timer MaintenanceTimer { get; private set; }
    public Timer? MoveTimer { get; private set; }
    
    public event EventHandler<Vehicle>? OnMove;
    public event EventHandler? OnRouteSet;

    protected Vehicle(Grid<ModelGridObject> grid, Resource resource, float speed, int maintenanceCost,
        int price, int maxCapacity, float maintenanceInterval = 100,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
    {
        _grid = grid;
        _cityService = cityService;
        Resource = resource;
        MoveSpeed = speed;
        MaintenanceCost = maintenanceCost;
        Price = price;
        MaxCapacity = maxCapacity;
        ResourceAmount = resourceAmount;
        
        MaintenanceTimer = new Timer(maintenanceInterval);
        _route = route;
        MaintenanceTimer.RemainingTime = maintenanceRemainingTime;
        if (moveRemainingTime != null)
        {
            SetRoute(_route!);
            MoveTimer!.RemainingTime = moveRemainingTime.Value;
        }
        
        
        MaintenanceTimer.OnTimerElapsed += MaintenanceTimerOnTimerElapsed;
    }

    public void SetCityService(CityService cityService)
    {
        _cityService = cityService;
    }

    

    private void TryMove(object sender, EventArgs e)
    {
        MoveNext();
    }
    
    public void MoveNext()
    {
        List<Cell> neighbouringCells = GetNeighbouringCells();
        TryDepositToCurrentCity();
        if (HandleStationAction(neighbouringCells)) return;
        
        if (_route == null) return;
        if (_grid.GetGridObject(_route.NextPosition) is null) return;
        
        ModelGridObject nextCell = _grid.GetGridObject(_route.NextPosition);
        if (nextCell?.Model is not RoadCell nextRoadCell) return;
        if (!CanMove(nextRoadCell)) return;

        Location? oldLocation = CurrentLocation;
        
        _route.Step();
        
        RoadCell currentRoadCell = (_grid.GetGridObject(oldLocation).Model as RoadCell)!;
        currentRoadCell.RemoveVehicle(this);
        nextRoadCell.RemoveWaitingVehicle(this);
        nextRoadCell.AddVehicle(this);
        
        
        if (_grid.GetGridObject(_route.NextPosition).Model is not RoadCell nextNextRoadCell) return;
        nextNextRoadCell.AddWaitingVehicle(this);
        OnMove?.Invoke(this, this);
    }
    
    protected virtual bool HandleStationAction(List<Cell> neighbouringCells)
    {
        if (TryDepositToNeighbours(neighbouringCells)) return true;
        if (TryLoadFromNeighbours(neighbouringCells)) return true;
        return false;
    }

    protected bool TryDepositToNeighbours(List<Cell> neighbouringCells)
    {
        if (_route == null) return false;
        
        foreach (var neighbouringCell in neighbouringCells)
        {
            if (neighbouringCell is not IDepositPoint depositPoint) continue;

            if (this is not Bus && (depositPoint.RequiredResource != Resource || ResourceAmount <= 0)) continue;

            int resourceAmountBefore = ResourceAmount;

            UnloadResource(depositPoint);

            int depositedAmount = resourceAmountBefore - ResourceAmount;

            if (depositedAmount > 0)
            {
                PlayerState.Instance.AddMoney(
                    GameEconomy.Instance.GetResourcePrice(Resource) * depositedAmount
                );

                return true;
            }
        }

        return false;
    }

    private bool TryDepositToCurrentCity()
    {
        if (this is Bus) return false;
        if (_cityService == null || CurrentLocation == null || ResourceAmount <= 0)
        {
            return false;
        }

        if (!_cityService.CityByLocationMap.ContainsKey(CurrentLocation))
        {
            return false;
        }

        int depositedAmount = Math.Min(DepositPerCellInCity, ResourceAmount);
        ResourceAmount -= depositedAmount;

        PlayerState.Instance.AddMoney(
            GameEconomy.Instance.GetResourcePrice(Resource) * depositedAmount
        );

        return true;
    }

    protected bool TryLoadFromNeighbours(List<Cell> neighbouringCells)
    {
        if (_route == null) return false;
        
        foreach (var neighbouringCell in neighbouringCells)
        {
            if (neighbouringCell is not IResourceProvider resourceProvider) continue;
            
            if (resourceProvider.ProducedResource == Resource && ResourceAmount < MaxCapacity)
            {
                LoadResource(resourceProvider);
                return true;
            }
        }

        return false;
    }

    private bool CanMove(RoadCell road)
    {
        if (_route == null) throw new InvalidOperationException("There is no Route given to the vehicle.");

        bool canmove = RightDirecion(road) && road.IsVehicleAllowedToPass(this);
        
        return canmove;
    }

    public void SetRoute(Route route)
    {
        //ha mar volt utja, akkor nem vette ki magat a mezorol, amikor ujat kapott xddd
        if (_route is not null)
        {
            RoadCell currentRoadCell = (_grid.GetGridObject(CurrentLocation).Model as RoadCell)!;
            currentRoadCell.RemoveVehicle(this);
        }
        
        _route = route;
        MoveTimer = new Timer(MoveSpeed);
        MoveTimer.OnTimerElapsed += TryMove;
        
        RoadCell startingRoadCell = (_grid.GetGridObject(CurrentLocation).Model as RoadCell)!;
        startingRoadCell.AddVehicle(this);
        
        RoadCell nextRoadCell = (_grid.GetGridObject(_route.NextPosition).Model as RoadCell)!;
        nextRoadCell.AddWaitingVehicle(this);
        
        OnRouteSet?.Invoke(this, EventArgs.Empty);
    }

    
    private bool RightDirecion(RoadCell road)
    {
        Location locationDiff = road.Origin - CurrentLocation;

        if (locationDiff.X == 0 && locationDiff.Y == 0) return true;
    
        Direction dirToRoad = locationDiff.ToDirection();
        return road.Directions.Contains(dirToRoad.Opposite());
    }

    protected virtual List<Cell> GetNeighbouringCells()
    {
        List<Cell> neighbours = new();
        if(_grid.GetGridObject(CurrentLocation + Direction.Up)?.Model is {} up) {neighbours.Add(up);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Down)?.Model is {} down) {neighbours.Add(down);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Left)?.Model is {} left) {neighbours.Add(left);}
        if(_grid.GetGridObject(CurrentLocation + Direction.Right)?.Model is {} right) {neighbours.Add(right);}
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
        PlayerState.Instance.SpendMoney(MaintenanceCost);
    }
    public void Tick(float delta)
    {
        if(MoveTimer != null){ MoveTimer.Tick(delta); }
        
        MaintenanceTimer.Tick(delta);
    }
    
}

