using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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
            new Location(0, 5),
            new Location(5, 5),
            new Location(5, 0),
            new Location(0, 0)
        };
    }
    
    [Test]
    public void RouteConstructorAndInitialize()
    {
        Route route = new Route(_testPath);
        
        Assert.AreEqual(2, route.Vertices.Count);
        
        Assert.AreEqual(new Location(0, 0), route.CurrentPosition);
        
        Assert.AreEqual(new Location(5, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 0), route.CurrentVertex);
        Assert.AreEqual(new Location(0, 5), route.NextVertex);
    }

    [Test]
    public void StepTest()
    {
        Route route = new Route(_testPath);
        
        Assert.AreEqual(new Location(0, 0), route.CurrentPosition);
        
        route.Step();
        
        Assert.AreEqual(new Location(0, 1), route.CurrentPosition);
        
        Assert.AreEqual(new Location(5, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 0), route.CurrentVertex);  
        Assert.AreEqual(new Location(0, 5), route.NextVertex);
        
        route.Step();
        route.Step();
        route.Step();
        route.Step();
        
        Assert.AreEqual(new Location(0, 5), route.CurrentPosition);
        
        Assert.AreEqual(new Location(0, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 5), route.CurrentVertex);  
        Assert.AreEqual(new Location(5, 5), route.NextVertex);
        
        route.Step();
        Assert.AreEqual(new Location(1, 5), route.CurrentPosition);
        
        Assert.AreEqual(new Location(0, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 5), route.CurrentVertex);  
        Assert.AreEqual(new Location(5, 5), route.NextVertex);
        
    }

    [Test]
    public void StepVertexTest()
    {
        Route route = new Route(_testPath);
        
        route.StepVertex();
        
        Assert.AreEqual(new Location(0, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 5), route.CurrentVertex);  
        Assert.AreEqual(new Location(5, 5), route.NextVertex);
    }

    [Test]
    public void ContainsVertexTest()
    {
        Route route = new Route(_testPath);
        
        Assert.True(route.ContainsVertex(new Location(0, 0)));
        Assert.True(route.ContainsVertex(new Location(0, 5)));
        Assert.False(route.ContainsVertex(new Location(50, 5)));
    }
}
