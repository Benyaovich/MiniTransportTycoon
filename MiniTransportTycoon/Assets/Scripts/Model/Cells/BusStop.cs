
using System;
using JetBrains.Annotations;
using Model.Interfaces;

public class BusStop : Cell, IAdvancable, IPurchasable
{
    public int NumOfPeople { get; private set; }
    public City City { get; private set; }
    public int BuildPrice { get; set; }

    private Timer _timer;
    private int _range;
    private int _maxNumOfPeople;
    private IGrid<IHasCellModel> _grid;

    public BusStop(Location location, IGrid<IHasCellModel> grid, [CanBeNull] Size size = null, bool destroyable = true,
        float interval = 15, int buildPrice = 1500, int range = 5) :
        base(location, size, destroyable)
    {
        _grid = grid;
        BuildPrice = buildPrice;
        _range = range;
        _timer = new Timer(interval);
        _timer.OnTimerElapsed += GetPeopleFromCity;
        
        LocateAndSetCity();
    }

    private void LocateAndSetCity()
    {
        int radius = (int)MathF.Floor(_range/2.0f);
        for (int y = Origin.Y - radius; y < Origin.Y + radius; y++)
        {
            for (int x = Origin.X - radius; x < Origin.X + radius; x++)
            {
                IHasCellModel gridObject = _grid.GetGridObject(x, y);
                if (gridObject?.Model is null) continue;
                if(gridObject.Model is not City city) continue;
                City = city;
                return;
            }
        }

        City = null;
    }

    private void GetPeopleFromCity(object sender, EventArgs e)
    {
        int amount = City.ProvidePeopleToBusStop();
        NumOfPeople += amount;
        if (NumOfPeople > _maxNumOfPeople)
        {
            int diff = NumOfPeople - _maxNumOfPeople;
            NumOfPeople -= diff;
            City.AddPeople(diff);
        }
    }

    public void AddPeopleToBusStop(int amount)
    {
        City.AddPeople(amount);
    }

    public int GetPeopleFromBusStop(int amount)
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
