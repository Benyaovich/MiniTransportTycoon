namespace Model.Vehicles.SemiTrucks
{
    public class WoodSemiTruck : SemiTruck
    {
        public WoodSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Wood, float speed = 1, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}