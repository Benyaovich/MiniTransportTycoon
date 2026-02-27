using UnityEngine;
using System;

public class RateChangeHandler : IAdvancable
{
    private Timer rateChangeTimer;
    private int minRate;
    private int maxRate;
    private int currentRate;
    private int rateChange;

    //rakd hozza osztalydiagramhoz
    private float rateChangeInterval; //masodperc 
    private System.Random rnd;

    public RateChangeHandler(int minRate, int maxRate, int rateChange, float rateChangeInterval, int currentRate = -1)
    {
        rnd = new System.Random();
        
        this.minRate = minRate;
        this.maxRate = maxRate;
        
        this.currentRate = currentRate == -1 ? rnd.Next(minRate + rateChange, maxRate - rateChange + 1) : currentRate;

        if (this.currentRate < minRate || this.currentRate > maxRate)
        {
            throw new ArgumentOutOfRangeException("currentRate", "Invalid current rate, must be between (minRate + rateChange) and (maxRate - rateChange)");
        }
        this.rateChange = rateChange;
        this.rateChangeInterval = rateChangeInterval;

        rateChangeTimer = new Timer(rateChangeInterval);
        rateChangeTimer.OnTimerEllapsed += (UpdateCurrentRate);
    }
    

    private void UpdateCurrentRate(object e, EventArgs args)
    {
        int change = rnd.Next(0, 2);

        //van itt egy padding igazabol a ket szel kozott
        if (change == 0 && (currentRate - rateChange) >= (minRate + rateChange))
        {
            currentRate -= rateChange;
        }  
        else if (change == 1 && (currentRate + rateChange) <= (maxRate - rateChange))
        {
            currentRate += rateChange;
        }
    }

    public int GetValue()
    {
        return rnd.Next(currentRate - rateChange, currentRate + rateChange + 1);
    }

    public void Tick(float deltaTime)
    {
        rateChangeTimer.Tick(deltaTime);
    }
}
