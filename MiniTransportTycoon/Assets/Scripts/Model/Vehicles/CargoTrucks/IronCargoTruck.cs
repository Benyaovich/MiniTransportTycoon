#nullable enable
namespace Model.Vehicles.CargoTrucks
{
    public class IronCargoTruck : CargoTruck
    {
        public IronCargoTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Iron, float speed = 2, int maintenanceCost = 80,
            int purchaseCost = 1000, int maxCarryCapacity = 50,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}