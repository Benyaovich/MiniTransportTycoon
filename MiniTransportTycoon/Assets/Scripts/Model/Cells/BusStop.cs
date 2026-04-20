
using System;
using JetBrains.Annotations;
using Model.Interfaces;

public class BusStop : Cell, IAdvancable, IPurchasable, IVisitableBuiling, IDepositPoint, IResourceProvider
{
    public int NumOfPeople { get; private set; }
    [CanBeNull] public City City { get; private set; }
    public int Price { get; set; }
    public Resource ProducedResource => Resource.People;
    public Resource RequiredResource => Resource.People;
    private readonly Timer _timer;
    private readonly int _range;
    public int MaxNumOfPeople { get; }
    [CanBeNull] private CityService _cityService;

    public BusStop(Location location, [CanBeNull] CityService cityService = null, [CanBeNull] Size size = null, bool destroyable = true,
        float interval = 15, int buildPrice = 1000,
        int range = 5, int maxNumOfPeople = 50) :
        base(location, size, destroyable)
    {
        _cityService = cityService;
        Price = buildPrice;
        MaxNumOfPeople = maxNumOfPeople;
        _range = range;
        _timer = new Timer(interval);
        _timer.OnTimerElapsed += GetPeopleFromCity;
        
        LocateAndSetCity();
    }

    public void SetCityService(CityService cityService)
    {
        _cityService = cityService;
    }
    public void LocateAndSetCity()
    {
        if (_cityService is null) return;
        
        int radius = (int)MathF.Floor(_range/2.0f);
        for (int y = Origin.Y - radius; y < Origin.Y + radius; y++)
        {
            for (int x = Origin.X - radius; x < Origin.X + radius; x++)
            {
                if (!_cityService.CityByLocationMap.TryGetValue(new Location(x, y), out City city)) continue;
                
                City = city;
                return;
            }
        }

        City = null;
    }

    private void GetPeopleFromCity(object sender, EventArgs e)
    {
        if (City is null) return;
        
        int amount = City.ProvidePeopleToBusStop();
        NumOfPeople += amount;
        if (NumOfPeople > MaxNumOfPeople)
        {
            int diff = NumOfPeople - MaxNumOfPeople;
            NumOfPeople -= diff;
            City.AddPeople(diff);
        }
    }

    public int AddResource(int amount)
    {
        if (City is null) return 0;
        
        City.AddPeople(amount);
        return 0;
    }

    

    public int GetResource(int amount)
    {
        if (amount > NumOfPeople)
        {
            int temp = NumOfPeople;
            NumOfPeople = 0;
            return temp;
        }

        NumOfPeople -= amount;
        return amount;
    }

    public void Tick(float deltaTime)
    {
        _timer.Tick(deltaTime);
    }
}
