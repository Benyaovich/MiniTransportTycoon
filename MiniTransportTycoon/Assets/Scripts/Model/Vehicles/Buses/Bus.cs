 
using System.Collections.Generic;
using Model.Interfaces;

public class Bus : Vehicle
{
    private bool _visitedAStation = false;
    
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
        if (_visitedAStation)
        {
            _visitedAStation = false;
            return false;
        }
        
        bool unloaded = TryDepositToNeighbours(neighbouringCells);
        bool loaded = TryLoadFromNeighbours(neighbouringCells);
        
        _visitedAStation = unloaded || loaded;

        return _visitedAStation;
    }
    
    protected override 
}