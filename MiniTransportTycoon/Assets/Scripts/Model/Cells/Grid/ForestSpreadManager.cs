using System;
using System.Collections.Generic;
using Model.Enumerations;

namespace Model.Cells.Grid
{
    public class ForestSpreadManager
    {
        private List<Forest> _forestCells = new();
        private IGrid<ModelGridObject> _grid;
        private readonly Random _random = new();
        private readonly CellBuildingManager _cellBuildingManager;

        public ForestSpreadManager(IGrid<ModelGridObject> grid, CellBuildingManager cellBuildingManager)
        {
            _grid = grid;
            _cellBuildingManager = cellBuildingManager;
        }
        
        public void AddForest(Forest forest)
        {
            if (_forestCells.Contains(forest)) return;
            _forestCells.Add(forest);
            forest.OnSpread += HandleSpread;
        }

        public void RemoveForest(Forest forest)
        {
            if (!_forestCells.Contains(forest)) return;
            _forestCells.Remove(forest);
            forest.OnSpread -= HandleSpread;
        }

        private void HandleSpread(object sender, Location location)
        {
            List<Location> validCells = GetValidNeighbouringCells(location);
            if (validCells.Count == 0) return;
            
            int index = _random.Next(validCells.Count);
            Forest forest = new Forest(validCells[index]);
            _cellBuildingManager.TryBuild(forest);
            AddForest(forest);
        }


        private List<Location> GetValidNeighbouringCells(Location location)
        {
            List<Location> validCells = new();
            
            if(_grid.GetGridObject(location + Direction.Up).Model == null) {validCells.Add(location + Direction.Up);}
            if(_grid.GetGridObject(location + Direction.Down).Model == null) {validCells.Add(location + Direction.Down);}
            if(_grid.GetGridObject(location + Direction.Left).Model == null) {validCells.Add(location + Direction.Left);}
            if(_grid.GetGridObject(location + Direction.Right).Model == null ) {validCells.Add(location + Direction.Right);}
            return validCells;
        }
    }
}