using System;

public class RateChangeHandler : IAdvancable
{
    private readonly Timer _rateChangeTimer;
    private readonly int _minRate;
    private readonly int _maxRate;
    public int CurrentRate { get; private set; }
    private readonly int _rateChange;
    public float RateChangeInterval { get; }
    private readonly Random _rnd;

    public RateChangeHandler(int minRate = 0, int maxRate = 100, int rateChange = 10, float rateChangeInterval = 120, int currentRate = -1)
    {
        _rnd = new Random();
        
        _minRate = minRate;
        _maxRate = maxRate;
        
        CurrentRate = currentRate == -1 ? _rnd.Next(minRate + rateChange, maxRate - rateChange + 1) : currentRate;

        if (CurrentRate < minRate || CurrentRate > maxRate)
        {
            throw new ArgumentOutOfRangeException(nameof(currentRate), "Invalid current rate, must be between (minRate + rateChange) and (maxRate - rateChange)");
        }
        _rateChange = rateChange;
        RateChangeInterval = rateChangeInterval;

        _rateChangeTimer = new Timer(rateChangeInterval);
        _rateChangeTimer.OnTimerElapsed += (UpdateCurrentRate);
    }
    

    private void UpdateCurrentRate(object e, EventArgs args)
    {
        int change = _rnd.Next(0, 2);

        //van itt egy padding igazabol a ket szel kozott
        if (change == 0 && (CurrentRate - _rateChange) >= (_minRate + _rateChange))
        {
            CurrentRate -= _rateChange;
        }  
        else if (change == 1 && (CurrentRate + _rateChange) <= (_maxRate - _rateChange))
        {
            CurrentRate += _rateChange;
        }
    }

    public int GetValue()
    {
        return _rnd.Next(CurrentRate - _rateChange, CurrentRate + _rateChange + 1);
    }

    public void Tick(float deltaTime)
    {
        _rateChangeTimer.Tick(deltaTime);
    }
}
