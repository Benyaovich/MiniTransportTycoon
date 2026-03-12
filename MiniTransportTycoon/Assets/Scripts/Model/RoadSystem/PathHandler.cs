using System;
using System.Collections.Generic;
using Model.RoadSystem;

public class PathHandler
{
    public Graph Graph { get; private set; } = new Graph();

    public List<Location> GetPathFromRoute(List<Location> vertices)
    {
        if(vertices == null) throw new ArgumentNullException(nameof(vertices),"Given route cannot be null!");
        if(vertices.Count < 3) throw new ArgumentException("Given route must contain at least two different location!");
        if(vertices[0] != vertices[^1]) throw new ArgumentException("Given route must be circular!");
        
        List<Location> completedRoute = new();
        completedRoute.Add(vertices[0]);
        Graph reachableGraph = GraphAlgorithms.GetReachableGraphFromBfsTable(Graph, GraphAlgorithms.Bfs(vertices[0], Graph));
        for (int j = 0; j < vertices.Count - 1; j++)
        {
            Dictionary<Location,SearchNode> dijkstra = GraphAlgorithms.Dijkstra(vertices[j],reachableGraph);
            List<Location> subRoute = new();
            Location runner = vertices[j + 1];
            subRoute.Add(runner);
            while (dijkstra[runner!].Parent != vertices[j])
            {
                subRoute.Add(dijkstra[runner].Parent);
                runner = dijkstra[runner].Parent;
            }
            subRoute.Reverse();
            foreach (Location loc in subRoute) completedRoute.Add(loc);
        }
        completedRoute.RemoveAt(completedRoute.Count - 1);
        return completedRoute;
    }
}