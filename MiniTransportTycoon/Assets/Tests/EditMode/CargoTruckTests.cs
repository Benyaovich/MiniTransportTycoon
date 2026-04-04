using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;
using System.Numerics;
using Model.Cells.RoadCells;
using Model.Enumerations;

public class CargoTruckTests
{
    private CargoTruck _testTruck;
    private CargoTruck _testTruck2;
    private Route _testRoute;
    private Route _testRoute2;
    private Grid<ModelGridObject> _grid;

    [SetUp]
    public void Init()
    {
        SetUpGrid();
        _testTruck = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        
        _testTruck2 = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
    }

    private void SetUpRoute1()
    {
        _testRoute = new Route(new List<Location>()
        {
            new(0,0),
            new(1,0),
            new(3,0),
            new(4,0),
            new(3,0),
            new(1,0),
            new(0,0)
        });
    }
    
    // C R R R C
    // R   B   R
    // R       R
    // R F   F R
    // C R R R C
    private void SetUpGrid()
    {
        _grid = new Grid<ModelGridObject>(new Size(5, 5), 10, new System.Numerics.Vector3(0,0,0),
            (g, l) => new ModelGridObject(g, l));
        
        // Corners
        _grid.GetGridObject(0,0).SetModel(new TwoWayCornerUR(new Location(0,0)));
        _grid.GetGridObject(0,4).SetModel(new TwoWayCornerDR(new Location(0,4)));
        _grid.GetGridObject(4,0).SetModel(new TwoWayCornerUL(new Location(4,0)));
        _grid.GetGridObject(4,4).SetModel(new TwoWayCornerDL(new Location(4,4)));
        
        // Vertical
        _grid.GetGridObject(0,1).SetModel(new TwoWayUD(new Location(0,1)));
        _grid.GetGridObject(0,2).SetModel(new TwoWayUD(new Location(0,2)));
        _grid.GetGridObject(0,3).SetModel(new TwoWayUD(new Location(0,3)));
        _grid.GetGridObject(4,1).SetModel(new TwoWayUD(new Location(4,1)));
        _grid.GetGridObject(4,2).SetModel(new TwoWayUD(new Location(4,2)));
        _grid.GetGridObject(4,3).SetModel(new TwoWayUD(new Location(4,3)));
        
        // Horizontal
        _grid.GetGridObject(1,0).SetModel(new TwoWayLR(new Location(1,0)));
        _grid.GetGridObject(2,0).SetModel(new TwoWayLR(new Location(2,0)));
        _grid.GetGridObject(3,0).SetModel(new TwoWayLR(new Location(3,0)));
        _grid.GetGridObject(1,4).SetModel(new TwoWayLR(new Location(1,4)));
        _grid.GetGridObject(2,4).SetModel(new TwoWayLR(new Location(2,4)));
        _grid.GetGridObject(3,4).SetModel(new TwoWayLR(new Location(3,4)));
        
        // Bus stop
        _grid.GetGridObject(2,3).SetModel(new BusStop(new Location(2,3)));
        
        // Factory
        ProcessingBuildingSteel pbs = new ProcessingBuildingSteel(new Location(2, 3),
            rch: new RateChangeHandler(100,100,0,1,100));
        _grid.GetGridObject(1,1).SetModel(pbs);

        ExtractorBuilding ebi = new ExtractorBuilding(Resource.Iron, 100, new Location(3,1),
            rch: new RateChangeHandler(100,100,0,1,100));
        _grid.GetGridObject(3,1).SetModel(ebi);
        
        
        
        
    }

    private void gridsetup2()
    {
        _grid = new Grid<ModelGridObject>(new Size(3, 3), 5, new System.Numerics.Vector3(0,0,0),
            (g, l) => new ModelGridObject(g, l));
        
        _grid.GetGridObject(1,1).SetModel(new FourWay(new Location(1,1)));
        _grid.GetGridObject(0,1).SetModel(new TwoWayLR(new Location(0,1)));
        _grid.GetGridObject(2,1).SetModel(new TwoWayLR(new Location(2,1)));
        _grid.GetGridObject(1,0).SetModel(new TwoWayUD(new Location(1,0)));
        _grid.GetGridObject(1,2).SetModel(new TwoWayUD(new Location(1,3)));
    }
    
    [Test]
    public void CargoTruckConstructor()
    {
        Assert.AreEqual(_testTruck.Resource, Resource.Iron);
        Assert.AreEqual(_testTruck.MoveSpeed, 2f);
        Assert.AreEqual(_testTruck.MaintenanceCost, 5);
        Assert.AreEqual(_testTruck.PurchaseCost, 50);
        Assert.AreEqual(_testTruck.ResourceAmount, 0);
    }


    [Test]
    public void CarTeleportsToRouteStartLocationWhenRouteIsSet()
    {
        SetUpRoute1();
        Assert.IsNull(_testTruck.CurrentLocation);
        _testTruck.SetRoute(_testRoute);
        Assert.AreEqual(_testRoute.CurrentVertex, _testTruck.CurrentLocation);
    }

    
    [Test]
    public void VehicleMovesAlongTheRoute()
    {
        SetUpRoute1();
        _testTruck.SetRoute(_testRoute);
        
        Assert.AreEqual(new Location(0,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(1,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(2,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(3,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(4,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(3,0), _testTruck.CurrentLocation);
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        Assert.AreEqual(new Location(0,0), _testTruck.CurrentLocation);
        
    }

    [Test]
    public void VehiclePickUpResourceFromIResourceProvider()
    {
        SetUpRoute1();
        _testTruck = new CargoTruck(_grid, Resource.Steel, resourceAmount: 50);
        ProcessingBuildingSteel pbs = _grid.GetGridObject(1,1).Model as ProcessingBuildingSteel;
        pbs!.AddResource(200);
        pbs.Tick(100);
        _testTruck.SetRoute(_testRoute);
        
        Assert.AreEqual(0, _testTruck.ResourceAmount);
        _testTruck.MoveNext();
        Assert.AreEqual(50, _testTruck.ResourceAmount);
        Assert.AreEqual(50, pbs.ResourceAmount);
    }
    
    [Test]
    public void VehicleDepositResourceToIDepositPoint()
    {
        SetUpRoute1();
        _testTruck = new CargoTruck(_grid, Resource.Iron, resourceAmount: 50);
        ProcessingBuildingSteel pbs = _grid.GetGridObject(1,1).Model as ProcessingBuildingSteel;
        ExtractorBuilding ebi = _grid.GetGridObject(3,1).Model as ExtractorBuilding;
        ebi!.Tick(100);
        _testTruck.SetRoute(_testRoute);
        
        Assert.AreEqual(0, _testTruck.ResourceAmount);
        Assert.AreEqual(0, pbs!.ResourceAmount);
        
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        Assert.AreEqual(50, _testTruck.ResourceAmount);
        Assert.AreEqual(50, ebi.ResourceAmount);
        
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        _testTruck.MoveNext();
        Assert.AreEqual(0, _testTruck.ResourceAmount);
        Assert.AreEqual(50, pbs.RequiredResourceAmount);
        
    }

    
    //    V         
    //    R    or  V R V
    //    V
    [Test]
    public void CrossroadStraightPassing()
    {
        gridsetup2();
        
        _testTruck = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        _testTruck2 = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);

        #region LeftToRight

        _testRoute = new Route(new List<Location>()
        {
            new(0,1),
            new(1,1),
            new(2,1),
            new(1,1),
            new(0,1)
        });
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(2,1),
            new(1,1),
            new(0,1),
            new(1,1),
            new(2,1)
        });
        
        _testTruck.SetRoute(_testRoute);
        _testTruck2.SetRoute(_testRoute2);
        
        Assert.AreEqual(new Location (0, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (2, 1), _testTruck2.CurrentLocation);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (2, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (0, 1), _testTruck2.CurrentLocation);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (0, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (2, 1), _testTruck2.CurrentLocation);

            #endregion

        #region UpToDown

            _testRoute = new Route(new List<Location>()
            {
                new(1,2),
                new(1,1),
                new(1,0),
                new(1,1),
                new(1,2)
            });
        
            _testRoute2 = new Route(new List<Location>()
            {
                new(1,0),
                new(1,1),
                new(1,2),
                new(1,1),
                new(1,0)
            });

            _testTruck.SetRoute(_testRoute);
            _testTruck2.SetRoute(_testRoute2);
        
            Assert.AreEqual(new Location (1, 2), _testTruck.CurrentLocation);
            Assert.AreEqual(new Location (1, 0), _testTruck2.CurrentLocation);
        
            _testTruck.MoveNext();
            _testTruck2.MoveNext();
        
            Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
            Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        
            _testTruck.MoveNext();
            _testTruck2.MoveNext();
        
            Assert.AreEqual(new Location (1, 0), _testTruck.CurrentLocation);
            Assert.AreEqual(new Location (1, 2), _testTruck2.CurrentLocation);
        
            _testTruck.MoveNext();
            _testTruck2.MoveNext();
        
            Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
            Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        
            _testTruck.MoveNext();
            _testTruck2.MoveNext();
        
            Assert.AreEqual(new Location (1, 2), _testTruck.CurrentLocation);
            Assert.AreEqual(new Location (1, 0), _testTruck2.CurrentLocation);
            #endregion
    }

    
    //             
    //  V R    or   V R V   or    R V  ....stb
    //    V                       V
    [Test]
    public void RightTurnPassing()
    {
        gridsetup2();
        
        _testTruck = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        _testTruck2 = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        
        #region RoutesAndSteps
        
        //posive tests
        _testRoute = new Route(new List<Location>()
        {
            new(0,1),
            new(1,1),
            new(1,2),
            new(1,1),
            new(0,1)
        });
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(1,0),
            new(1,1),
            new(2,1),
            new(1,1),
            new(1,0)
        });
        
        _testTruck.SetRoute(_testRoute);
        _testTruck2.SetRoute(_testRoute2);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        
        //negative tests - megall a kamion ha nem mehet
        
        _testRoute = new Route(new List<Location>()
        {
            new(1,0),
            new(1,1),
            new(2,1),
            new(1,1),
            new(1,0)
        });
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(0,1),
            new(1,1),
            new(2,1),
            new(1,1),
            new(0,1)
        });
        
        _testTruck.SetRoute(_testRoute);
        _testTruck2.SetRoute(_testRoute2);
        
        Assert.AreEqual(new Location (1, 0), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (0, 1), _testTruck2.CurrentLocation);

        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (1, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (0, 1), _testTruck2.CurrentLocation);
        
        _testTruck.MoveNext();
        _testTruck2.MoveNext();
        
        Assert.AreEqual(new Location (2, 1), _testTruck.CurrentLocation);
        Assert.AreEqual(new Location (1, 1), _testTruck2.CurrentLocation);
        #endregion
    }

    [Test]
    public void LeftTurnPassing()
    {
        gridsetup2();
        
        _testTruck = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        _testTruck2 = new CargoTruck(_grid, Resource.Iron, 2f, 5, 50, 100);
        
        _testRoute = new Route(new List<Location>()
        {
            new(0,1),
            new(1,1),
            new(1,2),
            new(1,1),
            new(0,1)
        });
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(1,0),
            new(1,1),
            new(2,1),
            new(1,1),
            new(1,0)
        });
    }
    
    // [Test]
    // public void CargoTruckMovement()
    // {
    //     CargoTruck _crossingTruck = new CargoTruck(_grid, Resource.Steel, 2f, 5, 50, 100);
    //     Route _crossingRoute = new Route(new List<Location>
    //     {
    //         new Location(-1, 1),
    //         new Location(0, 1), 
    //         new Location(1, 1),
    //         new Location(2, 1), 
    //         new Location(3, 1),
    //         new Location(-1, 1)
    //     });
    //     _crossingTruck.SetRoute(_crossingRoute);
    //     
    //     Assert.AreEqual(new Location(0, 0), _testTruck.CurrentLocation);
    //     Assert.AreEqual(new Location(0, 1), _testTruck.Route.NextVertex);
    //     
    //     RoadCell road01 = new RoadCell(_testTruck.Route.NextVertex, true,
    //         new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
    //     
    //     _testTruck.MoveNext();
    //     
    //     Assert.AreEqual(new Location(0, 1), _testTruck.CurrentLocation);
    //     Assert.AreEqual(new Location(-1, 1), _crossingTruck.CurrentLocation);
    //     //nem tud lepni, mert az elozo kamion ott van
    //     _crossingTruck.MoveNext();
    //     
    //     Assert.AreEqual(new Location(-1, 1), _crossingTruck.CurrentLocation);
    //     
    //     RoadCell road02 = new RoadCell(_testTruck.Route.NextVertex, true,
    //         new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
    //     
    //     _testTruck.MoveNext();
    //     
    //     Assert.AreEqual(new Location(0, 2), _testTruck.CurrentLocation);
    //     
    //     _crossingTruck.MoveNext();
    //     
    //     Assert.AreEqual(new Location(0, 1), _crossingTruck.CurrentLocation);
    // }
    //
    // [Test]
    // public void CargoTruckTakesAndPutsResource()
    // {
    //     Facility extract = new ExtractorBuilding(Resource.Iron, 50000, new Location(-1, 0), 50, new Size(1, 1));
    //     for (int i = 0; i < 50000; i++)
    //     {
    //         extract.Tick(10f);
    //     }
    //     Facility produce = new ProcessingBuilding(Resource.Steel, Resource.Iron, 50000, 
    //         new Location(-1, -1), 1f, new Size(1, 1), false, 
    //         new RateChangeHandler(10, 100, 1, 120f, 50));
    //     
    //     RoadCell road01 = new RoadCell(_testTruck.Route.NextVertex, true,
    //         new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
    //     
    //     _testTruck.MoveNext();
    //     
    //     Assert.AreEqual(new  Location(0, 0), _testTruck.CurrentLocation);
    //     Assert.AreEqual(100, _testTruck.ResourceAmount);
    //     
    //     _testTruck.MoveNext();
    //     
    //     Assert.AreEqual(new  Location(0, 1), _testTruck.CurrentLocation);
    //     
    //     RoadCell road02 = new RoadCell(_testTruck.Route.NextVertex, true,
    //         new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
    //     
    //     _testTruck.MoveNext();
    //     
    //     Assert.AreEqual(new  Location(0, 1), _testTruck.CurrentLocation);
    //     Assert.AreEqual(0, _testTruck.ResourceAmount);
    //     
    //     RoadCell road03 = new RoadCell(_testTruck.Route.NextVertex, true,
    //         new List<Direction>() { Direction.Down, Direction.Left, Direction.Right, Direction.Up }, true);
    //     
    //     _testTruck.MoveNext();
    // }
}
