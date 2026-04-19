using System;
using Model.Interfaces;
using UnityEngine;

public class SemiTruck : Vehicle
{
    public SemiTruck(Grid<ModelGridObject> grid, Resource resource, float speed = 1, int maintenanceCost = 60, int purchaseCost = 500, int maxCarryCapacity = 25) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
}
