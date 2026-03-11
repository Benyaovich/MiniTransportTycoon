using System.Collections.Generic;
using NUnit.Framework;

public class GraphTests
{
    private Graph graph;
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
        graph = new Graph();
    }

    private void FillUpGraph()
    {
        foreach (var item in testVertices)
        {
            graph.AddVertex(item);
        }
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        graph.AddEdge(new Edge(testVertices[0], testVertices[2]));
        graph.AddEdge(new Edge(testVertices[2], testVertices[3]));
        graph.AddEdge(new Edge(testVertices[3], testVertices[4]));
        graph.AddEdge(new Edge(testVertices[4], testVertices[5]));
         
        graph.AddEdge(new Edge(testVertices[6], testVertices[7]));
        graph.AddEdge(new Edge(testVertices[6], testVertices[8]));
        graph.AddEdge(new Edge(testVertices[8], testVertices[9]));
        graph.AddEdge(new Edge(testVertices[9], testVertices[10]));
        graph.AddEdge(new Edge(testVertices[10], testVertices[11]));
    }
    
    [Test]
    public void AddEdgeTest()
    {
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,graph.Edges.Count);
        
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,graph.Edges.Count);
    }
    
    [Test]
    public void RemoveEdgeTest()
    {
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(1,graph.Edges.Count);
        
        graph.RemoveEdge(new Edge(testVertices[2], testVertices[3]));
        Assert.AreEqual(1,graph.Edges.Count);
        
        graph.RemoveEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.AreEqual(0,graph.Edges.Count);
    }

    [Test]
    public void ContainsEdgeTest()
    {
        Assert.IsFalse(graph.ContainsEdge(new Edge(testVertices[0], testVertices[1])));
        graph.AddEdge(new Edge(testVertices[0], testVertices[1]));
        Assert.IsTrue(graph.ContainsEdge(new Edge(new Location(0,1), new Location(0, 2))));
    }
    
    [Test]
    public void AddVertexTest()
    {
        graph.AddVertex(testVertices[0]);
        Assert.AreEqual(1, graph.Vertices.Count);
        
        graph.AddVertex(testVertices[0]);
        Assert.AreEqual(1, graph.Vertices.Count);
    }
    
    [Test]
    public void RemoveVertexTest()
    {
        graph.AddVertex(testVertices[0]);
        Assert.AreEqual(1, graph.Vertices.Count);
        
        graph.RemoveVertex(testVertices[1]);
        Assert.AreEqual(1, graph.Vertices.Count);
        
        graph.RemoveVertex(testVertices[0]);
        Assert.AreEqual(0, graph.Vertices.Count);
    }

    [Test]
    public void ContainsVertexTest()
    {
        Assert.IsFalse(graph.ContainsVertex(new Location(0, 1)));
        graph.AddVertex(testVertices[0]);
        Assert.IsTrue(graph.ContainsVertex(new Location(0, 1)));
    }
    
    [Test]
    public void GetNeighborsTest()
    {
        FillUpGraph();
        List<Location> neighbours = graph.GetNeighbours(new Location(0, 1));
        Assert.AreEqual(2,neighbours.Count);
        Assert.IsTrue(neighbours.Contains(new Location(0, 2)));
        Assert.IsTrue(neighbours.Contains(new Location(1, 1)));

        neighbours = graph.GetNeighbours(new Location(0, 2));
        Assert.AreEqual(1,neighbours.Count);
        Assert.AreEqual(new Location(0,1) ,neighbours[0]);
    }
    
}
