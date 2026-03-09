using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;


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
    }

    [Test]
    public void AddEdgeTest()
    {
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,pathHandler.Edges.Count);
        
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,pathHandler.Edges.Count);
    }

    [Test]
    public void RemoveEdgeTest()
    {
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,pathHandler.Edges.Count);
        
        pathHandler.RemoveEdge(new Edge(testVertices[2], testVertices[3]));
        Assert.AreEqual(1,pathHandler.Edges.Count);
        
        pathHandler.RemoveEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(0,pathHandler.Edges.Count);
    }

    [Test]
    public void ContainesEdgeTest()
    {
        Assert.IsFalse(pathHandler.ContainsEdge(new Edge(testVertices[0], testVertices[1])));
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.IsTrue(pathHandler.ContainsEdge(new Edge(testVertices[0], testVertices[1])));
    }

    [Test]
    public void AddVertexTest()
    {
        pathHandler.AddVertex(testVertices[0]);
        Assert.AreEqual(1, pathHandler.Vertices.Count);
        
        pathHandler.AddVertex(testVertices[0]);
        Assert.AreEqual(1, pathHandler.Vertices.Count);
    }

    [Test]
    public void RemoveVertexTest()
    {
        pathHandler.AddVertex(testVertices[0]);
        Assert.AreEqual(1, pathHandler.Vertices.Count);
        
        pathHandler.RemoveVertex(testVertices[1]);
        Assert.AreEqual(1, pathHandler.Vertices.Count);
        
        pathHandler.RemoveVertex(testVertices[0]);
        Assert.AreEqual(0, pathHandler.Vertices.Count);
    }
    
    [Test]
    public void ContainsVertexTest()
    {
        Assert.IsFalse(pathHandler.ContainsVertex(testVertices[0]));
        pathHandler.AddVertex(testVertices[0]);
        Assert.IsTrue(pathHandler.ContainsVertex(testVertices[0]));
    }

    [Test]
    public void GetPathFromRouteTest()
    {
        foreach (var item in testVertices)
        {
            pathHandler.AddVertex(item);
        }
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[2]));
        pathHandler.AddEdge(new Edge(testVertices[2], testVertices[3]));
        pathHandler.AddEdge(new Edge(testVertices[3], testVertices[4]));
        pathHandler.AddEdge(new Edge(testVertices[4], testVertices[5]));
        
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[7]));
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[8]));
        pathHandler.AddEdge(new Edge(testVertices[8], testVertices[9]));
        pathHandler.AddEdge(new Edge(testVertices[9], testVertices[10]));
        pathHandler.AddEdge(new Edge(testVertices[10], testVertices[11]));


        List<Location> Route = new List<Location>()
        {
            new Location(0, 2),
            new Location(3, 3),
            new Location(0, 2)
        };
        foreach (var item in pathHandler.GetPathFromRoute(Route))
        {
            Debug.Log(item.X + ", " + item.Y);
        }
    }

    [Test]
    public void GetNeighborsTest()
    {
        foreach (var item in testVertices)
        {
            pathHandler.AddVertex(item);
        }
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[2]));
        pathHandler.AddEdge(new Edge(testVertices[2], testVertices[3]));
        pathHandler.AddEdge(new Edge(testVertices[3], testVertices[4]));
        pathHandler.AddEdge(new Edge(testVertices[4], testVertices[5]));
        
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[7]));
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[8]));
        pathHandler.AddEdge(new Edge(testVertices[8], testVertices[9]));
        pathHandler.AddEdge(new Edge(testVertices[9], testVertices[10]));
        pathHandler.AddEdge(new Edge(testVertices[10], testVertices[11]));
        
        Assert.AreEqual(2,pathHandler.GetNeighbours(testVertices[0]).Count);
        Assert.IsTrue(pathHandler.GetNeighbours(testVertices[0]).Any(x=>x.Equals(testVertices[1])));
        Assert.IsTrue(pathHandler.GetNeighbours(testVertices[0]).Any(x=>x.Equals(testVertices[2])));
        
        
        Assert.AreEqual(1,pathHandler.GetNeighbours(new Location(0,2)).Count);
        Assert.IsTrue(new Location(0,1).Equals(pathHandler.GetNeighbours(new Location(0,2)).First()));
    }
    
    [Test]
    public void GetConnectedVerticesListTest()
    {
        
        foreach (var item in testVertices)
        {
            pathHandler.AddVertex(item);
        }
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[1]));
        pathHandler.AddEdge(new Edge(testVertices[0], testVertices[2]));
        pathHandler.AddEdge(new Edge(testVertices[2], testVertices[3]));
        pathHandler.AddEdge(new Edge(testVertices[3], testVertices[4]));
        pathHandler.AddEdge(new Edge(testVertices[4], testVertices[5]));
        
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[7]));
        pathHandler.AddEdge(new Edge(testVertices[6], testVertices[8]));
        pathHandler.AddEdge(new Edge(testVertices[8], testVertices[9]));
        pathHandler.AddEdge(new Edge(testVertices[9], testVertices[10]));
        pathHandler.AddEdge(new Edge(testVertices[10], testVertices[11]));
        
        
        Assert.AreEqual(6, pathHandler.GetConnectedVerticesList(testVertices[0]).Count);

        var getCVL = pathHandler.GetConnectedVerticesList(testVertices[0]);
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(0, 1))));
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(0, 2))));
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(1, 1))));
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(3, 1))));
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(3, 3))));
        Assert.IsTrue(getCVL.Any(x=>x.Equals(new Location(5, 3))));

    }
    
}
