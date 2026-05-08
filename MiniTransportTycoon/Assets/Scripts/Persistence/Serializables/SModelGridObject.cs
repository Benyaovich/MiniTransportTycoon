using System;
using Model.Cells.Cities.Houses;
using Model.Cells.Facility;
using UnityEngine;

[Serializable]
public class SModelGridObject
{
    public SCell model;
    public string modelType;
        
    public SModelGridObject(ModelGridObject modelGridObject)
    {
        if (modelGridObject.Model is Forest forest)
            model = new SForest(forest);
        if (modelGridObject.Model is BusStop busStop)
            model = new SBusStop(busStop);
        if (modelGridObject.Model is ExtractorBuildingIron extractorBuildingIron)
            model = new SExtractorBuildingIron(extractorBuildingIron);
        if (modelGridObject.Model is ExtractorBuildingWood extractorBuildingWood)
            model = new SExtractorBuildingWood(extractorBuildingWood);
        if (modelGridObject.Model is ExtractorBuildingCoal extractorBuildingCoal)
            model = new SExtractorBuildingCoal(extractorBuildingCoal);
        if (modelGridObject.Model is ProcessingBuildingSteel processingBuildingSteel)
            model = new SProcessingBuildingSteel(processingBuildingSteel);
        if(modelGridObject.Model is ProcessingBuildingPaper processingBuildingPaper)
            model = new SProcessingBuildingPaper(processingBuildingPaper);
        if (modelGridObject.Model is RoadCell roadCell)
            model = new SRoadCell(roadCell);
        if (modelGridObject.Model is ResidentialBuilding1 residentialBuilding1)
            model = new SResidentialBuilding1(residentialBuilding1);
        if (modelGridObject.Model is ResidentialBuilding2 residentialBuilding2)
            model = new SResidentialBuilding2(residentialBuilding2);
        modelType = modelGridObject.Model!.GetType().ToString();
    }

    public SModelGridObject()
    {
        
    }
}
