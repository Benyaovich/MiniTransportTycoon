using System;

[Serializable]
public class SFacility : SCell
{
    public int resourceAmount;
    public SFacility(Facility facility) : base(facility)
    {
        resourceAmount = facility.ResourceAmount;
    }
    public SFacility()
    {
        
    }
}