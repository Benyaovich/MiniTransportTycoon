using System;
using Model.Interfaces;
using UnityEngine;

public class CargoTruck : Vehicle
{
    public CargoTruck(Resource resource, float speed = 2, int maintenanceCost = 100, int purchaseCost = 1000, int resourceAmount = 50) 
        : base(resource, speed, maintenanceCost, purchaseCost, resourceAmount) { }

    protected override void LoadResource(Facility facility)
    {
        ResourceAmount = facility.GetProducedResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
}
