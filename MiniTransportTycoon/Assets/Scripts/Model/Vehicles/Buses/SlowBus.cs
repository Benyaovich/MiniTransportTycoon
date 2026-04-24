namespace Model.Vehicles.Buses
{
    public class SlowBus : Bus
    {
        public SlowBus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 4, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}