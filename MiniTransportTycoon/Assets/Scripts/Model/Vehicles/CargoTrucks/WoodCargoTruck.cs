namespace Model.Vehicles.CargoTrucks
{
    public class WoodCargoTruck : CargoTruck
    {
        public WoodCargoTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Wood, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int maxCarryCapacity = 50)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}