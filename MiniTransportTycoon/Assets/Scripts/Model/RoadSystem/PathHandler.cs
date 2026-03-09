using System.Collections.Generic;

public class PathHandler
{
    public List<Edge> Edges { get; private set; } = new();
    public List<Location> Vertices { get; private set; } = new();
    
    
    public PathHandler(){}

    
    

    #region Add | Remove | Contains methods for Lists

    
    public void AddEdge(Edge edge)
    {
        if (Edges.Contains(edge)) return;
        Edges.Add(edge);
    }

    public void RemoveEdge(Edge edge)
    {
        if (!Edges.Contains(edge)) return;
        Edges.Remove(edge);
    }

    public bool ContainsEdge(Edge edge)
    {
        return Edges.Contains(edge);
    }
    
    public void AddVertex(Location loc)
    {
        if (Vertices.Contains(loc)) return;
        Vertices.Add(loc);
    }

    public void RemoveVertex(Location loc)
    {
        if (!Vertices.Contains(loc)) return;
        Vertices.Remove(loc);
    }

    public bool ContainsVertex(Location loc)
    {
        return Vertices.Contains(loc);
    } 

    #endregion
}
