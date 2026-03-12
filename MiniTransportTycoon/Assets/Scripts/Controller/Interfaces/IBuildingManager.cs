
#nullable enable
using System;

public interface IBuildingManager
{
    public event EventHandler<Location>? OnRoadCellBuilt;
    public event EventHandler<Location>? OnRoadCellDemolished;
    public bool TryBuild(CellObjectTypeSO cellObjectTypeSo, Location location);
    public void TryDemolish(Location location);
    public void BuildFromExistingGrid();
}
