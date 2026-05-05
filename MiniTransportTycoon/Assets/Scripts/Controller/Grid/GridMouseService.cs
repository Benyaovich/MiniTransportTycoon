#nullable enable
using UnityEngine;
using UniVector3 = UnityEngine.Vector3;

namespace Controller.Grid
{
    public class GridMouseService
    {
        private readonly Grid<ModelGridObject> _grid;

        public GridMouseService(Grid<ModelGridObject> grid)
        {
            _grid = grid;
        }

        public bool TryGetMouseGridLocation(out Location location)
        {
            UniVector3 mousePos = Utils.GetMouseWorldPosition();
            if (mousePos == Vector3.zero)
            {
                location = default!;
                return false;
            }

            _grid.GetXY(mousePos.SV3(), out int x, out int y);
            location = new Location(x, y);
            return true;
        }

        public UniVector3 GetMousePosSnappedToGrid()
        {
            TryGetMouseGridLocation(out Location location);
            return _grid.GetWorldPosition(location.X, location.Y).UVXZ3();
        }
    }
}
