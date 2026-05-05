
public class ProcessingBuildingSteel : ProcessingBuilding
{
    public ProcessingBuildingSteel(Location loc, int maxCap = 300,
         float prodInterval = 70, Size size = null,
        bool destroyable = false, RateChangeHandler rch = null,int requiredResourceAmount = 0, int resourceAmount = 0) 
        : base(Resource.Steel, Resource.Iron, maxCap, loc, prodInterval, size, destroyable, rch,requiredResourceAmount,resourceAmount)
    {
        
    }
}
