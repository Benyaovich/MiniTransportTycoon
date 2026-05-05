namespace Model.Cells.Facility
{
    public class ExtractorBuildingWood : ExtractorBuilding
    {
        public ExtractorBuildingWood(Location loc, int maxCap = 500, float prodInterval = 60,
            Size size = null, bool destroyable = false, RateChangeHandler rch = null, int resourceAmount = 0)
            : base(Resource.Wood, maxCap, loc, prodInterval, size, destroyable, rch, resourceAmount)
        {
        }
    }
}