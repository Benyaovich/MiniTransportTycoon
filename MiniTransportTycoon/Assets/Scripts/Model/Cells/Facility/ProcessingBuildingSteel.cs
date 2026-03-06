using UnityEngine;

public class ProcessingBuildingSteel : ProcessingBuilding
{
    public ProcessingBuildingSteel(Location loc, int maxCap = 100,
         float prodInterval = 10, Size size = null,
        bool destroyable = false, RateChangeHandler rch = null) 
        : base(Resource.Steel, Resource.Iron, maxCap, loc, prodInterval, size, destroyable, rch)
    {
        
    }
}
