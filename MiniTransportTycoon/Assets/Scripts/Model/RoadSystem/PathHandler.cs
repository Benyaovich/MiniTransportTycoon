using System;
using System.Collections.Generic;
using System.Linq;

public class PathHandler
{
    public List<Edge> Edges { get; private set; } = new();
    public List<Location> Vertices { get; private set; } = new();
    
    
    public PathHandler(){}

    #region Graph methods

    public List<Location> GetConnectedVerticesList(Location loc)
    {
        Dictionary<Location,(string,int, Location?)> complexVertices = new Dictionary<Location,(string,int, Location)>();
        foreach (Location vertice in Vertices)
        {
            complexVertices[vertice] = ("white", Int32.MaxValue, null);
        }

        complexVertices[loc] = ("grey", 0, null);
        Queue<Location> q = new Queue<Location>();
        q.Enqueue(loc);
        while (q.Count != 0)
        {
            Location u = q.Dequeue();
            Location v;
            foreach (Edge edge in Edges.Where(x=>x.A == u || x.B == u))
            {
                if(edge.A == u ) v = edge.B;
                else v = edge.A;
                if (complexVertices[v].Item2 == Int32.MaxValue)
                {
                    complexVertices[v] = ("grey",complexVertices[u].Item2 + 1, u);
                    q.Enqueue(v);
                }
            }
            complexVertices[u] = ("grey", complexVertices[u].Item2, complexVertices[u].Item3);
        }

        return complexVertices.Where(x => x.Value.Item2 != Int32.MaxValue).Select(x => x.Key).ToList();
    }
    
    #endregion
    

    #region Add | Remove | Contains methods for Lists

    
    public void AddEdge(Edge edge)
    {
        if (ContainsEdge(edge)) return;
        Edges.Add(edge);
    }

    public void RemoveEdge(Edge edge)
    {
        if (!ContainsEdge(edge)) return;
        Edges.Remove(edge);
    }

    public bool ContainsEdge(Edge edge)
    {
        return Edges.Any(x=>x.Equals(edge));
    }
    
    public void AddVertex(Location loc)
    {
        if (ContainsVertex(loc)) return;
        Vertices.Add(loc);
    }

    public void RemoveVertex(Location loc)
    {
        if (!ContainsVertex(loc)) return;
        Vertices.Remove(loc);
    }

    public bool ContainsVertex(Location loc)
    {
        return Vertices.Any(x=>x.Equals(loc));
    } 

    #endregion
    
}
