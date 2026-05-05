namespace Model.Cells.Facility
{
    public class ExtractorBuildingCoal : ExtractorBuilding
    {
        public ExtractorBuildingCoal(Location loc, int maxCap = 500, float prodInterval = 60,
            Size size = null, bool destroyable = false, RateChangeHandler rch = null,int resourceAmount = 0)
            : base(Resource.Coal, maxCap, loc, prodInterval, size, destroyable, rch, resourceAmount)
        {
        }
    }
}