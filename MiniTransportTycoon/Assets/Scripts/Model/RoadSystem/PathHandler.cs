using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PathHandler
{
    public List<Edge> Edges { get; private set; } = new();
    public List<Location> Vertices { get; private set; } = new();
    
    
    public PathHandler(){}

    #region Graph methods

    public List<Location> GetNeighbours(Location location)
    {
        return Edges.Where(x=>x.A.Equals(location)).Select(x=>x.B).Union(Edges.Where(x=>x.B.Equals(location)).Select(x=>x.A)).Distinct().ToList();
    }
    public List<Location> GetPathFromRoute(List<Location> vertices)
    {
        if(vertices.Count < 2 || !vertices.First().Equals(vertices.Last())) throw new Exception("The route must contain at least two different location");
        List<Location> completedRoute = new();
        completedRoute.Add(vertices.First());
        List<Location> reducatedArea = GetConnectedVerticesList(vertices.First());
        for (int j = 0; j < vertices.Count - 1; j++)
        {
            Dictionary<Location, (int, Location?)> dijkstra = new();
            foreach (Location v in reducatedArea)
            {
                dijkstra[v] = (Int32.MaxValue, null);
            }

            dijkstra[vertices[j]] = (0, null);
            List<(Location, int)> q = new();
            foreach (var item in reducatedArea)
            {
                if (item.Equals(vertices[j])) continue;
                q.Add((item, dijkstra[item].Item1));
            }

            Location u = vertices[j];
            while (dijkstra[u].Item1 < Int32.MaxValue && q.Count != 0)
            {
                foreach (var v in GetNeighbours(u))
                {
                    int w = Edges.Find(x => (x.A.Equals(u) && x.B.Equals(v)) || (x.A.Equals(v) && x.B.Equals(u))).W;
                    if (dijkstra[v].Item1 > dijkstra[u].Item1 + w)
                    {
                        dijkstra[v] = (dijkstra[u].Item1 + w, u);
                        int index = q.FindIndex(x => x.Item1.Equals(v));
                        if (index != -1)
                        {
                            var item = q[index];
                            item.Item2 = dijkstra[v].Item1;
                            q[index] = item;
                        }
                    }
                }

                (Location uu, int d) = q.OrderBy(x => x.Item2).First();
                u = uu;
                q.Remove((uu, d));
            }

            List<Location> subRoute = new();
            Location runner = vertices[j + 1];
            subRoute.Add(runner);
            while (!dijkstra[runner].Item2.Equals(vertices[j]))
            {
                subRoute.Add(dijkstra[runner].Item2);
                runner = dijkstra[runner].Item2;
            }
            
            subRoute.Reverse();
            foreach (var item in subRoute)
            {
                completedRoute.Add(item);
            }
        }
        completedRoute.RemoveAt(completedRoute.Count - 1);
        return completedRoute;

    }

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
            foreach (Edge edge in Edges.Where(x=>x.A.Equals(u) || x.B.Equals(u)))
            {
                if(edge.A.Equals(u)) v = edge.B;
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
