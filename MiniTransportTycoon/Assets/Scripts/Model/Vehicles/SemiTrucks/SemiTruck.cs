#nullable enable
using System;
using Model.Interfaces;
using UnityEngine;

public class SemiTruck : Vehicle
{
    public SemiTruck(Grid<ModelGridObject> grid, Resource resource, float speed = 4, int maintenanceCost = 50,
        int purchaseCost = 500, int maxCarryCapacity = 25, int resourceAmount = 0, Route? route = null,
        float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity, resourceAmount: resourceAmount,
            route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime,
            cityService: cityService)
    {
        DepositPerCellInCity = 3;
    }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
}
