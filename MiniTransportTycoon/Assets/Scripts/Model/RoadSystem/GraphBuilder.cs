#nullable enable
using System.Collections.Generic;
using Model.Enumerations;

public class GraphBuilder<T> : IGraphBuilder where T : IHasCellModel
{
    private readonly IGrid<T> _grid;
    private readonly PathHandler _pathHandler;

    public GraphBuilder(IGrid<T> grid, PathHandler pathHandler)
    {
        _grid = grid;
        _pathHandler = pathHandler;
    }

    

    #region Interface Methods
    
    public void CreateVertex(Location location)
    {
        RoadCell? roadCell = GetCellIfRoad(location);
        if(roadCell is null) return;
        
        if(roadCell.IsVertexPoint){ _pathHandler.Graph.AddVertex(roadCell.Origin); }
        Dictionary<Direction, Location?> vertexInDirectionMap = GetConnectedVertices(roadCell);
        AddEdgesToGraph(roadCell, vertexInDirectionMap);
    }

    public void RemoveVertex(Location location)
    {
        RoadCell? roadCell = GetCellIfRoad(location);
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
            _pathHandler.Graph.AddEdge(new Edge(road.Origin, vertexInDirectionMap[key]));
        }
    }

    private void AddEdgeIfRoadIsNotVertex(Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        if (vertexInDirectionMap[Direction.Up] is not null &&
            vertexInDirectionMap[Direction.Down] is not null)
        {
            _pathHandler.Graph.AddEdge(
                new Edge(vertexInDirectionMap[Direction.Up],
                    vertexInDirectionMap[Direction.Down]));
        }
            
        if (vertexInDirectionMap[Direction.Left] is not null &&
            vertexInDirectionMap[Direction.Right] is not null)
        {
            _pathHandler.Graph.AddEdge(
                new Edge(vertexInDirectionMap[Direction.Left],
                    vertexInDirectionMap[Direction.Right]));
        }
    }

    #endregion
    
    #region Remove Edge
    
    private void RemoveEdgeFromGraph(RoadCell road, Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        if (road.IsVertexPoint)
        {
            _pathHandler.Graph.RemoveVertex(road.Origin);
            return;
        }

        RemoveEdgeIfRoadIsNotVertex(vertexInDirectionMap);
    }

    private void RemoveEdgeIfRoadIsNotVertex(Dictionary<Direction, Location?> vertexInDirectionMap)
    {
        if (vertexInDirectionMap[Direction.Up] is not null &&
            vertexInDirectionMap[Direction.Down] is not null)
        {
            _pathHandler.Graph.RemoveEdge(
                new Edge(vertexInDirectionMap[Direction.Up],
                    vertexInDirectionMap[Direction.Down]));
        }
            
        if (vertexInDirectionMap[Direction.Left] is not null &&
            vertexInDirectionMap[Direction.Right] is not null)
        {
            _pathHandler.Graph.RemoveEdge(
                new Edge(vertexInDirectionMap[Direction.Left],
                    vertexInDirectionMap[Direction.Right]));
        }
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
        if (!CanGoToDirection(road, direction)) return null;
        Location origin = road.Origin;

        // Tobbi pozicio
        do
        {
            origin += direction;
            IHasCellModel gridObject = _grid.GetGridObject(origin);
            if (gridObject?.Model is null) return null;
            if (gridObject.Model is not RoadCell nextRoad) return null;

            if (nextRoad.IsVertexPoint)
            {
                return nextRoad.Origin;
            }
            
            if (!CanGoToDirection(nextRoad, direction)) return null;
            

        }while (true);
    }

    #endregion

    #region Private Methods
    
    private RoadCell? GetCellIfRoad(Location location)
    {
        T gridObject = _grid.GetGridObject(location.X, location.Y);
        if (gridObject is null) return null;
        
        Cell cell = gridObject.Model;
        if (cell is null) return null;
        if( cell is not RoadCell) return null;
        
        RoadCell roadCell = (cell as RoadCell)!;
        return roadCell;
    }

    private bool CanGoToDirection(RoadCell road, Direction direction)
    {
        return road.Directions.Contains(direction);
    }
    
    #endregion
    
}
