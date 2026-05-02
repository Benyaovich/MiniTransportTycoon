using System;

public class ExtractorBuilding : Facility
{
    public ExtractorBuilding(Resource prodRes, int maxCap, Location loc, 
        float prodInterval = 60, Size size = null, 
        bool destroyable = false, RateChangeHandler rch = null, int resourceAmount = 0) 
        : base(prodRes, maxCap, loc, prodInterval, size, destroyable, rch, resourceAmount)
    {
    }

    protected override void Produce(object sender, EventArgs e)
    {
        ResourceAmount = Math.Min(ResourceAmount + Rch.GetValue(), MaxCapacity);
    }
}
