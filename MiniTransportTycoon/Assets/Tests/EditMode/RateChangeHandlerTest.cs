using System;
using System.Collections;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RateChangeHandlerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void InitialGetValueBetweenSetAmount()
    {
        RateChangeHandler rch = new RateChangeHandler(10, 50, 5, 10f, 25);
        Assert.IsTrue(rch.GetValue() >= 20 && rch.GetValue() <= 30);
    }

    [Test]
    public void ValueBetweenMinAndMaxOverTime()
    {
        RateChangeHandler rch = new RateChangeHandler(10, 100, 25, 1.5f, 45);
        for (int i = 0; i < 100; i++)
        {
            rch.Tick(5f);
            Assert.IsTrue(rch.GetValue() > 10 && rch.GetValue() < 100);
        }
    }

    [Test]
    public void RandomCurrentRateBetweenSetAmount()
    {
        RateChangeHandler rch = new RateChangeHandler(10, 50, 5, 10f);
        Assert.IsTrue(rch.GetValue() > 10 && rch.GetValue() < 50);
    }

    [Test]
    public void CorrectErrorThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new RateChangeHandler(10, 50, 5, 10f, 5));
    }
}
