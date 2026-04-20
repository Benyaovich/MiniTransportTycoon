using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;


    public class TwoWayUD: RoadCell
    {
        public TwoWayUD([NotNull] Location origin, bool destroyable = true) 
            : base(origin, false,
                new List<Direction>() { Direction.Up , Direction.Down},
                false,
                new Size(1,1), destroyable)
        {
        }
    }
