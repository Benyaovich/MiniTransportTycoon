using System.Collections.Generic;
using Model.Cells.Grid;
using NUnit.Framework;

public class BusTests
{
    private Bus _testBus;
    private Bus _testBus2;
    private Route _testRoute;
    private Route _testRoute2;
    private Grid<ModelGridObject> _grid;
    private CityService _cityService;
    private CellBuildingManager _buildingManager;

    private PathHandler _pathHandler = new PathHandler();

    private List<Location> _testVertices = new()
    {
        new(0, 2),
        new(4, 2),
        new(0, 2)
    };

    [SetUp]
    public void Init()
    {
        
        
        SetUpGrid();
        
        _testBus = new Bus(_grid);
        _testBus2 = new Bus(_grid);
    }

    private void SetUpGrid()
    {
        _grid = new Grid<ModelGridObject>(new Size(5, 5), 10, new System.Numerics.Vector3(0, 0, 0),
            (g, l) => new ModelGridObject(g, l));
        
        //city
        _cityService = new CityService();
        _buildingManager = new CellBuildingManager(_grid, new DynamicRoadBuildingManager(_grid), _cityService, new List<IAdvancable>());
        
        City city1 = new SmallCity(new Location(0,0), 
            rch: new RateChangeHandler(1,1,0,1,1));
        _cityService.AddCity(city1, city1.GetGridPositionList());
        BusStop bs1 = new BusStop(new Location(0, 1), _cityService, new Size(1, 1), interval: 1);
        _buildingManager.TryBuild(bs1);
        
        
        City city2 = new SmallCity(new Location(4,4), 
            rch: new RateChangeHandler(1,1,0,1,1));
        _cityService.AddCity(city2, city2.GetGridPositionList());
        BusStop bs2 = new BusStop(new Location(4, 3), _cityService, new Size(1, 1), interval: 1);
        _buildingManager.TryBuild(bs2);
        
        bs1.Tick(1);
        bs2.Tick(1);
        
        //Road
        _grid.GetGridObject(0, 2).SetModel(new TwoWayLR(new Location(0, 2)));
        _grid.GetGridObject(1, 2).SetModel(new TwoWayLR(new Location(1, 2)));
        _grid.GetGridObject(2, 2).SetModel(new TwoWayLR(new Location(2, 2)));
        _grid.GetGridObject(3, 2).SetModel(new TwoWayLR(new Location(3, 2)));
        _grid.GetGridObject(4, 2).SetModel(new TwoWayLR(new Location(4, 2)));

        // Bus stop
        _grid.GetGridObject(0, 1).SetModel(bs1);
        _grid.GetGridObject(4, 3).SetModel(bs2);

        //Factory building

        ProcessingBuildingSteel pbs = new ProcessingBuildingSteel(new Location(2, 1),
            rch: new RateChangeHandler(100, 100, 0, 1, 100));
        _grid.GetGridObject(2, 1).SetModel(pbs);

        foreach (var item in _testVertices)
        {
            _pathHandler.Graph.AddVertex(item);
        }

        _pathHandler.Graph.AddEdge(new Edge(_testVertices[0], _testVertices[1]));
    }

    private void SetUpRoute()
    {
        _testRoute = new Route(new List<Location>()
        {
            new(0, 2),
            new(4, 2),
            new(0, 2)
        }, _pathHandler);
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(4, 2),
            new(0, 2),
            new(4, 2)
        }, _pathHandler);
    }
    
    
    [Test]
    public void BusPicksUpAndPutsDownPassengers()
    {
        SetUpRoute();
        
        _testBus.SetRoute(_testRoute);
        _testBus2.SetRoute(_testRoute2);
        
        Assert.AreEqual(new Location(0, 2), _testBus.CurrentLocation);
        Assert.AreEqual(new Location(4, 2), _testBus2.CurrentLocation);
        
        Assert.AreEqual(0, _testBus.ResourceAmount);
        Assert.AreEqual(0, _testBus2.ResourceAmount);
        
        _testBus.MoveNext();
        _testBus2.MoveNext();
        
        Assert.AreEqual(1, _testBus.ResourceAmount);
        Assert.AreEqual(1, _testBus2.ResourceAmount);
        
        _testBus.MoveNext();
        _testBus2.MoveNext();
        
        Assert.AreEqual(new Location(1, 2), _testBus.CurrentLocation);
        Assert.AreEqual(new Location(3, 2), _testBus2.CurrentLocation);
        
        for(int i = 0; i < 5 ; i++)
        {
            _testBus.MoveNext();
            _testBus2.MoveNext();
        }
        
        Assert.AreEqual(new Location(4, 2), _testBus.CurrentLocation);
        Assert.AreEqual(new Location(0, 2), _testBus2.CurrentLocation);
        
        Assert.AreEqual(0, _testBus.ResourceAmount);
        Assert.AreEqual(0, _testBus2.ResourceAmount);
        
        //vissza ut + vissza fordulas
        
        _testBus.MoveNext();
        _testBus2.MoveNext();
    }
}
