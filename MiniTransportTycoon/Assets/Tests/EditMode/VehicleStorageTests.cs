using System.Numerics;
using Model.Vehicles;
using NUnit.Framework;

public class VehicleStorageTests
{
    private Grid<ModelGridObject> _grid;

    [SetUp]
    public void Init()
    {
        _grid = new Grid<ModelGridObject>(new Size(2, 1), 1, Vector3.Zero,
            (grid, location) => new ModelGridObject(grid, location));
    }

    [Test]
    public void AddVehicleSortsBySpeedAndSkipsDuplicates()
    {
        VehicleStorage storage = new VehicleStorage();
        Bus slowerVehicle = new Bus(_grid);
        CargoTruck fasterVehicle = new CargoTruck(_grid, Resource.Iron, speed: 2.5f);
        int addEventCount = 0;
        Vehicle lastAddedVehicle = null;

        storage.OnVehicleAdd += (_, vehicle) =>
        {
            addEventCount++;
            lastAddedVehicle = vehicle;
        };

        storage.AddVehicle(slowerVehicle);
        storage.AddVehicle(fasterVehicle);
        storage.AddVehicle(slowerVehicle);

        Assert.AreEqual(2, storage.Vehicles.Count);
        Assert.AreSame(fasterVehicle, storage.Vehicles[0]);
        Assert.AreSame(slowerVehicle, storage.Vehicles[1]);
        Assert.AreEqual(2, addEventCount);
        Assert.AreSame(fasterVehicle, lastAddedVehicle);
    }

    [Test]
    public void RemoveVehicleRaisesEventAndIgnoresMissingVehicle()
    {
        VehicleStorage storage = new VehicleStorage();
        Bus vehicle = new Bus(_grid);
        int removeEventCount = 0;
        Vehicle removedVehicle = null;

        storage.OnVehicleRemove += (_, removed) =>
        {
            removeEventCount++;
            removedVehicle = removed;
        };

        storage.AddVehicle(vehicle);

        storage.RemoveVehicle(vehicle);
        storage.RemoveVehicle(vehicle);

        Assert.AreEqual(0, storage.Vehicles.Count);
        Assert.AreEqual(1, removeEventCount);
        Assert.AreSame(vehicle, removedVehicle);
    }

    [Test]
    public void VehiclesReceiveUniquePersistentIdentifiers()
    {
        Bus firstVehicle = new Bus(_grid);
        Bus secondVehicle = new Bus(_grid);

        Assert.AreNotEqual(firstVehicle.Identifier, secondVehicle.Identifier);
    }

    [Test]
    public void RestoredIdentifierAdvancesNextVehicleIdentifier()
    {
        Bus loadedVehicle = new Bus(_grid);
        loadedVehicle.RestoreIdentifier(42);

        Bus nextVehicle = new Bus(_grid);

        Assert.AreEqual(42, loadedVehicle.Identifier);
        Assert.Greater(nextVehicle.Identifier, loadedVehicle.Identifier);
    }
}
