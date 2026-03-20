using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;
using Model.Enumerations;

public class CargoTruckTests
{
    CargoTruck _testTruck;
    Route _testRoute;

    [SetUp]
    public void Init()
    {
        _testTruck = new CargoTruck(Resource.Iron, 2f, 5, 50, 100);
        _testRoute = new Route(new List<Location>
        {
            new Location(0, 0),
            new Location(0, 1),
            new Location(0, 2),
            new Location(1, 2),
            new Location(2, 2),
            new Location(2, 1),
            new Location(2, 0),
            new Location(1, 0),
            new Location(0, 0),
        });
        
        _testTruck.Route = _testRoute;
    }
    
    [Test]
    public void CargoTruckConstuctor()
    {
        Assert.AreEqual(_testTruck.Resource, Resource.Iron);
        Assert.AreEqual(_testTruck.MoveSpeed, 2f);
        Assert.AreEqual(_testTruck.MaintenanceCost, 5);
        Assert.AreEqual(_testTruck.PurchaseCost, 50);
        Assert.AreEqual(_testTruck.ResourceAmount, 0);
    }

    [Test]
    public void CargoTruckMovement()
    {
        CargoTruck _crossingTruck = new CargoTruck(Resource.Steel, 2f, 5, 50, 100);
        Route _crossingRoute = new Route(new List<Location>
        {
            new Location(-1, 1),
            new Location(0, 1), 
            new Location(1, 1),
            new Location(2, 1), 
            new Location(3, 1),
            new Location(-1, 1)
        });
        _crossingTruck.Route = _crossingRoute;
        
        Assert.AreEqual(new Location(0, 0), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location(0, 1), _testTruck.Route.NextLocation);
        
        RoadCell road01 = new RoadCell(_testTruck.Route.NextLocation, true,
            new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
        
        _testTruck.NextStep(road01, new List<Cell>());
        
        Assert.AreEqual(new Location(0, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location(-1, 1), _crossingTruck.CurrentLocation);
        //nem tud lepni, mert az elozo kamion ott van
        _crossingTruck.NextStep(road01, new List<Cell>());
        
        Assert.AreEqual(new Location(-1, 1), _crossingTruck.CurrentLocation);
        
        RoadCell road02 = new RoadCell(_testTruck.Route.NextLocation, true,
            new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
        
        _testTruck.NextStep(road02, new List<Cell>());
        
        Assert.AreEqual(new Location(0, 2), _testTruck.CurrentLocation);
        
        _crossingTruck.NextStep(road01, new List<Cell>());
        
        Assert.AreEqual(new Location(0, 1), _crossingTruck.CurrentLocation);
    }

    [Test]
    public void CargoTruckTakesAndPutsResource()
    {
        Facility extract = new ExtractorBuilding(Resource.Iron, 50000, new Location(-1, 0), 50, new Size(1, 1));
        for (int i = 0; i < 50000; i++)
        {
            extract.Tick(10f);
        }
        Facility produce = new ProcessingBuilding(Resource.Steel, Resource.Iron, 50000, 
            new Location(-1, -1), 1f, new Size(1, 1), false, 
            new RateChangeHandler(10, 100, 1, 120f, 50));
        
        RoadCell road01 = new RoadCell(_testTruck.Route.NextLocation, true,
            new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
        
        _testTruck.NextStep(road01, new List<Cell>(){extract});
        
        Assert.AreEqual(new  Location(0, 0), _testTruck.CurrentLocation);
        Assert.AreEqual(100, _testTruck.ResourceAmount);
        
        _testTruck.NextStep(road01, new List<Cell>());
        
        Assert.AreEqual(new  Location(0, 1), _testTruck.CurrentLocation);
        
        RoadCell road02 = new RoadCell(_testTruck.Route.NextLocation, true,
            new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
        
        _testTruck.NextStep(road02, new List<Cell>(){produce});
        
        Assert.AreEqual(new  Location(0, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(0, _testTruck.ResourceAmount);
        
        RoadCell road03 = new RoadCell(_testTruck.Route.NextLocation, true,
            new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
        
        _testTruck.NextStep(road03, new List<Cell>(){produce});
    }
}
