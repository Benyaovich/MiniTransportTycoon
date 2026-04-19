using System.Collections.Generic;
using Model.Interfaces;

public class Bus : Vehicle
{
    public Bus(Grid<ModelGridObject> grid, Resource resource = Resource.People, float speed = 1.8f, int maintenanceCost = 100, int purchaseCost = 800, int maxCarryCapacity = 40) 
        : base(grid, resource, speed, maintenanceCost, purchaseCost, maxCarryCapacity) { }

    protected override void LoadResource(IResourceProvider resourceProvider)
    {
        ResourceAmount += resourceProvider.GetResource(MaxCapacity - ResourceAmount);
    }

    protected override void UnloadResource(IDepositPoint depositPoint)
    {
        ResourceAmount = depositPoint.AddResource(ResourceAmount);
    }
    
    protected override bool HandleStationAction(List<Cell> neighbouringCells)
    {
        bool unloaded = TryDepositToNeighbours(neighbouringCells);
        bool loaded = TryLoadFromNeighbours(neighbouringCells);

        return unloaded || loaded;
    }
}