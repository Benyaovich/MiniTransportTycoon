#nullable enable
using System.Collections.Generic;

namespace Model.Cells.Grid
{
    public abstract class BuildingManagerBase
    {
        protected readonly IGrid<ModelGridObject> Grid;
        
        protected BuildingManagerBase(
            IGrid<ModelGridObject> grid)
        {
            Grid = grid;
        }
        
        protected bool CheckIfCanBuild(List<Location> gridPositionList)
        {
            bool canBuild = true;

            foreach (Location position in gridPositionList)
            {
                ModelGridObject? go = Grid.GetGridObject(position.X, position.Y);
                if (go is not null && go.CanBuild()) continue;
                canBuild = false;
                break;
            }

            return canBuild;
        }
        
        protected void SetModelsValueInGridObjects(Cell cell, List<Location> gridPositionList)
        {
            if (gridPositionList.Count == 0) return;

            foreach (Location position in gridPositionList)
            {
                ModelGridObject go = Grid.GetGridObject(position.X, position.Y);
                go.SetModel(cell);
            }
        }
        
        protected void ClearModelFromGridObjects(List<Location> gridPositionList)
        {
            foreach (Location position in gridPositionList)
            {
                ModelGridObject go = Grid.GetGridObject(position.X, position.Y);
                go.ClearModel();
            }
        }
    }
}