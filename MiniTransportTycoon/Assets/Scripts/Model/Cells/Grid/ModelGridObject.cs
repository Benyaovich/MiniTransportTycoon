using System;
using JetBrains.Annotations;

public class ModelGridObject
{
    public Grid<ModelGridObject> Grid { get; private set; }
    public Location Location { get; private set; }

    [CanBeNull] public Cell Model { get; private set; }
    
    public ModelGridObject(Grid<ModelGridObject> grid, Location loc)
    {
        Grid = grid;
        Location = loc;
    }
    
    public void SetModel(Cell cell)
    {
        Model = cell;
        Grid.InvokeOnGridObjectChanged(Location);
    }
    
    public void ClearModel()
    {
        Model = null;
        Grid.InvokeOnGridObjectChanged(Location);
    }
    
    public override string ToString()
    {
        return $"{Location.X} - {Location.Y}\n{Model}";
    }
    
    public bool CanBuild()
    {
        return Model is null;
    }
}
