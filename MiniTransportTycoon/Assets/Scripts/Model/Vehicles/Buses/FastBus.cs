#nullable enable
namespace Model.Vehicles.Buses
{
    public class FastBus : Bus
    {
        public FastBus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int maxCarryCapacity = 50,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null, CityService? cityService = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService)
        {
        }
    }
}
