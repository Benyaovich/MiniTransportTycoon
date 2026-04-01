using System.Collections.Generic;
using Model.Cells.Grid;
using Model.Enumerations;
using NUnit.Framework;

public class DynamicRoadBuildingManagerTests
{
    private IGrid<ModelGridObject> _grid;
    private DynamicRoadBuildingManager _dynamicRoadBuildingManager;
    private CellBuildingManager _cellBuildingManager;
    

    [SetUp]
    public void Init()
    {
        _grid = new Grid<ModelGridObject>(new Size(5,5), 10f, new System.Numerics .Vector3(0,0,0),
            (g,l) => new ModelGridObject(g,l));
        _dynamicRoadBuildingManager = new DynamicRoadBuildingManager(_grid);
        _cellBuildingManager = new CellBuildingManager(_grid, _dynamicRoadBuildingManager, new CityService(), new List<IAdvancable>());
    }

    [Test]
    public void PlacingFirstRoadCellHasZeroDirections()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        DynamicRoadCell dynamicRoadCell = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        Assert.AreEqual(0,dynamicRoadCell!.Directions.Count);
    }
    
    [Test]
    public void PlacingFirstRoadCellIsNotVertexPoint()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        DynamicRoadCell dynamicRoadCell = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        Assert.AreEqual(false,dynamicRoadCell!.IsVertexPoint);
    }

    [Test]
    public void TwoRoadCellsConnect()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,0));
        DynamicRoadCell left = _grid.GetGridObject(new Location(0,0)).Model as DynamicRoadCell;
        DynamicRoadCell right = _grid.GetGridObject(new Location(1,0)).Model as DynamicRoadCell;
        
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Right }, left!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Left }, right!.Directions);
    }
    
    [Test]
    public void FourRoadCellsConnect()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(2,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        DynamicRoadCell a = _grid.GetGridObject(new Location(0,0)).Model as DynamicRoadCell;
        DynamicRoadCell b = _grid.GetGridObject(new Location(1,0)).Model as DynamicRoadCell;
        DynamicRoadCell c = _grid.GetGridObject(new Location(2,0)).Model as DynamicRoadCell;
        DynamicRoadCell d = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Right }, a!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Left, Direction.Up, Direction.Right }, b!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Left }, c!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Down }, d!.Directions);
        
    }
    [Test]
    public void FourWayRoadCellConnects()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,1));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(2,1));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,2));
        
        DynamicRoadCell a = _grid.GetGridObject(new Location(0,1)).Model as DynamicRoadCell;
        DynamicRoadCell b = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        DynamicRoadCell c = _grid.GetGridObject(new Location(2,1)).Model as DynamicRoadCell;
        DynamicRoadCell d = _grid.GetGridObject(new Location(1,0)).Model as DynamicRoadCell;
        DynamicRoadCell e = _grid.GetGridObject(new Location(1,2)).Model as DynamicRoadCell;
        
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Right, }, a!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Left, Direction.Up, Direction.Right, Direction.Down }, b!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Left }, c!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Up }, d!.Directions);
        CollectionAssert.AreEquivalent(new List<Direction>{ Direction.Down }, e!.Directions);
        
    }

    [Test]
    public void VertexPointIsFalseWhenThereAreNoNeighbours()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        DynamicRoadCell dynamicRoadCell = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        Assert.IsFalse(dynamicRoadCell!.IsVertexPoint);
    }
    
    [Test]
    public void VertexPointIsFalseWhenRoadNetworkIsALine()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,0));
        DynamicRoadCell a = _grid.GetGridObject(new Location(0,0)).Model as DynamicRoadCell;
        DynamicRoadCell b = _grid.GetGridObject(new Location(1,0)).Model as DynamicRoadCell;
        Assert.IsFalse(a!.IsVertexPoint);
        Assert.IsFalse(b!.IsVertexPoint);
        
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(3,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(3,1));
        DynamicRoadCell c = _grid.GetGridObject(new Location(3,0)).Model as DynamicRoadCell;
        DynamicRoadCell d = _grid.GetGridObject(new Location(3,1)).Model as DynamicRoadCell;
        Assert.IsFalse(c!.IsVertexPoint);
        Assert.IsFalse(d!.IsVertexPoint);
    }

    [Test]
    public void CornerIsVertexPoint()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,0));
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(1,1));
        DynamicRoadCell a = _grid.GetGridObject(new Location(0,0)).Model as DynamicRoadCell;
        DynamicRoadCell b = _grid.GetGridObject(new Location(1,0)).Model as DynamicRoadCell;
        DynamicRoadCell c = _grid.GetGridObject(new Location(1,1)).Model as DynamicRoadCell;
        
        Assert.IsFalse(a!.IsVertexPoint);
        Assert.IsTrue(b!.IsVertexPoint);
        Assert.IsFalse(c!.IsVertexPoint);
    }

    [Test]
    public void SingleRoadIsVertexPointIsNextToIVisitableBuilding()
    {
        ProcessingBuildingSteel pbs = new ProcessingBuildingSteel(new Location(0, 0));
        _cellBuildingManager.TryBuild(pbs);
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(2,0));
        DynamicRoadCell dynamicRoadCell = _grid.GetGridObject(new Location(2,0)).Model as DynamicRoadCell;
        Assert.IsTrue(dynamicRoadCell!.IsVertexPoint);
    }
    
    [Test]
    public void SingleRoadIsVertexPointIsNextToIVisitableBuilding2()
    {
        _dynamicRoadBuildingManager.TryBuildRoad(new Location(0,2));
        DynamicRoadCell dynamicRoadCell = _grid.GetGridObject(new Location(0,2)).Model as DynamicRoadCell;
        ProcessingBuildingSteel pbs = new ProcessingBuildingSteel(new Location(0, 0));
        _cellBuildingManager.TryBuild(pbs);
        Assert.IsTrue(dynamicRoadCell!.IsVertexPoint);
    }
}
