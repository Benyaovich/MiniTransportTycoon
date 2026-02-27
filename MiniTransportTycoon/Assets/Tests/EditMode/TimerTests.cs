using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimerTests
{
    [Test]
    public void CreatingTheTimerSetTheRemainingTimeAndInterval()
    {
        Timer timer = new Timer(1f);
        Assert.AreEqual(1f, timer.Interval);
        Assert.AreEqual(1f, timer.RemainingTime);
    }

    [Test]
    public void DecreasingTheRemainTime()
    {
        Timer timer = new Timer(1f);
        timer.Tick(0.5f);
        Assert.AreEqual(0.5f, timer.RemainingTime);
        Assert.AreEqual(1f, timer.Interval);
        
        timer.Tick(0.25f);
        Assert.AreEqual(0.25f, timer.RemainingTime);
        
    }

    [Test]
    public void DecreasingRemainingTimeToZeroFiresEvent()
    {
        Timer timer = new Timer(1f);
        bool eventFired = false;
        timer.OnTimerEllapsed += (sender, args) => eventFired = true;
        
        timer.Tick(0.5f);
        Assert.IsFalse(eventFired);
        
        timer.Tick(0.5f);
        Assert.IsTrue(eventFired);
        
    }

    [Test]
    public void TimerRestarts()
    {
        Timer timer = new Timer(1f);
        
        timer.Tick(1.5f);
        Assert.AreEqual(timer.RemainingTime, 0.5f);
        
        timer.Tick(0.5f);
        Assert.AreEqual(timer.RemainingTime, 1f);
        
    }
   
}
