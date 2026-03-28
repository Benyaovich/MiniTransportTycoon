using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

public class BusStopTests
{
    private Grid<ModelGridObject> _grid;
    private CityService _cityService;
    private IBuildingManager _buildingManager;
    
    [SetUp]
    public void Init()
    {
        _grid= new Grid<ModelGridObject>(new Size(10, 10), 10, Vector3.Zero,
            (g, l) => new ModelGridObject(g,l));
        _cityService = new CityService();
        _buildingManager = new BuildingManager(_grid, _cityService, new List<IAdvancable>());
    }
    
    [Test]
    public void BusStopLocatesTheNearbyCity()
    {
        City city = new SmallCity(new Location(0,0));
        _cityService.AddCity(city, city.GetGridPositionList());

        var bs = CreateBusStop(new Location(2, 3));
        Assert.AreEqual(city, bs.City);
    }
    
    [Test]
    public void BusStopTriesLocatesTheNearbyCityButFoundsNone()
    {
        City city = new SmallCity(new Location(0,0));
        _cityService.AddCity(city, city.GetGridPositionList());

        var bs = CreateBusStop(new Location(5, 5));
        Assert.IsNull(bs.City);
    }

    [Test]
    public void GetPeopleFromCity()
    {
        City city = new SmallCity(new Location(0,0),
            rch: new RateChangeHandler(1,1,0,1,1));
        _cityService.AddCity(city, city.GetGridPositionList());
        var bs = CreateBusStop(new Location(2, 3));
        Assert.AreEqual(0, bs.NumOfPeople);
        bs.Tick(1);
        Assert.AreEqual(1, bs.NumOfPeople);
        bs.Tick(1);
        Assert.AreEqual(2, bs.NumOfPeople);
    }
    
    [Test]
    public void GetPeopleFromCityDoesntExceedMaxNumOfPeopleInBusStop()
    {
        City city = new SmallCity(new Location(0,0),
            rch: new RateChangeHandler(20,20,0,1,20));
        _cityService.AddCity(city, city.GetGridPositionList());
        var bs = CreateBusStop(new Location(2, 3));
        bs.Tick(1);
        bs.Tick(1);
        Assert.AreEqual(40, bs.NumOfPeople);
        bs.Tick(1);
        Assert.AreEqual(50, bs.NumOfPeople);
        bs.Tick(1);
        Assert.AreEqual(50, bs.NumOfPeople);
    }

    [Test]
    public void GetPeopleFromBusStopRemovesTheCorrectAmount()
    {
        City city = new SmallCity(new Location(0,0),
            rch: new RateChangeHandler(10,10,0,1,10));
        _cityService.AddCity(city, city.GetGridPositionList());
        var bs = CreateBusStop(new Location(2, 3));
        
        bs.Tick(1);
        Assert.AreEqual(10, bs.NumOfPeople);
        int amount = bs.GetPeopleFromBusStop(2);
        Assert.AreEqual(2, amount);
        Assert.AreEqual(8, bs.NumOfPeople);

        amount = bs.GetPeopleFromBusStop(20);
        Assert.AreEqual(8, amount);
        Assert.AreEqual(0, bs.NumOfPeople);
    }

    [Test]
    public void AddingPeopleToBusStopInstantlyTransfersThemToTheCity()
    {
        City city = new SmallCity(new Location(0,0),
            rch: new RateChangeHandler(10,10,0,1,10));
        _cityService.AddCity(city, city.GetGridPositionList());
        var bs = CreateBusStop(new Location(2, 3));
        
        Assert.AreEqual(100, city.NumOfPeople);
        Assert.AreEqual(0, bs.NumOfPeople);
        bs.AddPeopleToBusStop(10);
        Assert.AreEqual(110, city.NumOfPeople);
        Assert.AreEqual(0, bs.NumOfPeople);
    }
    
    private BusStop CreateBusStop(Location location,
        float interval = 1, int range = 3,
        int maxNumOfPeople = 50)
    {
        BusStop bs = new BusStop(location, _cityService,
            interval: interval, range: range,
            maxNumOfPeople: maxNumOfPeople);
        _buildingManager.TryBuild(bs);
        return bs;
    }
}
