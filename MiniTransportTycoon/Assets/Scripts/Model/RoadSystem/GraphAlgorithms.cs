using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.RoadSystem
{
    public static class GraphAlgorithms
    {
        public static Dictionary<Location, SearchNode> Dijkstra(Location startLocation, Graph reachableGraph)
        {
            Dictionary<Location, SearchNode> dijkstra = new();
            foreach (Location v in reachableGraph.Vertices) dijkstra[v] = new SearchNode(Int32.MaxValue, null);
            dijkstra[startLocation].Distance = 0;
            List<QueueNode> q = new();
            foreach (Location possibleVertex in reachableGraph.Vertices)
            {
                if (possibleVertex == startLocation) continue;
                q.Add(new QueueNode(dijkstra[possibleVertex].Distance, possibleVertex));
            }
            Location u = startLocation;
            while (dijkstra[u].Distance < Int32.MaxValue && q.Count != 0)
            {
                foreach (Location v in reachableGraph.GetNeighbours(u))
                {
                    int w = reachableGraph.Edges.Find(x => (x.A == u && x.B == v) || (x.A == v && x.B == u)).W;
                    if (dijkstra[v].Distance > dijkstra[u].Distance + w)
                    {
                        dijkstra[v].Parent = u;
                        dijkstra[v].Distance = dijkstra[u].Distance + w;
                        int index = q.FindIndex(x => x.Vertex == v);
                        if (index != -1)
                        {
                            var item = q[index];
                            item.Distance = dijkstra[v].Distance;
                            q[index] = item;
                        }
                    }
                }
                QueueNode min = q.OrderBy(x => x.Distance).First();
                u = min.Vertex;
                q.Remove(min);
            }
            return dijkstra;
        }
        
        public static Dictionary<Location, SearchNode> Bfs(Location startLocation, Graph graph)
        {
            Dictionary<Location,SearchNode> bfs = new();
            foreach (Location vertex in graph.Vertices)
            {
                bfs[vertex] = new SearchNode(Int32.MaxValue, null);
            }
            bfs[startLocation].Distance = 0;
            Queue<Location> q = new Queue<Location>();
            q.Enqueue(startLocation);
            while (q.Count != 0)
            {
                Location u = q.Dequeue();
                foreach (Location v in graph.GetNeighbours(u))
                {
                    if (bfs[v].Distance == Int32.MaxValue)
                    {
                        bfs[v].Distance = bfs[u].Distance + 1;
                        bfs[v].Parent = u;
                        q.Enqueue(v);
                    }
                }
            }
            return bfs;
        }
        
        public static Graph GetReachableGraphFromBfsTable(Graph originalGraph, Dictionary<Location,SearchNode> bfs)
        {
            var vertices = bfs
                .Where(x => x.Value.Distance != Int32.MaxValue)
                .Select(x => x.Key)
                .ToList();
            var edges =
                originalGraph.Edges
                    .Where(x => vertices.Contains(x.A) || vertices.Contains(x.B))
                    .ToList();
            return new Graph(edges,vertices);
        }
    }
}