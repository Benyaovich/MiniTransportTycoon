using System.Numerics;
using Model.Vehicles.SemiTrucks;
using NUnit.Framework;

public class SemiTruckTests
{
    private Grid<ModelGridObject> _grid;

    [SetUp]
    public void Init()
    {
        _grid = new Grid<ModelGridObject>(new Size(2, 1), 1, Vector3.Zero,
            (grid, location) => new ModelGridObject(grid, location));
    }

    [Test]
    public void SpecializedSemiTruckConstructorsSetExpectedDefaults()
    {
        SemiTruck[] trucks =
        {
            new CoalSemiTruck(_grid),
            new IronSemiTruck(_grid),
            new PaperSemiTruck(_grid),
            new SteelSemiTruck(_grid),
            new WoodSemiTruck(_grid)
        };

        Resource[] expectedResources =
        {
            Resource.Coal,
            Resource.Iron,
            Resource.Paper,
            Resource.Steel,
            Resource.Wood
        };

        for (int i = 0; i < trucks.Length; i++)
        {
            Assert.AreEqual(expectedResources[i], trucks[i].Resource);
            Assert.AreEqual(1f, trucks[i].MoveSpeed);
            Assert.AreEqual(50, trucks[i].MaintenanceCost);
            Assert.AreEqual(500, trucks[i].PurchaseCost);
            Assert.AreEqual(25, trucks[i].MaxCapacity);
            Assert.AreEqual(0, trucks[i].ResourceAmount);
        }
    }
}
