
using System;

public class Timer : IAdvancable
{
    public float Interval { get; private set; }
    public float RemainingTime;

    public event EventHandler OnTimerElapsed;

    public Timer(float interval) {
        Interval = interval;
        RemainingTime = interval;
    }

    public void Tick(float deltaTime) {
        RemainingTime -= deltaTime;

        if(RemainingTime <= 0) {
            OnTimerElapsed?.Invoke(this, EventArgs.Empty);
            RemainingTime = RemainingTime + Interval;
        }
    }

}
