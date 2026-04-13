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
    private PathHandler pathHandler;

    private List<Location> testVertices = new ()
    {
        new Location(0, 0),
        new Location(0, 5),
        new Location(5, 5),
        new Location(5, 0),
        new Location(0, 0)
    };
    
    [SetUp]
    public void Init()
    {
        Graph graph = new Graph();
        
        foreach (var item in testVertices)
        {
            graph.AddVertex(item);
        }
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        graph.AddEdge(new Edge(testVertices[1], testVertices[2]));
        graph.AddEdge(new Edge(testVertices[3], testVertices[4]));
        
        _testPath = new List<Location>
        {
            new Location(0, 0),
            new Location(0, 5),
            new Location(5, 5),
            new Location(5, 0),
            new Location(0, 0)
        };

        pathHandler = new PathHandler(graph);
    }
    
    [Test]
    public void RouteConstructorAndInitialize()
    {
        Route route = new Route(_testPath, pathHandler);
        
        Assert.AreEqual(2, route.Vertices.Count);
        
        Assert.AreEqual(new Location(0, 0), route.CurrentPosition);
        
        Assert.AreEqual(new Location(5, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 0), route.CurrentVertex);
        Assert.AreEqual(new Location(0, 5), route.NextVertex);
    }

    [Test]
    public void StepTest()
    {
        Route route = new Route(_testPath, pathHandler);
        
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
        
        Assert.AreEqual(new Location(0, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 5), route.CurrentVertex);  
        Assert.AreEqual(new Location(5, 5), route.NextVertex);
        
        Assert.AreEqual(new Location(0, 5), route.CurrentPosition);
        
        route.Step();
        
        Assert.AreEqual(new Location(1, 5), route.CurrentPosition);
    }

    [Test]
    public void StepVertexTest()
    {
        Route route = new Route(_testPath, pathHandler);
        
        route.StepVertex();
        
        Assert.AreEqual(new Location(0, 0), route.PreviousVertex); 
        Assert.AreEqual(new Location(0, 5), route.CurrentVertex);  
        Assert.AreEqual(new Location(5, 5), route.NextVertex);
    }

    [Test]
    public void ContainsVertexTest()
    {
        Route route = new Route(_testPath, pathHandler);
        
        Assert.True(route.ContainsVertex(new Location(0, 0)));
        Assert.True(route.ContainsVertex(new Location(0, 5)));
        Assert.False(route.ContainsVertex(new Location(50, 5)));
    }

    [Test]
    public void RecalculationTest()
    {
        Route route = new Route(_testPath, pathHandler);
        
        pathHandler.Graph.RemoveEdge(new Edge(testVertices[1], testVertices[2]));

        for (int i = 0; i < 4; i++)
        {
            route.Step();   
        }
        
        Assert.AreEqual(new Location(5, 0), route.PreviousVertex);
        
        route.Step();
        
        //Assert.AreEqual(new Location(4, 0), route.PreviousVertex);
    }
}
