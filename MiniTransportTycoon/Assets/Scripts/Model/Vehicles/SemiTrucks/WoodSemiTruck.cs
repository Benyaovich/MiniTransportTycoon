#nullable enable
namespace Model.Vehicles.SemiTrucks
{
    public class WoodSemiTruck : SemiTruck
    {
        public WoodSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Wood, float speed = 1, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}