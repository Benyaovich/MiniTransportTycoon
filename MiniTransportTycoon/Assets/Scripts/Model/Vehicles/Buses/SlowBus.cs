#nullable enable
namespace Model.Vehicles.Buses
{
    public class SlowBus : Bus
    {
        public SlowBus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 4, int maintenanceCost = 6,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService)
        {
        }
    }
}
