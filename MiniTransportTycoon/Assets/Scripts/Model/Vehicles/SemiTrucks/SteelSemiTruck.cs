#nullable enable
namespace Model.Vehicles.SemiTrucks
{
    public class SteelSemiTruck : SemiTruck
    {
        public SteelSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Steel, float speed = 1, int maintenanceCost = 50,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}