#nullable enable
namespace Model.Vehicles.SemiTrucks
{
    public class CoalSemiTruck : SemiTruck
    {
        public CoalSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Coal, float speed = 1, int maintenanceCost = 50,
            int price = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, price, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}