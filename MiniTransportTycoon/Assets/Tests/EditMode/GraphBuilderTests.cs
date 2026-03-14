using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Model.Cells.RoadCells;
using Model.Enumerations;
using NUnit.Framework;
using NUnit.Framework.Internal;

public class GraphBuilderTests
{
    private GraphBuilder<MockGridObject> _graphBuilder;
    private Grid<MockGridObject> _grid;
    private IGraph _graph;
    [SetUp]
    public void Init()
    {
        _grid= new Grid<MockGridObject>(new Size(5, 5), 10, Vector3.Zero,
            (g, l) => new MockGridObject(g,l));
        _graph = new Graph();
        
        _graphBuilder = new GraphBuilder<MockGridObject>(_grid, _graph);
    }
        
    [Test]
    public void WhenModelIsNullOrNotRoadCellDoNothing()
    {
        
        Forest forest = new Forest(new Location(0, 0));
        var gridObject00 = _grid.GetGridObject(0,0);
        gridObject00.SetModel(forest);
        
        Assert.AreEqual(0, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
        _graphBuilder.CreateConnectionsAt(forest.Origin);
        Assert.AreEqual(0, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);

        gridObject00.ClearModel();
        Assert.AreEqual(0, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
        _graphBuilder.CreateConnectionsAt(new Location(0,0));
        Assert.AreEqual(0, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
    }


    [Test]
    public void AddVertexWhenAfterBuildingAVertexPointRoadCell()
    {
        RoadCell road = new TwoWayCornerDL(new Location(0, 0));
        var gridObject00 = _grid.GetGridObject(0,0);
        gridObject00.SetModel(road);
        
        _graphBuilder.CreateConnectionsAt(road.Origin);
        Assert.AreEqual(1, _graph.Vertices.Count);
    }

    [Test]
    public void GetNextVertexInUpDirectionWhenThereIsNone()
    {
        var road = CreateTwoWayCornerDLRoad(new Location(0,3));
        CreateTwoWayUDRoad(new Location(0, 2));
        CreateTwoWayUDRoad(new Location(0, 1));
        CreateForest(new Location(0, 0));
        
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Up));
    }
    
    [Test]
    public void GetNextVertexInUpDirectionWhenExists()
    {
        var road = CreateTwoWayCornerURRoad(new Location(0,0));
        CreateTwoWayUDRoad(new Location(0, 1));
        CreateTwoWayUDRoad(new Location(0, 2));
        var vertex = CreateTwoWayCornerURRoad(new Location(0, 3));
        _graph.AddVertex(vertex.Origin);
        
        Assert.AreEqual(vertex.Origin, _graphBuilder.GetNextVertexInDirection(road, Direction.Up));
    }
    
    [Test]
    public void GetNextVertexInRightDirectionWhenThereIsNone()
    {
        var road = CreateTwoWayCornerURRoad(new Location(0,1));
        CreateTwoWayLRRoad(new Location(1, 1));
        CreateTwoWayLRRoad(new Location(2, 1));
        CreateForest(new Location(3, 1));
        
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Right));
    }
    
    [Test]
    public void GetNextVertexInRightDirectionWhenExists()
    {
        var road = CreateTwoWayCornerURRoad(new Location(0,1));
        CreateTwoWayLRRoad(new Location(1, 1));
        CreateTwoWayLRRoad(new Location(2, 1));
        var vertex = CreateTwoWayCornerDLRoad(new Location(3, 1));
        _graph.AddVertex(vertex.Origin);
        
        Assert.AreEqual(vertex.Origin, _graphBuilder.GetNextVertexInDirection(road, Direction.Right));
    }

    [Test]
    public void CannotMoveInDirectionWhereRoadDoesntLeadTo()
    {
        var road = CreateTwoWayCornerDLRoad(new Location(0,1));
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Right));

        road = CreateTwoWayCornerURRoad(new Location(0, 0));
        Assert.IsNull(_graphBuilder.GetNextVertexInDirection(road, Direction.Down));
    }

    [Test]
    public void GetConnectedVerticesVertical()
    {
        CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayUDRoad(new Location(0, 1));
        var road = CreateTwoWayUDRoad(new Location(0,2));
        CreateTwoWayUDRoad(new Location(0, 3));
        CreateTwoWayCornerDLRoad(new Location(0, 4));
        var map = _graphBuilder.GetConnectedVertices(road);
        Assert.AreEqual(new Location(0,0), map[Direction.Down]);
        Assert.AreEqual(new Location(0,4), map[Direction.Up]);
        Assert.IsNull(map[Direction.Left]);
        Assert.IsNull(map[Direction.Right]);
    }
    
    [Test]
    public void GetConnectedVerticesHorizontal()
    {
        CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayLRRoad(new Location(1, 0));
        var road = CreateTwoWayLRRoad(new Location(2,0));
        CreateTwoWayLRRoad(new Location(3,0));
        CreateTwoWayCornerDLRoad(new Location(4,0));
        var map = _graphBuilder.GetConnectedVertices(road);
        Assert.AreEqual(new Location(0,0), map[Direction.Left]);
        Assert.AreEqual(new Location(4,0), map[Direction.Right]);
        Assert.IsNull(map[Direction.Up]);
        Assert.IsNull(map[Direction.Down]);
    }

    [Test]
    public void AddEdgesToGraphVertical()
    {
        var vertex1 = CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayUDRoad(new Location(0, 1));
        var road = CreateFourWayRoad(new Location(0,2));
        CreateTwoWayUDRoad(new Location(0, 3));
        var vertex2 = CreateTwoWayCornerDLRoad(new Location(0, 4));
        
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
        var vertex1 = CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayLRRoad(new Location(1,0));
        var road = CreateFourWayRoad(new Location(2,0));
        CreateTwoWayLRRoad(new Location(3,0));
        var vertex2 = CreateTwoWayCornerDLRoad(new Location(4,0));
        
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
        var vertex1 = CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayUDRoad(new Location(0, 1));
        var road = CreateFourWayRoad(new Location(0,2));
        CreateTwoWayUDRoad(new Location(0, 3));
        var vertex2 = CreateTwoWayCornerDLRoad(new Location(0, 4));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        Assert.AreEqual(2, _graph.Vertices.Count);
        
        _graphBuilder.CreateConnectionsAt(new Location(0,2));
        
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
        var vertex1 = CreateTwoWayCornerURRoad(new Location(0, 0));
        CreateTwoWayLRRoad(new Location(1,0));
        var road = CreateFourWayRoad(new Location(2,0));
        CreateTwoWayLRRoad(new Location(3,0));
        var vertex2 = CreateTwoWayCornerDLRoad(new Location(4,0));
        
        var map = _graphBuilder.GetConnectedVertices(road);
        _graph.AddVertex(vertex1.Origin);
        _graph.AddVertex(vertex2.Origin);
        
        Assert.AreEqual(0, _graph.Edges.Count);
        Assert.AreEqual(2, _graph.Vertices.Count);
        
        _graphBuilder.CreateConnectionsAt(new Location(2,0));
        
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
        CreateFourWayRoad(new Location(0, 0));
        _graphBuilder.CreateConnectionsAt(new Location(0,0));
        CreateTwoWayUDRoad(new Location(0, 1));
        CreateTwoWayUDRoad(new Location(0, 2));
        _graphBuilder.CreateConnectionsAt(new Location(0,2));
        CreateTwoWayUDRoad(new Location(0, 3));
        CreateFourWayRoad(new Location(0, 4));
        _graphBuilder.CreateConnectionsAt(new Location(0,4));
        
        Assert.AreEqual(2, _graph.Vertices.Count);
        Assert.AreEqual(1, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(new Location(0,2));
        Assert.AreEqual(2, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
    }

    [Test]
    public void RemoveEdgeFromGraphIfRoadIsNotVertexHorizontalAndVertical()
    {
        CreateFourWayRoad(new Location(2, 0));
        _graphBuilder.CreateConnectionsAt(new Location(2,0));
        CreateTwoWayUDRoad(new Location(2, 1));
        CreateFourWayRoad(new Location(2, 2));
        _graphBuilder.CreateConnectionsAt(new Location(2,2));
        CreateTwoWayUDRoad(new Location(2, 3));
        CreateFourWayRoad(new Location(2, 4));
        _graphBuilder.CreateConnectionsAt(new Location(2,4));

        CreateFourWayRoad(new Location(0, 2));
        _graphBuilder.CreateConnectionsAt(new Location(0,2));
        CreateTwoWayLRRoad(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(new Location(1,2));
        CreateTwoWayLRRoad(new Location(3, 2));
        CreateFourWayRoad(new Location(4, 2));
        _graphBuilder.CreateConnectionsAt(new Location(4,2));
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(new Location(1,2));
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(3, _graph.Edges.Count);
    }
    
    [Test]
    public void RemoveEdgeFromGraphIfRoadIsVertexHorizontalAndVertical()
    {
        CreateFourWayRoad(new Location(2, 0));
        _graphBuilder.CreateConnectionsAt(new Location(2,0));
        CreateTwoWayUDRoad(new Location(2, 1));
        CreateFourWayRoad(new Location(2, 2));
        _graphBuilder.CreateConnectionsAt(new Location(2,2));
        CreateTwoWayUDRoad(new Location(2, 3));
        CreateFourWayRoad(new Location(2, 4));
        _graphBuilder.CreateConnectionsAt(new Location(2,4));

        CreateFourWayRoad(new Location(0, 2));
        _graphBuilder.CreateConnectionsAt(new Location(0,2));
        CreateTwoWayLRRoad(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(new Location(1,2));
        CreateTwoWayLRRoad(new Location(3, 2));
        CreateFourWayRoad(new Location(4, 2));
        _graphBuilder.CreateConnectionsAt(new Location(4,2));
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(new Location(2,2));
        Assert.AreEqual(4, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);
    }
    
    [Test]
    public void RemoveEdgeFromGraphIfRoadIsVertexHorizontalAndVertical2()
    {
        CreateFourWayRoad(new Location(2, 0));
        _graphBuilder.CreateConnectionsAt(new Location(2,0));
        CreateTwoWayUDRoad(new Location(2, 1));
        CreateFourWayRoad(new Location(2, 2));
        _graphBuilder.CreateConnectionsAt(new Location(2,2));
        CreateTwoWayUDRoad(new Location(2, 3));
        CreateFourWayRoad(new Location(2, 4));
        _graphBuilder.CreateConnectionsAt(new Location(2,4));

        CreateFourWayRoad(new Location(0, 2));
        _graphBuilder.CreateConnectionsAt(new Location(0,2));
        CreateTwoWayLRRoad(new Location(1, 2));
        _graphBuilder.CreateConnectionsAt(new Location(1,2));
        CreateTwoWayLRRoad(new Location(3, 2));
        CreateFourWayRoad(new Location(4, 2));
        _graphBuilder.CreateConnectionsAt(new Location(4,2));
        
        Assert.AreEqual(5, _graph.Vertices.Count);
        Assert.AreEqual(4, _graph.Edges.Count);
        _graphBuilder.RemoveConnectionsAt(new Location(2,2));
        Assert.AreEqual(4, _graph.Vertices.Count);
        Assert.AreEqual(0, _graph.Edges.Count);

        CreateTwoWayUDRoad(new Location(2, 2));
        _graphBuilder.CreateConnectionsAt(new Location(2,2));
        Assert.AreEqual(4, _graph.Vertices.Count);
        Assert.AreEqual(1, _graph.Edges.Count);
    }
    
    #region Helper | Mock
    
    private RoadCell CreateTwoWayCornerDLRoad(Location location)
    {
        RoadCell road = new TwoWayCornerDL(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(road);
        return road;
    }
    
    private RoadCell CreateFourWayRoad(Location location)
    {
        RoadCell road = new FourWay(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(road);
        return road;
    }
    
    private RoadCell CreateTwoWayCornerURRoad(Location location)
    {
        RoadCell road = new TwoWayCornerUR(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(road);
        return road;
    }
    
    private RoadCell CreateTwoWayUDRoad(Location location)
    {
        RoadCell road = new TwoWayUD(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(road);
        return road;
    }
    
    private RoadCell CreateTwoWayLRRoad(Location location)
    {
        RoadCell road = new TwoWayLR(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(road);
        return road;
    }
    private Forest CreateForest(Location location)
    {
        Forest forest = new Forest(location);
        var gridObject = _grid.GetGridObject(location.X, location.Y);
        gridObject.SetModel(forest);
        return forest;
    }
    
    
    
    private class MockGridObject : IHasCellModel
    {
        public Cell Model { get; set; }
        public void SetModel(Cell cell)
        {
            Model = cell;
        }

        public void ClearModel()
        {
            Model = null;
        }

        public MockGridObject(Grid<MockGridObject> g, Location l)
        {
        }
    }

    #endregion
    
}


