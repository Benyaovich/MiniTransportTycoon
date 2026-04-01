using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Model.Cells.Grid;
using Model.Cells.RoadCells;
using Model.Enumerations;
using NUnit.Framework;
using NUnit.Framework.Internal;

public class GraphBuilderTestsWithDynamicRoads
{
    private GraphBuilder _graphBuilder;
    private Grid<ModelGridObject> _grid;
    private IGraph _graph;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private CellBuildingManager _cellBuildingManager;
    [SetUp]
    public void Init()
    {
        _grid= new Grid<ModelGridObject>(new Size(5, 5), 10, Vector3.Zero,
            (g, l) => new ModelGridObject(g,l));
        _graph = new Graph();
        
        _graphBuilder = new GraphBuilder(_grid, _graph);

        _dynamicRoadBuildingManager = new DynamicRoadBuildingManager(_grid);
        _cellBuildingManager = new CellBuildingManager(_grid, _dynamicRoadBuildingManager,
            new CityService(), new List<IAdvancable>());
    }


    [Test]
    public void AddVertexAfterBuildingAVertexPointRoadCell()
    {
        RoadCell road = new TwoWayCornerDL(new Location(0, 0));
        var gridObject00 = _grid.GetGridObject(0,0);
        gridObject00.SetModel(road);
        
        _graphBuilder.CreateConnectionsAt(road);
        Assert.AreEqual(1, _graph.Vertices.Count);
    }

    [Test]
    public void GetNextVertexInUpDirectionWhenThereIsNone()
    {
        var road = CreateRoadCell(new Location(0,3));
        CreateRoadCell(new Location(0, 2));
        CreateRoadCell(new Location(0, 1));
        _cellBuildingManager.TryBuild(new Forest(new Location(0, 0)));
        
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Up));
    }
    
    [Test]
    public void GetNextVertexInUpDirectionWhenExists()
    {
        var road = CreateRoadCell(new Location(0,0));
        CreateRoadCell(new Location(0, 1));
        CreateRoadCell(new Location(0, 2));
        var vertex = CreateRoadCell(new Location(0, 3));
        CreateRoadCell(new Location(1, 3));
        _graph.AddVertex(vertex.Origin);
        
        Assert.AreEqual(vertex.Origin, _graphBuilder.GetNextVertexInDirection(road, Direction.Up));
    }
    
    [Test]
    public void GetNextVertexInRightDirectionWhenThereIsNone()
    {
        var road = CreateRoadCell(new Location(0,1));
        CreateRoadCell(new Location(1, 1));
        CreateRoadCell(new Location(2, 1));
        _cellBuildingManager.TryBuild(new Forest(new Location(3, 1)));
        
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Right));
    }
    
    [Test]
    public void GetNextVertexInRightDirectionWhenExists()
    {
        var road = CreateRoadCell(new Location(0,1));
        CreateRoadCell(new Location(1, 1));
        CreateRoadCell(new Location(2, 1));
        var vertex = CreateRoadCell(new Location(3, 1));
        CreateRoadCell(new Location(3, 2));
        _graph.AddVertex(vertex.Origin);
        
        Assert.AreEqual(vertex.Origin, _graphBuilder.GetNextVertexInDirection(road, Direction.Right));
    }

    [Test]
    public void CannotMoveInDirectionWhereRoadDoesntLeadTo()
    {
        var road = CreateRoadCell(new Location(0,1));
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Right));

        road = CreateRoadCell(new Location(0, 0));
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Down));
    }

    [Test]
    public void GetConnectedVerticesVertical()
    {
        CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(0, 1));
        var road = CreateRoadCell(new Location(0,2));
        CreateRoadCell(new Location(1,2));
        CreateRoadCell(new Location(0, 3));
        CreateRoadCell(new Location(0, 4));
        CreateRoadCell(new Location(1, 4));
        var map = _graphBuilder.GetConnectedVertices(road);
        Assert.AreEqual(new Location(0,0), map[Direction.Down]);
        Assert.AreEqual(new Location(0,4), map[Direction.Up]);
        Assert.IsNull(map[Direction.Left]);
        Assert.IsNull(map[Direction.Right]);
    }
    
    [Test]
    public void GetConnectedVerticesHorizontal()
    {
        CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(0, 1));
        CreateRoadCell(new Location(1, 0));
        var road = CreateRoadCell(new Location(2,0));
        CreateRoadCell(new Location(2,1));
        CreateRoadCell(new Location(3,0));
        CreateRoadCell(new Location(4,0));
        CreateRoadCell(new Location(4,1));
        var map = _graphBuilder.GetConnectedVertices(road);
        Assert.AreEqual(new Location(0,0), map[Direction.Left]);
        Assert.AreEqual(new Location(4,0), map[Direction.Right]);
        Assert.IsNull(map[Direction.Up]);
        Assert.IsNull(map[Direction.Down]);
    }

    [Test]
    public void AddEdgesToGraphVertical()
    {
        var vertex1 = CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(0, 1));
        var road = CreateRoadCell(new Location(0,2));
        CreateRoadCell(new Location(1,2));
        CreateRoadCell(new Location(0, 3));
        var vertex2 = CreateRoadCell(new Location(0, 4));
        CreateRoadCell(new Location(1, 4));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        _graph.AddVertex(road.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        
        _graphBuilder.AddEdgesToGraph(road, map);
        
        Assert.AreEqual(2, _graph.Edges.Count);

        var edge1 = _graph.Edges.Find(x => x == new Edge(new Location(0, 0), new Location(0, 2)));
        Assert.IsNotNull(edge1);
        var edge2 = _graph.Edges.Find(x => x == new Edge(new Location(0, 2), new Location(0, 4)));
        Assert.IsNotNull(edge2);
    }
    
    [Test]
    public void AddEdgesToGraphHorizontal()
    {
        var vertex1 = CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(0, 1));
        CreateRoadCell(new Location(1,0));
        var road = CreateRoadCell(new Location(2,0));
        CreateRoadCell(new Location(2,1));
        CreateRoadCell(new Location(3,0));
        var vertex2 = CreateRoadCell(new Location(4,0));
        CreateRoadCell(new Location(4,1));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        _graph.AddVertex(road.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        
        _graphBuilder.AddEdgesToGraph(road, map);
        
        Assert.AreEqual(2, _graph.Edges.Count);

        var edge1 = _graph.Edges.Find(x => x == new Edge(new Location(0, 0), new Location(2,0)));
        Assert.IsNotNull(edge1);
        var edge2 = _graph.Edges.Find(x => x == new Edge(new Location(2,0), new Location(4,0)));
        Assert.IsNotNull(edge2);
    }

    [Test]
    public void BuildRoadTestsVertical()
    {
        var vertex1 = CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(0, 1));
        var road = CreateRoadCell(new Location(0,2));
        CreateRoadCell(new Location(1,2));
        CreateRoadCell(new Location(0, 3));
        var vertex2 = CreateRoadCell(new Location(0, 4));
        CreateRoadCell(new Location(1, 4));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        Assert.AreEqual(2, _graph.Vertices.Count);
        
        _graphBuilder.CreateConnectionsAt(road);
        
        Assert.AreEqual(2, _graph.Edges.Count);
        Assert.AreEqual(3, _graph.Vertices.Count);

        var edge1 = _graph.Edges.Find(x => x == new Edge(new Location(0, 0), new Location(0, 2)));
        Assert.IsNotNull(edge1);
        var edge2 = _graph.Edges.Find(x => x == new Edge(new Location(0, 2), new Location(0, 4)));
        Assert.IsNotNull(edge2);
    }
    
    
    [Test]
    public void BuildRoadTestsHorizontal()
    {
        var vertex1 = CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(0, 1));
        CreateRoadCell(new Location(1,0));
        var road = CreateRoadCell(new Location(2,0));
        CreateRoadCell(new Location(2,1));
        CreateRoadCell(new Location(3,0));
        var vertex2 = CreateRoadCell(new Location(4,0));
        CreateRoadCell(new Location(4, 1));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        Assert.AreEqual(2, _graph.Vertices.Count);
        
        _graphBuilder.CreateConnectionsAt(road);
        
        Assert.AreEqual(2, _graph.Edges.Count);
        Assert.AreEqual(3, _graph.Vertices.Count);

        var edge1 = _graph.Edges.Find(x => x == new Edge(new Location(0, 0), new Location(2,0)));
        Assert.IsNotNull(edge1);
        var edge2 = _graph.Edges.Find(x => x == new Edge(new Location(2,0), new Location(4,0)));
        Assert.IsNotNull(edge2);
    }


    [Test]
    public void RemoveEdgeFromGraphIfRoadIsNotVertexVertical()
    {
        RoadCell road1 = CreateRoadCell(new Location(0, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(0, 1));
        _graphBuilder.CreateConnectionsAt(road1);
        RoadCell road2 = CreateRoadCell(new Location(0, 2));
        _graphBuilder.CreateConnectionsAt(road2);
        CreateRoadCell(new Location(0, 3));
        RoadCell road3 = CreateRoadCell(new Location(0, 4));
        CreateRoadCell(new Location(1,4));
        _graphBuilder.CreateConnectionsAt(road3);
        
        Assert.AreEqual(2, _graph.Vertices.Count);
        Assert.AreEqual(1, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(road2);
        Assert.AreEqual(2, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
    }

    [Test]
    public void RemoveEdgeFromGraphIfRoadIsNotVertexHorizontalAndVertical()
    {
        RoadCell road1 = CreateRoadCell(new Location(2, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(2, 1));
        _graphBuilder.CreateConnectionsAt(road1);
        RoadCell road2 = CreateRoadCell(new Location(2, 2));
        CreateRoadCell(new Location(3, 2));
        _graphBuilder.CreateConnectionsAt(road2);
        CreateRoadCell(new Location(2, 3));
        RoadCell road3 = CreateRoadCell(new Location(2, 4));
        CreateRoadCell(new Location(3, 4));
        _graphBuilder.CreateConnectionsAt(road3);

        RoadCell road4 = CreateRoadCell(new Location(0, 2));
        CreateRoadCell(new Location(0, 1));
        RoadCell road5 = CreateRoadCell(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(road4);
        _graphBuilder.CreateConnectionsAt(road5);
        CreateRoadCell(new Location(3, 2));
        RoadCell road6 = CreateRoadCell(new Location(4, 2));
        CreateRoadCell(new Location(4, 1));
        _graphBuilder.CreateConnectionsAt(road6);
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(road5);
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(3, _graph.Edges.Count);
    }
    
    [Test]
    public void RemoveEdgeFromGraphIfRoadIsVertexHorizontalAndVertical()
    {
        RoadCell road1 = CreateRoadCell(new Location(2, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(2, 1));
        _graphBuilder.CreateConnectionsAt(road1);
        RoadCell road2 = CreateRoadCell(new Location(2, 2));
        CreateRoadCell(new Location(3, 2));
        _graphBuilder.CreateConnectionsAt(road2);
        CreateRoadCell(new Location(2, 3));
        RoadCell road3 = CreateRoadCell(new Location(2, 4));
        CreateRoadCell(new Location(3, 4));
        _graphBuilder.CreateConnectionsAt(road3);

        RoadCell road4 = CreateRoadCell(new Location(0, 2));
        CreateRoadCell(new Location(0, 1));
        RoadCell road5 = CreateRoadCell(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(road4);
        _graphBuilder.CreateConnectionsAt(road5);
        CreateRoadCell(new Location(3, 2));
        RoadCell road6 = CreateRoadCell(new Location(4, 2));
        CreateRoadCell(new Location(4, 1));
        _graphBuilder.CreateConnectionsAt(road6);
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(road2);
        Assert.AreEqual(4, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
    }
    
    [Test]
    public void RemoveEdgeFromGraphIfRoadIsVertexHorizontalAndVertical2()
    {
        RoadCell road1 = CreateRoadCell(new Location(2, 0));
        CreateRoadCell(new Location(1, 0));
        CreateRoadCell(new Location(2, 1));
        _graphBuilder.CreateConnectionsAt(road1);
        RoadCell road2 = CreateRoadCell(new Location(2, 2));
        CreateRoadCell(new Location(3, 2));
        _graphBuilder.CreateConnectionsAt(road2);
        CreateRoadCell(new Location(2, 3));
        RoadCell road3 = CreateRoadCell(new Location(2, 4));
        CreateRoadCell(new Location(3, 4));
        _graphBuilder.CreateConnectionsAt(road3);

        RoadCell road4 = CreateRoadCell(new Location(0, 2));
        CreateRoadCell(new Location(0, 1));
        RoadCell road5 = CreateRoadCell(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(road4);
        _graphBuilder.CreateConnectionsAt(road5);
        CreateRoadCell(new Location(3, 2));
        RoadCell road6 = CreateRoadCell(new Location(4, 2));
        CreateRoadCell(new Location(4, 1));
        _graphBuilder.CreateConnectionsAt(road6);
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(road2);
        Assert.AreEqual(4, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);

        RoadCell road7 = CreateRoadCell(new Location(2, 2));
        _graphBuilder.CreateConnectionsAt(road7);
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
    }

    private RoadCell CreateRoadCell(Location location)
    {
        _dynamicRoadBuildingManager.TryBuildRoad(location);
        return _grid.GetGridObject(location).Model as RoadCell;
    }
    
    
    
}


