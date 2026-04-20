using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;


    public class TwoWayCornerDL : RoadCell
    {
        public TwoWayCornerDL([NotNull] Location origin, bool destroyable = true, bool isVertexPoint = true)
            : base(origin, false,
                new List<Direction>() { Direction.Down , Direction.Left},
                isVertexPoint,
                new Size(1,1), destroyable)
        {
        }
        
    }
