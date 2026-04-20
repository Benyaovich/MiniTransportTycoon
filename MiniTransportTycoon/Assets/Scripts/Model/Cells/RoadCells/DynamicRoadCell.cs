
#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Model.Enumerations;

public class DynamicRoadCell : RoadCell
{
    public DynamicRoadCell(Location origin, Dictionary<Direction, Cell?> neighbours,
        Size? size = null, bool destroyable = true)
        : base(origin, false, new List<Direction>(), false, size, destroyable)
    {
        Refresh(neighbours);
    }

    public void Refresh(Dictionary<Direction, Cell?> neighbours)
    {
        CalculateDirections(neighbours);
        IsVertexPoint = CalculateIsVertexPoint(neighbours);
        IsIntersection = neighbours.Count(x => x.Value is DynamicRoadCell) >= 3;
    }

    private void CalculateDirections(Dictionary<Direction, Cell?> neighbours)
    {
        Directions.Clear();
            
        if (neighbours.Count(x => x.Value is DynamicRoadCell) == 0) return;

        foreach (var direction in neighbours.Where(x=>x.Value is DynamicRoadCell).
                     Select(y=>y.Key))
        {
            if (Directions.Contains(direction)) throw new Exception("Ugyan az az irány kétszer szerepel!");
            Directions.Add(direction);
        }
    }

    private bool CalculateIsVertexPoint(Dictionary<Direction, Cell?> neighbours)
    {
        if (neighbours.Any(x => x.Value is IVisitableBuiling)) return true;
        if (Directions.Count == 2 && Directions[0] != Directions[1].Opposite()) return true; // Kanyar
        if (Directions.Count >= 3) return true;
        return false;
    }
}
