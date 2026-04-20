namespace Model.Vehicles.SemiTrucks
{
    public class IronSemiTruck : SemiTruck
    {
        public IronSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Iron, float speed = 1, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}