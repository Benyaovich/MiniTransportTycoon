using System.Collections.Generic;
using System.Numerics;
using Model.Cells.Grid;
using NUnit.Framework;

public class ForestSpreadManagerTests
{
    private Grid<ModelGridObject> _grid;
    private CellBuildingManager _buildingManager;
    private ForestSpreadManager _spreadManager;

    [SetUp]
    public void Init()
    {
        _grid = new Grid<ModelGridObject>(new Size(3, 3), 1, Vector3.Zero,
            (grid, location) => new ModelGridObject(grid, location));
        _buildingManager = new CellBuildingManager(
            _grid,
            new DynamicRoadBuildingManager(_grid),
            new CityService(),
            new List<IAdvancable>());
        _spreadManager = new ForestSpreadManager(_grid, _buildingManager);
    }

    [Test]
    public void AddedForestSpreadsToTheOnlyValidNeighbour()
    {
        Forest forest = new Forest(new Location(1, 1));

        _grid.GetGridObject(1, 0).SetModel(new Forest(new Location(1, 0)));
        _grid.GetGridObject(1, 2).SetModel(new Forest(new Location(1, 2)));
        _grid.GetGridObject(0, 1).SetModel(new Forest(new Location(0, 1)));

        _spreadManager.AddForest(forest);
        _spreadManager.AddForest(forest);

        forest.OnSpread?.Invoke(forest, forest.Origin);

        Assert.IsInstanceOf<Forest>(_grid.GetGridObject(2, 1).Model);

        _spreadManager.RemoveForest(forest);
        _spreadManager.RemoveForest(forest);
    }
}
