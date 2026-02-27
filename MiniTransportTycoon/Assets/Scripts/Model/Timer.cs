
using System;

public class Timer : IAdvancable
{
    public float Interval { get; private set; }
    public float RemainingTime { get; private set; }

    public event EventHandler OnTimerEllapsed;

    public Timer(float interval) {
        Interval = interval;
        RemainingTime = interval;
    }

    public void Tick(float deltaTime) {
        RemainingTime -= deltaTime;

        if(RemainingTime <= 0) {
            OnTimerEllapsed?.Invoke(this, EventArgs.Empty);
            RemainingTime = RemainingTime + Interval;
        }
    }

}
