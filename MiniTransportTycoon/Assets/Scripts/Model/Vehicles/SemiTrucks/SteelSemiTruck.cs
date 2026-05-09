#nullable enable
namespace Model.Vehicles.SemiTrucks
{
    public class SteelSemiTruck : SemiTruck
    {
        public SteelSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Steel, float speed = 4, int maintenanceCost = 50,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService)
        {
        }
    }
}
