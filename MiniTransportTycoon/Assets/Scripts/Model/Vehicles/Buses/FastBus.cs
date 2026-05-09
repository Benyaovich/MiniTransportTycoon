#nullable enable
namespace Model.Vehicles.Buses
{
    public class FastBus : Bus
    {
        public FastBus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 2, int maintenanceCost = 100,
            int price = 1000, int maxCarryCapacity = 50,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
            : base(grid, resource, speed, maintenanceCost, price, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService)
        {
        }
    }
}
