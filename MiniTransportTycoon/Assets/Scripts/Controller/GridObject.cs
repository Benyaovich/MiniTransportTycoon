using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Constraints;

public class GridObject
{
    public Grid<GridObject> Grid { get; private set; }
    public Location Location { get; private set; }
    
    public Cell Value { get; private set; }
    private UnityEngine.Transform cellPrefab = null;
    
    
    public GridObject(Grid<GridObject> grid, Location loc)
    {
        Grid = grid;
        Location = loc;
        Value = new Field(Location);
    }

    public void SetValue(Cell cell)
    {
        Value = cell;
        Grid.InvokeOnGridObjectChanged(Location);
    }

    public void ClearValue()
    {
        Value = null;
        Grid.InvokeOnGridObjectChanged(Location);
    }

    public override string ToString()
    {
        return Value!.ToString();
    }

    public bool CanBuild()
    {
        return Value is null or Field;
    }
}