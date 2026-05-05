
public class ProcessingBuildingPaper : ProcessingBuilding
{
    public ProcessingBuildingPaper(Location loc, int maxCap = 300,
         float prodInterval = 70, Size size = null,
        bool destroyable = false, RateChangeHandler rch = null,int requiredResourceAmount = 0, int resourceAmount = 0) 
        : base(Resource.Paper, Resource.Wood, maxCap, loc, prodInterval, size, destroyable, rch,requiredResourceAmount,resourceAmount)
    {
        
    }
}
