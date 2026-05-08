 
#nullable enable
using System.Collections.Generic;
using System.Linq;
using Model.Interfaces;
using UnityEngine;

public class Bus : Vehicle
{
    public BusStop? VisitedStationThisTurn { get; set; }
    private int _peopleAtBusStop;

    public Bus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 1.8f, int maintenanceCost = 100, int purchaseCost = 800, int maxCarryCapacity = 40,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null, CityService? cityService = null) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        _peopleAtBusStop = resourceProvider.GetResource(MaxCapacity);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        depositPoint.AddResource(ResourceAmount);
        ResourceAmount = _peopleAtBusStop;
        int overflow = ResourceAmount - MaxCapacity;
        if (overflow > 0)
        {
            ResourceAmount = MaxCapacity;
            depositPoint.AddResource(overflow);
        }
    }
    
    protected override bool HandleStationAction(List<Cell> neighbouringCells)
    {
        BusStop? currentVisitedBusStop = neighbouringCells.FirstOrDefault(x=>x is BusStop) as BusStop;
        if (VisitedStationThisTurn == currentVisitedBusStop){ return false; }
        
        bool loaded = TryLoadFromNeighbours(neighbouringCells);
        bool unloaded = TryDepositToNeighbours(neighbouringCells);


        VisitedStationThisTurn = currentVisitedBusStop;
        return loaded || unloaded;
    }
}
