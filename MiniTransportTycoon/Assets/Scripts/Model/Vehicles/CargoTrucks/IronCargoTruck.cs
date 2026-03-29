namespace Model.Vehicles.CargoTrucks
{
    public class IronCargoTruck : CargoTruck
    {
        public IronCargoTruck(Resource resource = Resource.Iron, float speed = 2, int maintenanceCost = 100,
            int purchaseCost = 1000, int resourceAmount = 50)
            : base(resource, speed, maintenanceCost, purchaseCost, resourceAmount)
        {
        }
    }
}