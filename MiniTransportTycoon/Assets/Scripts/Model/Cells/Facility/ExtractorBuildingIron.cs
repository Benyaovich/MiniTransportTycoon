namespace Model.Cells.Facility
{
    public class ExtractorBuildingIron : ExtractorBuilding
    {
        public ExtractorBuildingIron(Location loc, int maxCap = 100, float prodInterval = 10,
            Size size = null, bool destroyable = false, RateChangeHandler rch = null, int resourceAmount = 0)
            : base(Resource.Iron, maxCap, loc, prodInterval, size, destroyable, rch, resourceAmount)
        {
        }
    }
}