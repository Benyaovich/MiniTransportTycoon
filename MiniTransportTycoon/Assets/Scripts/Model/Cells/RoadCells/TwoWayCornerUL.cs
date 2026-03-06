using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

namespace Model.Cells.RoadCells
{
    public class TwoWayCornerUL : RoadCell
    {
        public TwoWayCornerUL([NotNull] Location origin, bool destroyable = true) 
            : base(origin, false,
                new List<Direction>() { Direction.Up , Direction.Left},
                new Size(1,1), destroyable)
        {
        }
    }
}