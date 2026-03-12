#nullable enable
using System;
using System.Collections.Generic;
using Model.Enumerations;

public class GraphBuilder : IGraphBuilder
{
    private readonly IGrid<IHasCellModel> _grid;
    private readonly IGraph _graph;

    public GraphBuilder(IGrid<IHasCellModel> grid, IGraph graph)
    {
        _grid = grid;
        _graph = graph;
    }

    

    #region Interface Methods
    
    public void CreateConnectionsAt(Location location)
    {
        RoadCell? roadCell = GetRoadCell(location);
        if(roadCell is null) return;
        
        if(roadCell.IsVertexPoint){ _graph.AddVertex(roadCell.Origin); }
        Dictionary<Direction, Location?> vertexInDirectionMap = GetConnectedVertices(roadCell);
        AddEdgesToGraph(roadCell, vertexInDirectionMap);
    }

    public void RemoveConnectionsAt(Location location)
    {
        RoadCell? roadCell = GetRoadCell(location);
        if(roadCell is null) return;
        
        Dictionary<Direction, Location?> vertexInDirectionMap = GetConnectedVertices(roadCell);
        RemoveEdgeFromGraph(roadCell, vertexInDirectionMap);
    }

    #endregion

    #region  Add Edges
    
    public void AddEdgesToGraph(RoadCell road, Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        if (road.IsVertexPoint)
        {
            AddEdgeIfRoadIsVertex(road, vertexInDirectionMap);
            return;
        }

        AddEdgeIfRoadIsNotVertex(vertexInDirectionMap);
    }

    private void AddEdgeIfRoadIsVertex(RoadCell road, Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        foreach (Direction key in vertexInDirectionMap.Keys)
        {
            if(vertexInDirectionMap[key] is null) continue;
            _graph.AddEdge(new Edge(road.Origin, vertexInDirectionMap[key]));
        }
    }

    private void AddEdgeIfRoadIsNotVertex(Dictionary<Direction, Location?> vertices)
    {
        ConnectOppositeDirections(vertices, _graph.AddEdge);
    }

    #endregion
    
    #region Remove Edge
    
    private void RemoveEdgeFromGraph(RoadCell road, Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        if (road.IsVertexPoint)
        {
            _graph.RemoveVertex(road.Origin);
            return;
        }

        RemoveEdgeIfRoadIsNotVertex(vertexInDirectionMap);
    }

    private void RemoveEdgeIfRoadIsNotVertex(Dictionary<Direction, Location?> vertices)
    {
        ConnectOppositeDirections(vertices, _graph.RemoveEdge);
    }

    #endregion

    #region Public Methods
    
    public Dictionary<Direction, Location?> GetConnectedVertices(RoadCell road)
    {
        List<Direction> directions = new() { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        Dictionary<Direction, Location?> vertexInDirectionMap = new();
        
        foreach (Direction direction in directions)
        {
            Location? nextVertexInDirection = GetNextVertexInDirection(road, direction);
            vertexInDirectionMap.TryAdd(direction, nextVertexInDirection);
        }

        return vertexInDirectionMap;
    }

    public Location? GetNextVertexInDirection(RoadCell road, Direction direction)
    {
        // Kezdo pozicio kezelese
        if (!HasExitTowards(road, direction)) return null;
        Location origin = road.Origin;

        // Tobbi pozicio
        do
        {
            origin += direction;
            
            RoadCell? nextRoad = GetRoadCell(origin);
            if (nextRoad is null) return null;
            
            if (nextRoad.IsVertexPoint)
            {
                return nextRoad.Origin;
            }
            
            if (!HasExitTowards(nextRoad, direction)) return null;
            

        }while (true);
    }

    #endregion

    #region Private Methods
    
    private void ConnectOppositeDirections(
        Dictionary<Direction, Location?> vertexInDirectionMap,
        Action<Edge> edgeAction)
    {
        TryApplyEdge(vertexInDirectionMap, Direction.Up, Direction.Down, edgeAction);
        TryApplyEdge(vertexInDirectionMap, Direction.Left, Direction.Right, edgeAction);
    }

    private void TryApplyEdge(
        Dictionary<Direction, Location?> vertexInDirectionMap,
        Direction first,
        Direction second,
        Action<Edge> edgeAction)
    {
        Location? from = vertexInDirectionMap[first];
        Location? to = vertexInDirectionMap[second];

        if (from is null || to is null)
            return;

        edgeAction(new Edge(from, to));
    }
    
    private RoadCell? GetRoadCell(Location location)
    {
        IHasCellModel? gridObject = _grid.GetGridObject(location.X, location.Y);
        return gridObject?.Model as RoadCell;
    }

    private bool HasExitTowards(RoadCell road, Direction direction)
    {
        return road.Directions.Contains(direction);
    }
    
    #endregion
    
}
