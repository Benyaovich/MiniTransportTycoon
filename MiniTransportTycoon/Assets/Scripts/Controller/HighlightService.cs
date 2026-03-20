
using System.Collections.Generic;

public class HighlightService
{
    private readonly Grid<GridObject> _grid;

    public HighlightService(Grid<GridObject> grid)
    {
        _grid = grid;
    }
    
    private void EnableHighlight(List<IHighlightable> highlightables)
    {
        foreach (IHighlightable highlightable in highlightables)    
        {
            highlightable.SetHighlighted(true);
        }
    }
    public void EnableHighlight(List<Location> locations)
    {
        EnableHighlight(GetHighlightableList(locations));
    }
    
    private void DisableHighlight(List<IHighlightable> highlightables)
    {
        foreach (IHighlightable highlightable in highlightables)    
        {
            highlightable.SetHighlighted(false);
        }
    }
    public void DisableHighlight(List<Location> locations)
    {
        DisableHighlight(GetHighlightableList(locations));
    }

    public void ToggleHighlight(List<Location> locations)
    {
        List<IHighlightable> highlightables = GetHighlightableList(locations);
        if (highlightables.Count == 0) return;
        
        if(highlightables[0].Highlighted){DisableHighlight(highlightables);}
        else if(!highlightables[0].Highlighted){EnableHighlight(highlightables);}
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
