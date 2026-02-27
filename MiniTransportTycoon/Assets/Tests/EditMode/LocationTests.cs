using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class LocationTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void LocationCreatedCorrectly()
    {
        Location loc = new Location(1, 2);
        Assert.AreEqual(1, loc.X);
        Assert.AreEqual(2, loc.Y);
    }

    [Test]
    public void LocationAdditionWorks()
    {
        Location loc1 = new Location(1, 2);
        Location loc2 = new Location(3, 4);
        
        Assert.AreEqual(4, (loc1 + loc2).X);
        Assert.AreEqual(6, (loc1 + loc2).Y);
    }
    
    [Test]
    public void LocationSubrtactionWorks()
    {
        Location loc1 = new Location(1, 2);
        Location loc2 = new Location(3, 4);
        
        Assert.AreEqual(-2, (loc1 - loc2).X);
        Assert.AreEqual(-2, (loc1 - loc2).Y);
        
        Assert.AreEqual(2, (loc2 - loc1).X);
        Assert.AreEqual(2, (loc2 - loc1).Y);
        
    }
}
