public class City : Cell, IAdvancable
{
    public int NumOfPeople { get; private set; }
    private RateChangeHandler travelDemandRCH;
    
    public City(Location origin, Size size = null, bool destroyable = true,RateChangeHandler rch = null,int numOfPeople = 100)
        : base(origin, size, destroyable)
    {
        NumOfPeople = numOfPeople;
        travelDemandRCH = rch ?? new RateChangeHandler();
    }

    public int ProvidePeopleToBusStop()
    {
        int num = travelDemandRCH.GetValue();
        RemovePeople(num);
        return num;
    }
    
    public void Tick(float deltaTime)
    {
        travelDemandRCH.Tick(deltaTime);
    }

    public void RemovePeople(int num)
    {
        if (NumOfPeople - num < 0) NumOfPeople = 0;
        else NumOfPeople -= num;
    }

    public void AddPeople(int num)
    {
        NumOfPeople += num;
    }
}
