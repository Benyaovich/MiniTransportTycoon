namespace Model.Vehicles.CargoTrucks
{
    public class SteelCargoTruck : CargoTruck
    {
        public SteelCargoTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Steel, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int maxCarryCapacity = 50)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}