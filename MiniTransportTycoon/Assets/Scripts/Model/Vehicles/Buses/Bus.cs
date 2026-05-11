 
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;
using UnityEngine;

public class Bus : Vehicle
{
    public BusStop? VisitedStationThisTurn { get; set; }

    public Bus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 1.8f, int maintenanceCost = 10, int price = 800, int maxCarryCapacity = 40,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null) 
        : base(grid, resource, speed, maintenanceCost, price, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService) { }

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
        BusStop? currentVisitedBusStop = neighbouringCells.FirstOrDefault(x => x is BusStop) as BusStop;
        if (currentVisitedBusStop == null) return false;
        if (VisitedStationThisTurn == currentVisitedBusStop) return false;

        int passengersBefore = ResourceAmount;
        int peopleAtStopBefore = currentVisitedBusStop.NumOfPeople;

        UnloadResource(currentVisitedBusStop);
        int depositedAmount = passengersBefore - ResourceAmount;
        if (depositedAmount > 0)
        {
            PlayerState.Instance.AddMoney(
                GameEconomy.Instance.GetResourcePrice(Resource) * depositedAmount
            );
        }

        int freeCapacity = MaxCapacity - ResourceAmount;
        int peopleToLoad = Math.Min(freeCapacity, peopleAtStopBefore);
        ResourceAmount += currentVisitedBusStop.GetResource(peopleToLoad);

        VisitedStationThisTurn = currentVisitedBusStop;

        return true;
    }
}
