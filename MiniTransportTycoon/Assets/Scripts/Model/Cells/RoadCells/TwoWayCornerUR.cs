using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

namespace Model.Cells.RoadCells
{
    public class TwoWayCornerUR : RoadCell
    {
        public TwoWayCornerUR([NotNull] Location origin, bool destroyable = true, bool isVertexPoint = true) 
            : base(origin, false,
                new List<Direction>() { Direction.Up , Direction.Right},
                isVertexPoint,
                new Size(1,1), destroyable)
        {
        }
    }
}