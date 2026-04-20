using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

namespace Model.Cells.RoadCells
{
    public class ThreeWayURL : RoadCell
    {
        public ThreeWayURL([NotNull] Location origin, bool destroyable = true, bool isVertexPoint = true)
            : base(origin, true,
                new List<Direction>() { Direction.Up ,Direction.Right, Direction.Left},
                isVertexPoint,
                new Size(1,1), destroyable)
        {
        }
    }
}