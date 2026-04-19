namespace Model.Vehicles.Buses
{
    public class FastBus : Bus
    {
        public FastBus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int maxCarryCapacity = 50)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}