using System;

[Serializable]
public class SCity : SCell
{
    public int numOfPeople;
    
    public SCity(City city) : base(city)
    {
        numOfPeople = city.NumOfPeople;
    }
    
    public SCity()
    {
        
    }
}

[Serializable]
public class SSmallCity : SCity
{
    public SSmallCity(SmallCity smallCity) : base(smallCity)
    {
        
    }
    
    public SSmallCity()
    {
        
    }
}