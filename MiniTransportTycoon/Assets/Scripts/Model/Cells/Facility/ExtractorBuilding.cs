using System;

public class ExtractorBuilding : Facility
{
    public ExtractorBuilding(Resource prodRes, int maxCap, Location loc, 
        float prodInterval = 10, Size size = null, 
        bool destroyable = false, RateChangeHandler rch = null) 
        : base(prodRes, maxCap, loc, prodInterval, size, destroyable, rch)
    {
    }

    protected override void Produce(object sender, EventArgs e)
    {
        ResourceAmount = Math.Min(ResourceAmount + Rch.GetValue(), MaxCapacity);
    }
}
