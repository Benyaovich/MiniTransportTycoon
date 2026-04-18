namespace Model.Cells.Facility
{
    public class ExtractorBuildingCoal : ExtractorBuilding
    {
        public ExtractorBuildingCoal(Location loc, int maxCap = 100, float prodInterval = 10,
            Size size = null, bool destroyable = false, RateChangeHandler rch = null)
            : base(Resource.Coal, maxCap, loc, prodInterval, size, destroyable, rch)
        {
        }
    }
}