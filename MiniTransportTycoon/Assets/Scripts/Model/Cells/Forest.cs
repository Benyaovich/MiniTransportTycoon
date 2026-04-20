using System;
using JetBrains.Annotations;
using Model.Interfaces;

public class Forest : Cell, IAdvancable, IDestroyable
{

    public int NumOfTrees { get; private set; } = 1;
    public bool CanDestroy => true;
    public int DestroyPrice => NumOfTrees * 50;
    private Timer growthTimer;
    private Timer spreadTimer;
    private readonly Random random;
    [CanBeNull] public EventHandler<Location> OnSpread;
    [CanBeNull] public EventHandler<Location> OnGrow;

    public Forest(Location origin, Size size = null, bool destroyable = true,float growthInterval = 60, float spreadInterval = 60) : base(origin, size, destroyable)
    {
        growthTimer = new Timer(growthInterval);
        growthTimer.OnTimerElapsed += GrowTree;
        spreadTimer = new Timer(spreadInterval);
        spreadTimer.OnTimerElapsed += Spread;
        random = new Random();
    }

    public void Tick(float deltaTime)
    {
        growthTimer.Tick(deltaTime);
        spreadTimer.Tick(deltaTime);
    }

    public void Spread([CanBeNull] object sender, EventArgs e)
    {
        if(!(NumOfTrees == 3 || NumOfTrees == 4)) return;
        
        bool spread = random.Next(3) == 1;
        if (spread) OnSpread?.Invoke(this, Origin);
    }

    public void GrowTree([CanBeNull] object sender, EventArgs e)
    {
        if (NumOfTrees == 4) return;
        NumOfTrees++;
        OnGrow?.Invoke(this, Origin);
    }

    
}
