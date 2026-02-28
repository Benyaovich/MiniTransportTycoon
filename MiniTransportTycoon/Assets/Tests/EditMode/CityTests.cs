using NUnit.Framework;

public class CityTests
{

    private City city = null!;
    private int initPeopleInCity;
    
    [SetUp]
    public void Initialize()
    {
        city = new City(new Location(0, 0), rch: new RateChangeHandler(1, 1, 0, 1, 1));
        initPeopleInCity = city.NumOfPeople;
    }
    
    [Test]
    public void AddPeopleTest()
    {
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity);
        city.AddPeople(1);
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity + 1);
    }
    
    [Test]
    public void RemovePeopleTest()
    {
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity);
        city.RemovePeople(1);
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity - 1);
    }
    
    [Test]
    public void NumOfPeopleCantBeNegativeAfterRemovingPeopleTest()
    {
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity);
        city.RemovePeople(initPeopleInCity + 1);
        Assert.AreEqual(city.NumOfPeople,0);
    }

    [Test]
    public void ProvidePeopleToBusStopTest()
    {
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity);
        Assert.AreEqual(city.ProvidePeopleToBusStop(),1);
        Assert.AreEqual(city.NumOfPeople,initPeopleInCity - 1);
    }
}
