using System;
using UnityEngine;

public class ProcessingBuilding : Facility
{
    public Resource RequiredResource { get; private set; }
    public int RequiredResourceAmount { get; private set; }
    private int requiredResourceCapacity;
    
    public ProcessingBuilding(Resource prodRes, Resource reqRes, int maxCap,
        Location loc, float prodInterval = 10, Size size = null, 
        bool destroyable = false, RateChangeHandler rch = null)
        : base(prodRes, maxCap, loc, prodInterval, size, destroyable, rch)
    {
        RequiredResource = reqRes;
        requiredResourceCapacity = maxCap;
    }

    // return the amount left after adding to the facility
    public int AddRequiredResource(int amount)
    {
        if (amount <= 0) return 0;
        if (RequiredResourceAmount == requiredResourceCapacity) return amount;
        
        RequiredResourceAmount += amount;
        
        if (RequiredResourceAmount <= requiredResourceCapacity) return 0;
        
        int overhead = RequiredResourceAmount - requiredResourceCapacity;
        RequiredResourceAmount = requiredResourceCapacity;
        return overhead;

    }

    // 1 to 1 conversion when producing resource
    protected override void Produce(object sender, EventArgs e)
    {
        Debug.Log("termleek csor");
        int amountToProduce = rch.GetValue();
        
        if (amountToProduce >= RequiredResourceAmount)
        {
            ResourceAmount += RequiredResourceAmount;
            RequiredResourceAmount = 0;
            return;
        }

        ResourceAmount += amountToProduce;
        RequiredResourceAmount -= amountToProduce;
    }
}
