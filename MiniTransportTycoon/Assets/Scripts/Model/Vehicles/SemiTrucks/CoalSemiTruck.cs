namespace Model.Vehicles.SemiTrucks
{
    public class CoalSemiTruck : SemiTruck
    {
        public CoalSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Coal, float speed = 1, int maintenanceCost = 50,
            int purchaseCost = 500, int maxCarryCapacity = 25,int resourceAmount = 0, Route? route = null, float maintenanceRemainingTime = 0, float? moveRemainingTime = null)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity,resourceAmount: resourceAmount, route: route, maintenanceRemainingTime: maintenanceRemainingTime, moveRemainingTime: moveRemainingTime)
        {
        }
    }
}