namespace Model.Vehicles.CargoTrucks
{
    public class CoalCargoTruck : CargoTruck
    {
        public CoalCargoTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Coal, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int maxCarryCapacity = 50)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}