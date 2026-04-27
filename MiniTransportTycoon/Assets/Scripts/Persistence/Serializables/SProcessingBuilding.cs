using System;

[Serializable]
public class SProcessingBuilding : SFacility
{
    public int requiredResourceAmount;

    public SProcessingBuilding(ProcessingBuilding processingBuilding) : base(processingBuilding)
    {
        requiredResourceAmount = processingBuilding.RequiredResourceAmount;
    }
    public SProcessingBuilding() { }
}

[Serializable]
public class SProcessingBuildingSteel : SProcessingBuilding
{
    public SProcessingBuildingSteel(ProcessingBuildingSteel processingBuildingSteel) : base (processingBuildingSteel) { } 
    public SProcessingBuildingSteel() { }
}

[Serializable]
public class SProcessingBuildingPaper : SProcessingBuilding
{
    public SProcessingBuildingPaper(ProcessingBuildingPaper processingBuildingPaper) : base (processingBuildingPaper) { } 
    public SProcessingBuildingPaper() { }
}