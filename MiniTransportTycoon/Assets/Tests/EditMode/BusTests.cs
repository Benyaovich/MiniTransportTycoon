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
        new(0, 0),
        new(4, 0),
        new(0, 0)
    };

    [SetUp]
    public void Init()
    {
        SetUpGrid();
        
        _cityService = new CityService();
        _buildingManager = new CellBuildingManager(_grid, new DynamicRoadBuildingManager(_grid), _cityService, new List<IAdvancable>());
        
        _testBus = new Bus(_grid);
        _testBus2 = new Bus(_grid);
    }

    private void SetUpGrid()
    {
        _grid = new Grid<ModelGridObject>(new Size(5, 3), 10, new System.Numerics.Vector3(0, 0, 0),
            (g, l) => new ModelGridObject(g, l));
        
        //city
        City city1 = new SmallCity(new Location(0,2), 
            rch: new RateChangeHandler(1,1,0,1,1));
        _cityService.AddCity(city1, city1.GetGridPositionList());
        BusStop bs1 = new BusStop(new Location(0, 1), _cityService, new Size(1, 1), interval: 1);
        _buildingManager.TryBuild(bs1);
        
        City city2 = new SmallCity(new Location(4,2), 
            rch: new RateChangeHandler(1,1,0,1,1));
        _cityService.AddCity(city2, city2.GetGridPositionList());
        BusStop bs2 = new BusStop(new Location(0, 1), _cityService, new Size(1, 1), interval: 1);
        _buildingManager.TryBuild(bs2);

        //Road
        _grid.GetGridObject(0, 0).SetModel(new TwoWayLR(new Location(0, 0)));
        _grid.GetGridObject(1, 0).SetModel(new TwoWayLR(new Location(1, 0)));
        _grid.GetGridObject(2, 0).SetModel(new TwoWayLR(new Location(2, 0)));
        _grid.GetGridObject(3, 0).SetModel(new TwoWayLR(new Location(3, 0)));
        _grid.GetGridObject(4, 0).SetModel(new TwoWayLR(new Location(4, 0)));

        // Bus stop
        _grid.GetGridObject(1, 1).SetModel(new BusStop(new Location(1, 1)));
        _grid.GetGridObject(4, 1).SetModel(new BusStop(new Location(1, 1)));

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
            new(0, 0),
            new(4, 0),
            new(0, 0)
        }, _pathHandler);
        
        _testRoute2 = new Route(new List<Location>()
        {
            new(4, 0),
            new(0, 0),
            new(4, 0)
        }, _pathHandler);
    }
    
    
    [Test]
    public void BusPicksUpAndPutsDownPassengers()
    {
        SetUpRoute();
        
        _testBus.SetRoute(_testRoute);
        _testBus2.SetRoute(_testRoute2);
        
        Assert.AreEqual(new Location(0, 0), _testBus.CurrentLocation);
        Assert.AreEqual(new Location(4, 0), _testBus2.CurrentLocation);
    }
}
