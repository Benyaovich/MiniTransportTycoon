using System;
using UnityEngine;

public class CargoTruck : Vehicle
{
    public CargoTruck(Resource resource, float speed, int maintenanceCost, int purchaseCost, int resourceAmount) 
        : base(resource, speed, maintenanceCost, purchaseCost, resourceAmount) { }

    protected override void LoadResource(ProcessingBuilding pBuilding)
    {
        ResourceAmount = pBuilding.AddRequiredResource(ResourceAmount);
    }

    protected override void UnloadResource(ExtractorBuilding eBuilding)
    {
        throw new NotImplementedException();
    }
}
