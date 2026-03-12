using System.Collections.Generic;
using JetBrains.Annotations;
using Model.Enumerations;

public class FourWay : RoadCell
{
    public FourWay([NotNull] Location origin, bool isIntersection = true,
        bool isVertexPoint = true,
        [CanBeNull] Size size = null, bool destroyable = true) 
        : base(origin, isIntersection, new List<Direction>(){Direction.Up, Direction.Down, Direction.Left, Direction.Right},
            isVertexPoint, size, destroyable)
    {
        
    }
}
