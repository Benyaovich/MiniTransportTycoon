namespace Model.Vehicles.CargoTrucks
{
    public class PaperSemiTruck : CargoTruck
    {
        public PaperSemiTruck(Grid<ModelGridObject> grid, Resource resource = Resource.Paper, float speed = 1, int maintenanceCost = 60,
            int purchaseCost = 500, int maxCarryCapacity = 25)
            : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity)
        {
        }
    }
}