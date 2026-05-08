#nullable enable
using Model.Interfaces;

public class CargoTruck : Vehicle
{
    public CargoTruck(Grid<ModelGridObject> grid, Resource resource, float speed = 2, int maintenanceCost = 80, int purchaseCost = 1000, int maxCarryCapacity = 50,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
}
