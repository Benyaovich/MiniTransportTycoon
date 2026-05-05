using System;
using JetBrains.Annotations;
using Model.Interfaces;

public class Forest : Cell, IAdvancable, IDestroyable
{

    public bool CanDestroy => true;
    public int DestroyPrice => NumOfTrees * 7;
    public int NumOfTrees { get; private set; }
    private readonly Timer _growthTimer;
    private readonly Timer _spreadTimer;
    private readonly Random _random;
    [CanBeNull] public EventHandler<Location> OnSpread;
    [CanBeNull] public EventHandler<Location> OnGrow;

    public Forest(Location origin, Size size = null, bool destroyable = true,float growthInterval = 60, float spreadInterval = 45, int numOfTrees = 1) : base(origin, size, destroyable)
    {
        _growthTimer = new Timer(growthInterval);
        _growthTimer.OnTimerElapsed += GrowTree;
        _spreadTimer = new Timer(spreadInterval);
        _spreadTimer.OnTimerElapsed += Spread;
        _random = new Random();
        NumOfTrees = numOfTrees;
    }

    public void Tick(float deltaTime)
    {
        _growthTimer.Tick(deltaTime);
        _spreadTimer.Tick(deltaTime);
    }

    public void Spread([CanBeNull] object sender, EventArgs e)
    {
        if(!(NumOfTrees == 3 || NumOfTrees == 4)) return;
        
        bool spread = _random.Next(20) == 1;
        if (spread) OnSpread?.Invoke(this, Origin);
    }

    public void GrowTree([CanBeNull] object sender, EventArgs e)
    {
        if (NumOfTrees == 4) return;
        NumOfTrees++;
        OnGrow?.Invoke(this, Origin);
    }

    
}
