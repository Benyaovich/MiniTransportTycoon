namespace Model.Cells.Facility
{
    public class ExtractorBuildingIron : ExtractorBuilding
    {
        public ExtractorBuildingIron(Location loc, int maxCap = 500, float prodInterval = 60,
            Size size = null, bool destroyable = false, RateChangeHandler rch = null, int resourceAmount = 0)
            : base(Resource.Iron, maxCap, loc, prodInterval, size, destroyable, rch, resourceAmount)
        {
        }
    }
}