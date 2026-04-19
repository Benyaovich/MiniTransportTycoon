
using System;
using Model.Interfaces;

public abstract class Facility : Cell, IAdvancable, IVisitableBuiling, IResourceProvider
{
    public Resource ProducedResource { get; private set; }
    public int ResourceAmount { get; protected set; }
    public int MaxCapacity { get; protected set; }
    private readonly Timer _productionTimer;
    public RateChangeHandler Rch { get; }

    internal Facility(Resource prodRes, int maxCap, Location loc, float prodInterval = 10f, 
        Size size = null, bool destroyable = false, RateChangeHandler rch = null) : base(loc, size, destroyable)
    {
        ProducedResource = prodRes;
        MaxCapacity = maxCap;
        _productionTimer = new Timer(prodInterval);
        Rch = rch ?? new RateChangeHandler();
        Size = size ?? new Size(2, 2);
        
        _productionTimer.OnTimerElapsed += Produce;
    }

    protected abstract void Produce(object sender, EventArgs e);

    public void Tick(float deltaTime)
    {
        _productionTimer.Tick(deltaTime);
        Rch.Tick(deltaTime);
    }

    public int GetResource(int amount)
    {
        if (amount <= 0) return 0;

        if (amount >= ResourceAmount)
        {
            int temp = ResourceAmount;
            ResourceAmount = 0;
            return temp;
        }

        ResourceAmount -= amount;
        return amount;
    }


}
