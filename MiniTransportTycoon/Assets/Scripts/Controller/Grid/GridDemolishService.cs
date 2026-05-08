#nullable enable
using Model.Cells.Grid;
using Model.Interfaces;

namespace Controller.Grid
{
    public class GridDemolishService
    {
        private readonly Grid<ModelGridObject> _grid;
        private readonly CellBuildingManager _cellBuildingManager;
        private readonly DynamicRoadBuildingManager _dynamicRoadBuildingManager;
        private readonly ForestSpreadManager _forestSpreadManager;
        private readonly GridMouseService _gridMouseService;

        public GridDemolishService(
            Grid<ModelGridObject> grid,
            CellBuildingManager cellBuildingManager,
            DynamicRoadBuildingManager dynamicRoadBuildingManager,
            ForestSpreadManager forestSpreadManager,
            GridMouseService gridMouseService)
        {
            _grid = grid;
            _cellBuildingManager = cellBuildingManager;
            _dynamicRoadBuildingManager = dynamicRoadBuildingManager;
            _forestSpreadManager = forestSpreadManager;
            _gridMouseService = gridMouseService;
        }

        public void DemolishAtCurrentMousePosition()
        {
            if (!_gridMouseService.TryGetMouseGridLocation(out Location location)) return;

            ModelGridObject gridObject = _grid.GetGridObject(location.X, location.Y);
            if (gridObject.Model == null) return;

            if (gridObject.Model is Forest forest)
            {
                _forestSpreadManager.RemoveForest(forest);
            }

            if (gridObject.Model is DynamicRoadCell)
            {
                _dynamicRoadBuildingManager.TryDemolishRoad(location);
                return;
            }

            _cellBuildingManager.TryDemolish(location);
        }
    }
}
