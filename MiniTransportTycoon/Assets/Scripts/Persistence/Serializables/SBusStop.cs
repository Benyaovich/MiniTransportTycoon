using System;

[Serializable]
public class SBusStop : SCell
{
    public int numOfPeople;
        
    public SBusStop(BusStop busStop) : base(busStop)
    {
        numOfPeople = busStop.NumOfPeople;
    }

    public SBusStop()
    {
        
    }
}
