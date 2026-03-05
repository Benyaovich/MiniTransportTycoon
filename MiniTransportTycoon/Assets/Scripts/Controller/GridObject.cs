using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject
{
    public Grid<GridObject> Grid { get; private set; }
    public Location Location { get; private set; }
    
    public Cell Value { get; private set; }
    [CanBeNull] public Transform CellPrefab { get; private set; } = null;
    
    
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

    public void SetCellPrefab(Transform prefab)
    {
        CellPrefab = prefab;
    }
    
    
    public void ClearValue()
    {
        Value = null;
        Grid.InvokeOnGridObjectChanged(Location);
    }

    private void ClearCellPrefab()
    {
        CellPrefab = null;
    }

    public override string ToString()
    {
        return $"{Location.X} - {Location.Y}\n{Value}";
    }

    public bool CanBuild()
    {
        return Value is null;
    }
}