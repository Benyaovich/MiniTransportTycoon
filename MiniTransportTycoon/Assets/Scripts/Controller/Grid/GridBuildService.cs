#nullable enable
using System.Collections.Generic;
using Model.Cells.Grid;

namespace Controller.Grid
{
    public class GridBuildService
    {
        private readonly CellBuildingManager _cellBuildingManager;
        private readonly DynamicRoadBuildingManager _dynamicRoadBuildingManager;
        private readonly ForestSpreadManager _forestSpreadManager;
        private readonly GridMouseService _gridMouseService;

        public GridBuildService(
            CellBuildingManager cellBuildingManager,
            DynamicRoadBuildingManager dynamicRoadBuildingManager,
            ForestSpreadManager forestSpreadManager,
            GridMouseService gridMouseService)
        {
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

        public void BuildOnLocations(List<Location> locations)
        {
            foreach (Location location in locations)
            {
                if (BuildSelectionManager.Instance.SelectedObjectType == null) return;

                if (BuildSelectionManager.Instance.SelectedObjectType.CellType == typeof(DynamicRoadCell))
                {
                    _dynamicRoadBuildingManager.TryBuildRoad(location);
                }
            }
        }

        private void BuildAt(Location location)
        {
            if (BuildSelectionManager.Instance.SelectedObjectType!.CellType == typeof(DynamicRoadCell))
            {
                _dynamicRoadBuildingManager.TryBuildRoad(location);
                return;
            }

            Cell cell = BuildSelectionManager.Instance.SelectedObjectType.Create(location);

            if (cell is Forest forest)
            {
                _forestSpreadManager.AddForest(forest);
            }

            _cellBuildingManager.TryBuild(cell);
        }
    }
}
