using System.Collections.Generic;

public interface IGraph
{
    public List<Edge> Edges { get; }
    public List<Location> Vertices { get; }
    public void AddEdge(Edge edge);
    public void RemoveEdge(Edge edge);
    public void AddVertex(Location vertex);
    public void RemoveVertex(Location vertex);
    public List<Location> GetNeighbours(Location vertex);
}
