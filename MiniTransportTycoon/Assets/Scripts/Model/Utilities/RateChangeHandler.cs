using System;

public class RateChangeHandler : IAdvancable
{
    private readonly Timer _rateChangeTimer;
    public int MinRate { get; }
    public int MaxRate { get; }
    public int CurrentRate { get; private set; }
    public int RateChange { get; }
    public float RateChangeInterval { get; }
    private readonly Random _rnd;

    public RateChangeHandler(int minRate = 0, int maxRate = 100, int rateChange = 10, float rateChangeInterval = 120, int currentRate = -1)
    {
        _rnd = new Random();
        
        MinRate = minRate;
        MaxRate = maxRate;
        
        CurrentRate = currentRate == -1 ? _rnd.Next(minRate + rateChange, maxRate - rateChange + 1) : currentRate;

        if (CurrentRate < minRate || CurrentRate > maxRate)
        {
            throw new ArgumentOutOfRangeException(nameof(currentRate), "Invalid current rate, must be between (minRate + rateChange) and (maxRate - rateChange)");
        }
        RateChange = rateChange;
        RateChangeInterval = rateChangeInterval;

        _rateChangeTimer = new Timer(rateChangeInterval);
        _rateChangeTimer.OnTimerElapsed += (UpdateCurrentRate);
    }
    

    private void UpdateCurrentRate(object e, EventArgs args)
    {
        int change = _rnd.Next(0, 2);

        //van itt egy padding igazabol a ket szel kozott
        if (change == 0 && (CurrentRate - RateChange) >= (MinRate + RateChange))
        {
            CurrentRate -= RateChange;
        }  
        else if (change == 1 && (CurrentRate + RateChange) <= (MaxRate - RateChange))
        {
            CurrentRate += RateChange;
        }
    }

    public int GetValue()
    {
        return _rnd.Next(CurrentRate - RateChange, CurrentRate + RateChange + 1);
    }

    public void Tick(float deltaTime)
    {
        _rateChangeTimer.Tick(deltaTime);
    }
}
