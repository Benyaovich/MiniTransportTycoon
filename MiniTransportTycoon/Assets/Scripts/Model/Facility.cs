
using System;

public abstract class Facility : Cell, IAdvancable
{
    public Resource ProducedResource { get; private set; }
    public int ResourceAmount { get; protected set; }
    private Timer productionTimer;
    protected int maxCapacity;
    protected RateChangeHandler rch;

    internal Facility(Resource prodRes, int maxCap, Location loc, float prodInterval = 10f, 
        Size size = null, bool destroyable = false, RateChangeHandler rch = null) : base(loc, size, destroyable)
    {
        ProducedResource = prodRes;
        maxCapacity = maxCap;
        productionTimer = new Timer(prodInterval);
        this.rch = rch ?? new RateChangeHandler();
        Size = size ?? new Size(2, 2);
        
        productionTimer.OnTimerElapsed += Produce;
    }

    protected abstract void Produce(object sender, EventArgs e);

    public void Tick(float deltaTime)
    {
        productionTimer.Tick(deltaTime);
        rch.Tick(deltaTime);
    }

    public int GetProducedResource(int amount)
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
