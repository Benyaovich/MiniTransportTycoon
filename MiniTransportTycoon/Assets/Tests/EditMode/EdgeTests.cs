using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;
using UnityEngine.DedicatedServer;

public class EdgeTests
{
    [Test]
    public void EdgeConstructorPasses()
    {
        Location aStart = new Location(1, 2);
        Location bStart = new Location(1, 5);
        
        Edge e = new Edge(aStart, bStart);
        
        Assert.AreEqual(e.A, aStart);
        Assert.AreEqual(e.B, bStart);
    }
    
    [Test]
    public void EdgeNotInALine()
    {
        Location aStart = new Location(1, 2);
        Location bStart = new Location(2, 4);

        Assert.Throws<ArgumentException>(() => new Edge(aStart, bStart));

    }
    
    [Test]
    public void EdgeAreTheSame()
    {
        Location aStart = new Location(3, 3);
        Location bStart = new Location(3, 3);

        Assert.Throws<ArgumentException>(() => new Edge(aStart, bStart));
    }
}
