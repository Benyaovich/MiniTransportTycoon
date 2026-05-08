#nullable enable
using System.Collections.Generic;
using Model.Cells.Grid;

namespace Controller.Grid
{
    public class GridBuildService
    {
        private readonly IBuildSelectionManager _buildSelectionManager;
        private readonly CellBuildingManager _cellBuildingManager;
        private readonly DynamicRoadBuildingManager _dynamicRoadBuildingManager;
        private readonly ForestSpreadManager _forestSpreadManager;
        private readonly GridMouseService _gridMouseService;

        public GridBuildService(
            IBuildSelectionManager buildSelectionManager,
            CellBuildingManager cellBuildingManager,
            DynamicRoadBuildingManager dynamicRoadBuildingManager,
            ForestSpreadManager forestSpreadManager,
            GridMouseService gridMouseService)
        {
            _buildSelectionManager = buildSelectionManager;
            _cellBuildingManager = cellBuildingManager;
            _dynamicRoadBuildingManager = dynamicRoadBuildingManager;
            _forestSpreadManager = forestSpreadManager;
            _gridMouseService = gridMouseService;
        }

        public void BuildOnCurrentMousePosition()
        {
            if (!_gridMouseService.TryGetMouseGridLocation(out Location location)) return;
            BuildAt(location);
        }

        public bool CanBuildAt(Location location)
        {
            if (_buildSelectionManager.SelectedObjectType == null) return false;

            if (_buildSelectionManager.SelectedObjectType.CellType == typeof(DynamicRoadCell))
            {
                return _dynamicRoadBuildingManager.CanBuildRoad(location);
            }

            Cell cell = _buildSelectionManager.SelectedObjectType.Create(location);
            cell.SetRotation(_buildSelectionManager.CurrentRotationDegrees);
            return _cellBuildingManager.CanBuild(cell);
        }

        public void BuildOnLocations(List<Location> locations)
        {
            foreach (Location location in locations)
            {
                if (_buildSelectionManager.SelectedObjectType == null) return;

                if (_buildSelectionManager.SelectedObjectType.CellType == typeof(DynamicRoadCell))
                {
                    _dynamicRoadBuildingManager.TryBuildRoad(location);
                }
            }
        }

        private void BuildAt(Location location)
        {
            if (_buildSelectionManager.SelectedObjectType!.CellType == typeof(DynamicRoadCell))
            {
                _dynamicRoadBuildingManager.TryBuildRoad(location);
                return;
            }

            Cell cell = _buildSelectionManager.SelectedObjectType.Create(location);
            cell.SetRotation(_buildSelectionManager.CurrentRotationDegrees);

            if (cell is Forest forest)
            {
                _forestSpreadManager.AddForest(forest);
            }

            _cellBuildingManager.TryBuild(cell);
        }
    }
}
