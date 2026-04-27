using System;
using Model.Cells.Facility;

[Serializable]
public class SExtractorBuilding : SFacility
{
    public SExtractorBuilding(ExtractorBuilding extractorBuilding) : base(extractorBuilding) { }
    public SExtractorBuilding() { }
}

[Serializable]
public class SExtractorBuildingIron : SExtractorBuilding
{
    public SExtractorBuildingIron(ExtractorBuildingIron extractorBuildingIron) : base(extractorBuildingIron) { }
    public SExtractorBuildingIron() { }
}

[Serializable]
public class SExtractorBuildingWood : SExtractorBuilding
{
    public SExtractorBuildingWood(ExtractorBuildingWood extractorBuildingWood) : base(extractorBuildingWood) { }
    public SExtractorBuildingWood() { }
}

[Serializable]
public class SExtractorBuildingCoal : SExtractorBuilding
{
    public SExtractorBuildingCoal(ExtractorBuildingCoal extractorBuildingCoal) : base(extractorBuildingCoal) { }
    public SExtractorBuildingCoal() { }
}