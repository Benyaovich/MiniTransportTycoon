using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

namespace Model.Cells.RoadCells
{
    public class ThreeWayRDL : RoadCell
    {
        public ThreeWayRDL([NotNull] Location origin, bool destroyable = true, bool isVertexPoint = true)
            : base(origin, true,
                new List<Direction>() { Direction.Right ,Direction.Down, Direction.Left},
                isVertexPoint,
                new Size(1,1), destroyable)
        {
        }
    }
}