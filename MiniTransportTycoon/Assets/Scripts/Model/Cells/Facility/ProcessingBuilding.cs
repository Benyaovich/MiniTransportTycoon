using System;
using Model.Interfaces;
using UnityEngine;

public class ProcessingBuilding : Facility, IDepositPoint
{
    public Resource RequiredResource { get; private set; }
    public int RequiredResourceAmount { get; private set; }
    public int RequiredResourceCapacity { get; }

    public ProcessingBuilding(Resource prodRes, Resource reqRes, int maxCap,
        Location loc, float prodInterval = 10, Size size = null, 
        bool destroyable = false, RateChangeHandler rch = null)
        : base(prodRes, maxCap, loc, prodInterval, size, destroyable, rch)
    {
        RequiredResource = reqRes;
        RequiredResourceCapacity = maxCap;
    }

    // return the amount left after adding to the facility
    public int AddResource(int amount)
    {
        if (amount <= 0) return 0;
        if (RequiredResourceAmount == RequiredResourceCapacity) return amount;
        
        RequiredResourceAmount += amount;
        
        if (RequiredResourceAmount <= RequiredResourceCapacity) return 0;
        
        int overhead = RequiredResourceAmount - RequiredResourceCapacity;
        RequiredResourceAmount = RequiredResourceCapacity;
        return overhead;

    }

    // 1 to 1 conversion when producing resource
    protected override void Produce(object sender, EventArgs e)
    {
        if (RequiredResourceAmount <= 0)
            return;

        int freeCapacity = MaxCapacity - ResourceAmount;

        if (freeCapacity <= 0)
            return;

        int producedAmount = Math.Min(
            Rch.GetValue(),
            Math.Min(RequiredResourceAmount, freeCapacity)
        );

        ResourceAmount += producedAmount;
        RequiredResourceAmount -= producedAmount;
    }
}
