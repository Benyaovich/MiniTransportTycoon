
using System.Collections.Generic;

public class HighlightService
{
    private readonly Grid<GridObject> _grid;

    public HighlightService(Grid<GridObject> grid)
    {
        _grid = grid;
    }
    
    public void EnableHighlight(List<Location> locations)
    {
        foreach (IHighlightable highlightable in GetHighlightableList(locations))    
        {
            highlightable.SetHighlighted(true);
        }
    }
    
    public void DisableHighlight(List<Location> locations)
    {
        foreach (IHighlightable highlightable in GetHighlightableList(locations))    
        {
            highlightable.SetHighlighted(false);
        }
    }

    private List<IHighlightable> GetHighlightableList(List<Location> locations)
    {
        List<IHighlightable> highlightables = new();
        foreach (Location location in locations)
        {
            GridObject gridObject = _grid.GetGridObject(location.X, location.Y);
            if (gridObject?.Model is not IHighlightable highlightable) continue;
            highlightables.Add(highlightable);
        }

        return highlightables;
    }
    
    
}
