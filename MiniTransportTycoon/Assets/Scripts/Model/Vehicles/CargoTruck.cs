using System;
using Model.Interfaces;
using UnityEngine;

public class CargoTruck : Vehicle
{
    public CargoTruck(Grid<ModelGridObject> grid, Resource resource, float speed = 2, int maintenanceCost = 100, int purchaseCost = 1000, int resourceAmount = 50) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, resourceAmount) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
}
