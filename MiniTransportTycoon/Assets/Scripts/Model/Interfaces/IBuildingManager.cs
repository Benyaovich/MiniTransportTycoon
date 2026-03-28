
#nullable enable
using System;

public interface IBuildingManager
{
    public event EventHandler<Location>? OnRoadCellBuilt;
    public event EventHandler<Location>? OnRoadCellDemolished;
    public event EventHandler<OnModelChangedEventArgs>? OnModelChanged;
    public bool TryBuild(Cell cell);
    public void TryDemolish(Location location);
    public void BuildFromExistingGrid();
}
