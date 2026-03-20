using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Model.Cells.RoadCells;
using Model.Enumerations;
using NUnit.Framework;
using NUnit.Framework.Internal;

public class RouteTests
{
    private List<Location> _testPath;
    
    [SetUp]
    public void Init()
    {
        _testPath = new List<Location>
        {
            new Location(0, 0),
            new Location(0, 1),
            new Location(1, 1),
            new Location(1, 0),
            new Location(0, 0)
        };
    }
    
    [Test]
    public void RouteConstructorAndInitialize()
    {
        Route route = new Route(_testPath);
        
        Assert.AreEqual(2, route.Vertices.Count);
        
        Assert.AreEqual(new Location(1, 0), route.PreviousLocation); 
        Assert.AreEqual(new Location(0, 0), route.CurrentLocation);  
        Assert.AreEqual(new Location(0, 1), route.NextLocation);
    }

    [Test]
    public void StepVertexTest()
    {
        Route route = new Route(_testPath);
        
        route.StepVertex();
        
        Assert.AreEqual(new Location(0, 0), route.PreviousLocation); 
        Assert.AreEqual(new Location(0, 1), route.CurrentLocation);  
        Assert.AreEqual(new Location(1, 1), route.NextLocation);
    }

    [Test]
    public void ContainsVertexTest()
    {
        Route route = new Route(_testPath);
        
        Assert.True(route.ContainsVertex(new Location(0, 0)));
        Assert.True(route.ContainsVertex(new Location(0, 1)));
        Assert.False(route.ContainsVertex(new Location(50, 5)));
    }
}
