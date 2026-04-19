namespace Model.Vehicles.CargoTrucks
{
    public class SteelSemiTruck : CargoTruck
    {
        public SteelSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Steel, float speed = 1, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}