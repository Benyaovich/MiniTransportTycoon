#nullable enable
namespace Model.Vehicles.SemiTrucks
{
    public class WoodSemiTruck : SemiTruck
    {
        public WoodSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Wood, float speed = 4, int maintenanceCost = 5,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 100, float? moveRemainingTime = null, CityService? cityService = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime, cityService: cityService)
        {
        }
    }
}
