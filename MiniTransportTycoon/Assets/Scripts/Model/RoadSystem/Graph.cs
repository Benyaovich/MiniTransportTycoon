using System.Collections.Generic;
using System.Linq;


    public class Graph
    {
        public List<Edge> Edges { get; private set; } = new();
        public List<Location> Vertices { get; private set; } = new();
        
        public Graph(){}

        public Graph(List<Edge> edges, List<Location> vertices)
        {
            Edges = edges;
            Vertices = vertices;
        }
        
        public List<Location> GetNeighbours(Location vertex)
        {
            return Edges
                .Where(x=>x.A == vertex)
                .Select(x=>x.B)
                .Union(Edges.Where(x=>x.B == vertex)
                .Select(x=>x.A)).Distinct().ToList();
        }
        
        public void AddEdge(Edge edge)
        {
            if (!Edges.Contains(edge)) Edges.Add(edge);
        }

        public void RemoveEdge(Edge edge) => Edges.Remove(edge);
        public bool ContainsEdge(Edge edge) => Edges.Contains(edge);
    
        public void AddVertex(Location vertex)
        {
            if (!Vertices.Contains(vertex)) Vertices.Add(vertex);
        }

        public void RemoveVertex(Location vertex) => Vertices.Remove(vertex);
        public bool ContainsVertex(Location vertex) => Vertices.Contains(vertex);
        
    }
