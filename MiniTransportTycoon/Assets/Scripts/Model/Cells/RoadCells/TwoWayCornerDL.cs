using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

namespace Model.Cells.RoadCells
{
    public class TwoWayCornerDL : RoadCell
    {
        public TwoWayCornerDL([NotNull] Location origin, bool destroyable = true) 
            : base(origin, false,
                new List<Direction>() { Direction.Down , Direction.Left},
                new Size(1,1), destroyable)
        {
        }
        
    }
}