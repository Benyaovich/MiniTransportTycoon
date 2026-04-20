using UnityEngine;

public class ProcessingBuildingPaper : ProcessingBuilding
{
    public ProcessingBuildingPaper(Location loc, int maxCap = 100,
         float prodInterval = 10, Size size = null,
        bool destroyable = false, RateChangeHandler rch = null) 
        : base(Resource.Paper, Resource.Wood, maxCap, loc, prodInterval, size, destroyable, rch)
    {
        
    }
}
