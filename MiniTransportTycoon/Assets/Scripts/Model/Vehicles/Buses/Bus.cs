 
#nullable enable
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;

public class Bus : Vehicle
{
    private BusStop? _visitedStation;
    
    public Bus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 1.8f, int maintenanceCost = 100, int purchaseCost = 800, int maxCarryCapacity = 40,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
    
    protected override bool HandleStationAction(List<Cell> neighbouringCells)
    {
        if (_visitedStation is not null && neighbouringCells.Contains(_visitedStation))
        {
            return false;
        }
        
        bool unloaded = TryDepositToNeighbours(neighbouringCells);
        bool loaded = TryLoadFromNeighbours(neighbouringCells);

        if (unloaded || loaded)
        {
            _visitedStation = neighbouringCells[0] as BusStop;
            return true;
        }

        return false;
    }

    protected override List<Cell> GetNeighbouringCells()
    {
        List<Cell> neighbours = new();
        if(_grid.GetGridObject(CurrentLocation + _route!.CurrentDirection.TurnRightClockwise().ToLocation())?.Model is {} up) {neighbours.Add(up);}
        return neighbours;
    }
}