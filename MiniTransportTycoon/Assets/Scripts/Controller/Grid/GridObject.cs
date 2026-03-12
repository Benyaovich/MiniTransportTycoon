using JetBrains.Annotations;
using Model.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class GridObject : IHasCellModel
{
    public Grid<GridObject> Grid { get; private set; }
    public Location Location { get; private set; }

    [CanBeNull] public Cell Model { get; private set; } = null;
    [CanBeNull] public Transform Visual { get; private set; } = null;
    
    
    public GridObject(Grid<GridObject> grid, Location loc)
    {
        Grid = grid;
        Location = loc;
    }

    public void SetModel(Cell cell)
    {
        Model = cell;
        Grid.InvokeOnGridObjectChanged(Location);
    }

    public void SetVisual(Transform visual)
    {
        Visual = visual;
    }


    public void ClearModel()
    {
        Model = null;
        Grid.InvokeOnGridObjectChanged(Location);
    }
    public void DestroyModel()
    {
        if(Model is IDestroyable destroyable){ destroyable.Destroy(); }
        ClearModel();
    }

    public void ClearVisual()
    {
        Visual = null;
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