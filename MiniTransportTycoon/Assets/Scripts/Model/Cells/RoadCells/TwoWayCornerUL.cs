using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;


    public class TwoWayCornerUL : RoadCell
    {
        public TwoWayCornerUL([NotNull] Location origin, bool destroyable = true, bool isVertexPoint = true) 
            : base(origin, false,
                new List<Direction>() { Direction.Up , Direction.Left},
                isVertexPoint,
                new Size(1,1), destroyable)
        {
        }
    }
