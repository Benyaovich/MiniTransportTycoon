using System;
using System.Collections.Generic;
using NUnit.Framework;

public class PathHandlerTests
{
    private PathHandler pathHandler = null!;
    private List<Location> testVertices = new()
    {
        new Location(0, 1),
        new Location(0, 2),
        new Location(1, 1),
        new Location(3, 1),
        new Location(3, 3),
        new Location(5, 3),
            
        new Location(9, 3),
        new Location(9, 4),
        new Location(10, 3),
        new Location(12, 3),
        new Location(12, 5),
        new Location(14, 5),
    };

    [SetUp]
    public void Initialize()
    {
        pathHandler = new PathHandler();
        FillUpGraph();
    }
    
    private void FillUpGraph()
    {
        foreach (var item in testVertices)
        {
            pathHandler.Graph.AddVertex(item);
        }
        pathHandler.Graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[0], testVertices[2]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[2], testVertices[3]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[3], testVertices[4]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[4], testVertices[5]));
         
        pathHandler.Graph.AddEdge(new Edge(testVertices[6], testVertices[7]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[6], testVertices[8]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[8], testVertices[9]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[9], testVertices[10]));
        pathHandler.Graph.AddEdge(new Edge(testVertices[10], testVertices[11]));
    }
    
    [Test]
    public void GetPathFromRouteTest()
    {
        Assert.Throws<ArgumentNullException>(() => { pathHandler.GetPathFromRoute(null); });
        Assert.Throws<ArgumentException>(() =>
        {
            pathHandler.GetPathFromRoute(new List<Location>()
            {
                new Location(0, 1),
                new Location(0,2)
            });
        });
        Assert.Throws<ArgumentException>(() =>
        {
            pathHandler.GetPathFromRoute(new List<Location>()
            {
                new Location(0, 1),
                new Location(0,2),
                new Location(0,2)
            });
        });
        List<Location> route = new List<Location>()
        {
            new Location(0, 2),
            new Location(3, 3),
            new Location(0, 2)
        };
        List<Location> expectedPath = new List<Location>()
        {
            new Location(0, 2),
            new Location(0, 1),
            new Location(1, 1),
            new Location(3, 1),
            new Location(3, 3),
            new Location(3, 1),
            new Location(1, 1),
            new Location(0, 1),
        };
        CollectionAssert.AreEqual(expectedPath, pathHandler.GetPathFromRoute(route));
    }
    
}
