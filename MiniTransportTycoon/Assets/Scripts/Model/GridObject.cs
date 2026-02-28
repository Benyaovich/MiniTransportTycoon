using System.Collections.Generic;
using JetBrains.Annotations;

public class GridObject
{
    public Grid<GridObject> Grid { get; private set; }
    public Location Location { get; private set; }
    
    [CanBeNull] public Cell Value { get; private set; }
    
    public bool CanBuild => Value == null;
    
    
    public GridObject(Grid<GridObject> grid, Location loc)
    {
        Grid = grid;
        Location = loc;
        Value = null;
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
        return Location.X + " - " + Location.Y;
    }
}